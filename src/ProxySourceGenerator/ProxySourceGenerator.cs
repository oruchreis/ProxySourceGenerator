using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace ProxySourceGenerator;

/// <summary>
/// Proxy Source Generator
/// </summary>
[Generator]
public class ProxySourceGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select classes and interfaces with attributes
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // select the nonabstract class with the [GenerateProxy] attribute
            .Where(static m => m != null) // filter out attributed classes that we don't care about
            .Select(static (m, _) => m!);

        // Combine the selected classes with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<SemanticFounds>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        // Generate the source using the compilation and enums
        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => (node is ClassDeclarationSyntax cs && cs.Modifiers.All(m => !m.IsKind(SyntaxKind.SealedKeyword))) ||
           (node is InterfaceDeclarationSyntax);

    private sealed class SemanticFounds
    {
        public TypeDeclarationSyntax TypeDeclarationSyntax { get; set; } = null!;
        public GenerateProxyAttribute? Attribute { get; set; }
    }

    static SemanticFounds? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        if (context.Node is not TypeDeclarationSyntax typeDeclarationSyntax)
            return null;

        var typeSymbol = context.SemanticModel.GetDeclaredSymbol(typeDeclarationSyntax);
        var attribute = RecursiveFindAttribute(typeSymbol, typeSymbol);

        if (attribute != null)
        {
            if ((typeDeclarationSyntax is ClassDeclarationSyntax classDeclarationSyntax &&
                classDeclarationSyntax.Modifiers.All(m => !m.IsKind(SyntaxKind.SealedKeyword))) ||
                typeDeclarationSyntax is InterfaceDeclarationSyntax)
            {
                return new SemanticFounds
                {
                    TypeDeclarationSyntax = typeDeclarationSyntax,
                    Attribute = attribute,
                };
            }
        }

        return null;

        static GenerateProxyAttribute? RecursiveFindAttribute(INamedTypeSymbol? typeSymbol, INamedTypeSymbol? firstTypeSymbol)
        {
            if (typeSymbol == null)
                return null;

            foreach (var attributeData in typeSymbol.GetAttributes())
            {
                if (attributeData.AttributeClass?.ToDisplayString() == "ProxySourceGenerator.GenerateProxyAttribute")
                {
                    var generateProxyAttribute = new GenerateProxyAttribute();
                    foreach (var argKv in attributeData.NamedArguments)
                    {
                        if (argKv.Key == "GenerateForDerived")
                            generateProxyAttribute.GenerateForDerived = (bool?)argKv.Value.Value ?? false;
                        if (argKv.Key == "AutoGenerateProxyInterface")
                            generateProxyAttribute.AutoGenerateProxyInterface = (bool?)argKv.Value.Value ?? false;
                        if (argKv.Key == "UseInterface")
                            generateProxyAttribute.UseInterface = (bool?)argKv.Value.Value ?? false;
                    }

                    if (SymbolEqualityComparer.IncludeNullability.Equals(typeSymbol, firstTypeSymbol) || generateProxyAttribute.GenerateForDerived)
                        return generateProxyAttribute;
                }
            }

            return typeSymbol switch
            {
                _ when typeSymbol?.BaseType != null && typeSymbol.BaseType.Name != "Object" && typeSymbol.BaseType.ContainingNamespace.Name != "System" => RecursiveFindAttribute(typeSymbol.BaseType, firstTypeSymbol),
                _ when typeSymbol != null && typeSymbol.BaseType == null && typeSymbol.Interfaces.Length > 0 => typeSymbol.Interfaces.Select(interfaceType => RecursiveFindAttribute(interfaceType, firstTypeSymbol)).FirstOrDefault(attr => attr != null),
                _ => null
            };
        }
    }

    static void Execute(Compilation compilation, ImmutableArray<SemanticFounds> semanticFounds, SourceProductionContext context)
    {
        if (semanticFounds.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var semanticFound in semanticFounds.GroupBy(s => s.TypeDeclarationSyntax).Select(g => g.First()))
        {
            var typeDeclarationSyntax = semanticFound.TypeDeclarationSyntax;
            var semanticModel = compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(typeDeclarationSyntax) is not INamedTypeSymbol typeSymbol)
                continue;

            var useInterface = semanticFound.Attribute?.UseInterface ?? false;
            var autoGenerateProxyInterface = semanticFound.Attribute?.AutoGenerateProxyInterface ?? false;
            var interfaceDeclarationSyntax = typeDeclarationSyntax as InterfaceDeclarationSyntax;

            var strBuilder = new StringBuilder();
            var className = interfaceDeclarationSyntax != null ? typeSymbol.Name.TrimStart('I') : typeSymbol.Name;
            var classTypeArguments = typeSymbol.IsGenericType ? $"<{string.Join(", ", typeSymbol.TypeArguments.Select(ta => ta.ToDisplayString()))}>" : "";
            var proxyClassName = $"{className}Proxy{classTypeArguments}";

            var usings = typeDeclarationSyntax.Ancestors(false)
                .SelectMany(a => a is NamespaceDeclarationSyntax nds ? nds.Usings : a is CompilationUnitSyntax cus ? cus.Usings : [])
                .Select(u => u.WithoutTrivia().NormalizeWhitespace().ToFullString())
                .Union(["using ProxySourceGenerator;"])
                .Distinct();
            foreach (var usingNs in usings)
            {
                strBuilder.AppendLine(usingNs);
            }

            strBuilder.AppendLine($$"""
                namespace {{typeSymbol.ContainingNamespace.ToDisplayString()}}
                {
                """);

            var distinctMembers = new HashSet<string>();
            if (useInterface && autoGenerateProxyInterface && interfaceDeclarationSyntax == null)
            {
                strBuilder.AppendLine($$"""
                        partial interface I{{className}}{{classTypeArguments}} {{typeDeclarationSyntax.ConstraintClauses}}
                        {
                    """);

                foreach (var member in RecursiveFindMembers(typeSymbol).Where(m => !m.IsStatic &&
                    (m.Kind == SymbolKind.Property || m.Kind == SymbolKind.Method) &&
                    (m.DeclaredAccessibility != Accessibility.Private)))
                {
                    var propertyDeclarationSyntax = member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as PropertyDeclarationSyntax ??
                        (member is not IPropertySymbol propertySymbol ? null :
                        SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertySymbol.Type.ToDisplayString()), propertySymbol.Name)
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())
                            .WithAccessorList(SyntaxFactory.AccessorList(new SyntaxList<AccessorDeclarationSyntax>(new[] {
                                propertySymbol.GetMethod != null ? SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)) : null,
                                propertySymbol.SetMethod != null ? SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)) : null }
                            .Where(a => a != null)
                            .Select(a => a!)))));

                    var methodDeclarationSyntax = member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax ??
                        (member is not IMethodSymbol methodSymbol || methodSymbol.MethodKind != MethodKind.Ordinary ? null :
                        SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(methodSymbol.ReturnType.ToDisplayString()), methodSymbol.Name)
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())
                            .WithTypeParameterList(methodSymbol.TypeParameters.Any() ? SyntaxFactory.TypeParameterList(SyntaxFactory.SeparatedList(
                                methodSymbol.TypeParameters.Select(tp => SyntaxFactory.TypeParameter(tp.ToDisplayString()))
                                )) : null)
                            .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(
                                methodSymbol.Parameters.Select(p => SyntaxFactory.Parameter(SyntaxFactory.Identifier(p.Name)).WithType(SyntaxFactory.ParseTypeName(p.Type.ToDisplayString()))))
                                )));

                    if (propertyDeclarationSyntax != null)
                    {
                        var newPropertyDeclarationSyntax = propertyDeclarationSyntax
                            .WithModifiers(new SyntaxTokenList())
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())
                            .WithAccessorList(
                                SyntaxFactory.AccessorList(propertyDeclarationSyntax.AccessorList != null ?
                                    new SyntaxList<AccessorDeclarationSyntax>(propertyDeclarationSyntax.AccessorList.Accessors
                                        .Where(a => a.Modifiers.All(m => !m.IsKind(SyntaxKind.PrivateKeyword) && !m.IsKind(SyntaxKind.InternalKeyword)))
                                        .Select(s => s.WithExpressionBody(null).WithBody(null).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                        )) :
                                    new SyntaxList<AccessorDeclarationSyntax>(
                                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                        )
                                ).WithoutTrivia()
                            )
                            .WithExpressionBody(null)
                            .WithInitializer(null)
                            .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)).NormalizeWhitespace();

                        var declerationString = newPropertyDeclarationSyntax.ToFullString();
                        if (distinctMembers.Contains(declerationString))
                            continue;
                        distinctMembers.Add(declerationString);

                        strBuilder.AppendLine($$"""
                                {{declerationString}}
                        """);
                    }
                    else if (methodDeclarationSyntax != null)
                    {
                        methodDeclarationSyntax = methodDeclarationSyntax
                            .WithModifiers(new SyntaxTokenList())
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())
                            .WithBody(null)
                            .WithExpressionBody(null)
                            .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)).NormalizeWhitespace();

                        var declerationString = methodDeclarationSyntax.ToFullString();
                        if (distinctMembers.Contains(declerationString))
                            continue;
                        distinctMembers.Add(declerationString);

                        strBuilder.AppendLine($$"""
                                {{declerationString}};
                        """);
                    }
                }

                strBuilder.AppendLine("""
                        }

                    """);
            }

            var typeAttributes = SyntaxFactory.List(typeSymbol.GetAttributes()
                .Where(ad => ad.AttributeClass?.ToDisplayString() != "ProxySourceGenerator.GenerateProxyAttribute")
                .Select(ad => SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new[]{
                    ad.ApplicationSyntaxReference?.GetSyntax() is AttributeSyntax attrSyntax ? attrSyntax :
                    SyntaxFactory.Attribute(SyntaxFactory.ParseName(ad.AttributeClass!.ToDisplayString()), SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        SyntaxFactory.AttributeArgument(SyntaxFactory.ParseExpression(string.Join(", ", ad.NamedArguments.Select(na => $"{na.Key} = {na.Value.ToCSharpString()}"))))
                    })))
                }))));
            var baseTypeName = $"{(useInterface ? "I" : "")}{className}{classTypeArguments}";
            strBuilder.AppendLine($$"""
                    internal static class {{proxyClassName}}Initializer
                    {
                        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
                        public static void RegisterProxy()
                        {
                            ProxyAccessor<{{baseTypeName}}>.Register(underlyingObject => new {{proxyClassName}}(underlyingObject));
                        }
                    }
                    
                    {{typeAttributes.ToFullString()}}
                    partial class {{proxyClassName}}: {{baseTypeName}}, IGeneratedProxy<{{baseTypeName}}> {{typeDeclarationSyntax.ConstraintClauses}}
                    {
                        /// <inheritdoc/>
                        public InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
                        /// <inheritdoc/>
                        public InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
                        /// <inheritdoc/>
                        public InterceptMethodHandler InterceptMethod { get; set; }
                        /// <inheritdoc/>
                        public InterceptAsyncMethodHandler InterceptAsyncMethod { get; set; }
                        /// <inheritdoc/>
                        public {{baseTypeName}} UnderlyingObject { get; set; }
                        /// <inheritdoc/>
                        {{baseTypeName}} IGeneratedProxy<{{baseTypeName}}>.Access => ({{baseTypeName}}) this;

                        public {{proxyClassName}}({{baseTypeName}} underlyingObject): base()
                        {
                            UnderlyingObject = underlyingObject;
                        }
                """);

            foreach (var ctor in typeSymbol.InstanceConstructors.Where(ctor =>
                !ctor.Parameters.IsEmpty &&
                ctor.DeclaredAccessibility != Accessibility.Private))
            {
                var accessibilityKeyword = ctor.DeclaredAccessibility switch
                {
                    Accessibility.Internal when !ctor.IsImplicitlyDeclared => "internal",
                    Accessibility.Protected when !ctor.IsImplicitlyDeclared => "protected",
                    Accessibility.ProtectedOrInternal when !ctor.IsImplicitlyDeclared => "protected internal",
                    Accessibility.ProtectedAndInternal when !ctor.IsImplicitlyDeclared => "private protected",
                    _ => "public"

                };
                strBuilder.AppendLine($$"""
                            {{accessibilityKeyword}} {{proxyClassName}} ({{string.Join(", ", new[] { $"{baseTypeName} underlyingObject" }.Concat(ctor.Parameters.Select(p => p.ToDisplayString())))}})
                                :base({{string.Join(", ", ctor.Parameters.Select(p => p.Name))}})
                            {
                                UnderlyingObject = underlyingObject;
                            }
                    """);
            }

            distinctMembers = [];
            foreach (var member in RecursiveFindMembers(typeSymbol).Where(m => !m.IsStatic && (useInterface || m.IsVirtual || m.IsAbstract) &&
                (m.Kind == SymbolKind.Property || m.Kind == SymbolKind.Method) &&
                (m.DeclaredAccessibility != Accessibility.Private)))
            {
                var attributeListSyntaxFromSymbol = member.GetAttributes().Select(ad => SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new[]{
                    SyntaxFactory.Attribute(SyntaxFactory.ParseName(ad.AttributeClass!.ToDisplayString()), SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        SyntaxFactory.AttributeArgument(SyntaxFactory.ParseExpression(string.Join(", ", ad.NamedArguments.Select(na => $"{na.Key} = {na.Value.ToCSharpString()}"))))
                    })))
                })));

                var propertySymbol = member as IPropertySymbol;
                var propertyDeclarationSyntax = member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as PropertyDeclarationSyntax ??
                        (propertySymbol == null ? null :
                        SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertySymbol.Type.ToDisplayString()), propertySymbol.Name)
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(attributeListSyntaxFromSymbol))
                            .WithAccessorList(SyntaxFactory.AccessorList(new SyntaxList<AccessorDeclarationSyntax>(new[] {
                                propertySymbol.GetMethod != null ? SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)) : null,
                                propertySymbol.SetMethod != null ? SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)) : null }
                            .Where(a => a != null)
                            .Select(a => a!))))
                            .WithModifiers(SyntaxFactory.TokenList(propertySymbol.DeclaredAccessibility switch
                            {
                                Accessibility.Public => SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                                Accessibility.Protected => SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                                Accessibility.Internal => SyntaxFactory.Token(SyntaxKind.InternalKeyword),
                                _ => SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                            },
                                propertySymbol.IsOverride ? SyntaxFactory.Token(SyntaxKind.OverrideKeyword) : SyntaxFactory.MissingToken(SyntaxKind.OverrideKeyword),
                                propertySymbol.IsAbstract ? SyntaxFactory.Token(SyntaxKind.AbstractKeyword) : SyntaxFactory.MissingToken(SyntaxKind.AbstractKeyword)))
                            );
                var methodSymbol = member as IMethodSymbol;
                var methodDeclarationSyntax = member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax ??
                        (methodSymbol == null || methodSymbol.MethodKind != MethodKind.Ordinary ? null :
                        SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(methodSymbol.ReturnType.ToDisplayString()), methodSymbol.Name)
                        .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(attributeListSyntaxFromSymbol))
                        .WithModifiers(SyntaxFactory.TokenList(methodSymbol.DeclaredAccessibility switch
                        {
                            Accessibility.Public => SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                            Accessibility.Protected => SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                            Accessibility.Internal => SyntaxFactory.Token(SyntaxKind.InternalKeyword),
                            _ => SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        },
                            methodSymbol.IsOverride ? SyntaxFactory.Token(SyntaxKind.OverrideKeyword) : SyntaxFactory.MissingToken(SyntaxKind.OverrideKeyword),
                            methodSymbol.IsAbstract ? SyntaxFactory.Token(SyntaxKind.AbstractKeyword) : SyntaxFactory.MissingToken(SyntaxKind.AbstractKeyword)))
                        .WithTypeParameterList(methodSymbol.TypeParameters.Any() ? SyntaxFactory.TypeParameterList(SyntaxFactory.SeparatedList(
                            methodSymbol.TypeParameters.Select(tp => SyntaxFactory.TypeParameter(tp.ToDisplayString()))
                            )) : null)
                        .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(
                            methodSymbol.Parameters.Select(p => SyntaxFactory.Parameter(SyntaxFactory.Identifier(p.Name)).WithType(SyntaxFactory.ParseTypeName(p.Type.ToDisplayString()))))
                            )));

                if (propertyDeclarationSyntax != null && propertySymbol != null)
                {
                    var modifiers = propertyDeclarationSyntax.Modifiers
                            .Where(m =>
                                !m.IsKind(SyntaxKind.NewKeyword) &&
                                !m.IsKind(SyntaxKind.AbstractKeyword) &&
                                !m.IsKind(SyntaxKind.OverrideKeyword) &&
                                !m.IsKind(SyntaxKind.VirtualKeyword));
                    if (!useInterface)
                        modifiers = modifiers
                            .Union([SyntaxFactory.Token(SyntaxKind.OverrideKeyword).WithTrailingTrivia(SyntaxFactory.Space)]);

                    var newPropertyDeclarationSyntax = propertyDeclarationSyntax
                        .WithModifiers(new SyntaxTokenList().AddRange(modifiers))
                        .WithAccessorList(null)
                        .WithExpressionBody(null)
                        .WithInitializer(null)
                        .WithoutTrivia()
                        .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)).NormalizeWhitespace(eol: " ");

                    var declerationString = newPropertyDeclarationSyntax.ToFullString();
                    if (distinctMembers.Contains(declerationString))
                        continue;
                    distinctMembers.Add(declerationString);

                    strBuilder.AppendLine($$"""
                                #region {{declerationString.Replace("\n", "").Replace("\r", "")}} Property
                        """);

                    var hasGet = false;
                    if (propertyDeclarationSyntax.AccessorList == null ||
                        propertyDeclarationSyntax.AccessorList.Accessors.Any(ads => ads.IsKind(SyntaxKind.GetAccessorDeclaration) &&
                        ads.Modifiers.All(m => !m.IsKind(SyntaxKind.PrivateKeyword) && !m.IsKind(SyntaxKind.InternalKeyword))))
                    {
                        strBuilder.AppendLine($$"""
                                protected virtual {{propertySymbol.Type.ToDisplayString()}} OnGet{{member.Name}}(Func<{{propertySymbol.Type.ToDisplayString()}}> getter)
                                {
                                    return getter();
                                }
                        """);
                        hasGet = true;
                    }

                    var hasSet = false;
                    if (propertyDeclarationSyntax.AccessorList != null &&
                        propertyDeclarationSyntax.AccessorList.Accessors.Any(ads => ads.IsKind(SyntaxKind.SetAccessorDeclaration) &&
                        ads.Modifiers.All(m => !m.IsKind(SyntaxKind.PrivateKeyword) && !m.IsKind(SyntaxKind.InternalKeyword))))
                    {
                        strBuilder.AppendLine($$"""
                                protected virtual void OnSet{{member.Name}}(Action<{{propertySymbol.Type.ToDisplayString()}}> setter, {{propertySymbol.Type.ToDisplayString()}} value)
                                {
                                    setter(value);
                                }
                        """);
                        hasSet = true;
                    }

                    strBuilder.AppendLine($$"""
                                {{declerationString}}
                                {
                        """);
                    if (hasGet)
                        strBuilder.AppendLine($$"""
                                    get 
                                    {
                                        if (InterceptPropertyGetter != null)
                                            return ({{propertySymbol.Type.ToDisplayString()}})InterceptPropertyGetter("{{member.Name}}", () => OnGet{{member.Name}}(() => UnderlyingObject.{{member.Name}}));
                                        else
                                            return OnGet{{member.Name}}(() => UnderlyingObject.{{member.Name}});
                                    }
                        """);
                    if (hasSet)
                        strBuilder.AppendLine($$"""
                                    set
                                    {
                                        if (InterceptPropertySetter != null)
                                            InterceptPropertySetter("{{member.Name}}", value => OnSet{{member.Name}}(v => UnderlyingObject.{{member.Name}} = v, ({{propertySymbol.Type.ToDisplayString()}})value), value);
                                        else
                                            OnSet{{member.Name}}(v => UnderlyingObject.{{member.Name}} = v, value);
                                    }
                        """);
                    strBuilder.AppendLine($$"""
                                }
                        """);
                    strBuilder.AppendLine($$"""
                                #endregion //{{declerationString}} Property
                        """);
                }
                else if (methodDeclarationSyntax != null && methodSymbol != null)
                {
                    var modifiers = methodDeclarationSyntax.Modifiers
                            .Where(m =>
                                !m.IsKind(SyntaxKind.NewKeyword) &&
                                !m.IsKind(SyntaxKind.AbstractKeyword) &&
                                !m.IsKind(SyntaxKind.OverrideKeyword) &&
                                !m.IsKind(SyntaxKind.VirtualKeyword));
                    if (!useInterface)
                        modifiers = modifiers
                            .Union([SyntaxFactory.Token(SyntaxKind.OverrideKeyword).WithTrailingTrivia(SyntaxFactory.Space)]);

                    var hasAsync = methodSymbol.IsAsync;
                    var isAwaitable = IsMethodAwaitable(methodSymbol, compilation);
                    var interceptMethod = isAwaitable ? "InterceptAsyncMethod" : "InterceptMethod";
                    if (!hasAsync && isAwaitable)
                    {
                        modifiers = modifiers
                            .Union([SyntaxFactory.Token(SyntaxKind.AsyncKeyword).WithTrailingTrivia(SyntaxFactory.Space)]);
                    }

                    methodDeclarationSyntax = methodDeclarationSyntax
                        .WithModifiers(new SyntaxTokenList().AddRange(modifiers))
                        .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())
                        .WithBody(null)
                        .WithExpressionBody(null)
                        .WithoutTrivia()
                        .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)).NormalizeWhitespace();

                    var declerationString = methodDeclarationSyntax.ToFullString();
                    if (distinctMembers.Contains(declerationString))
                        continue;
                    distinctMembers.Add(declerationString);

                    strBuilder.AppendLine($$"""
                                #region {{declerationString}} Method
                        """);

                    var methodNameSyntax = methodSymbol.IsGenericMethod ? $"{methodSymbol.Name}<{string.Join(", ", methodSymbol.TypeParameters.Select(tp => tp.ToDisplayString()))}>" : methodSymbol.Name;
                    var methodDelegate = methodSymbol.Parameters.Length switch
                    {
                        0 when methodSymbol.ReturnsVoid => "Action",
                        0 when !methodSymbol.ReturnsVoid => $"Func<{methodSymbol.ReturnType.ToDisplayString()}>",
                        _ when methodSymbol.ReturnsVoid => $"Action<{string.Join(", ", methodSymbol.Parameters.Select(p => p.Type.ToDisplayString()))}>",
                        _ => $"Func<{string.Join(", ", methodSymbol.Parameters.Select(p => p.Type.ToDisplayString()).Concat([methodSymbol.ReturnType.ToDisplayString()]))}>"
                    };

                    var taskType = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task");
                    var valueTaskType = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask");

                    var returnsVoid = methodSymbol.ReturnsVoid ||
                        SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType, taskType) ||
                        SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType, valueTaskType);
                    var resultType = isAwaitable ? GetAsyncResultType(methodSymbol.ReturnType, compilation)! : methodSymbol.ReturnType;

                    // if the method is void, Task, or ValueTask, we need to return null after calling the underlying method
                    var callUnderlyingObjectMethod = returnsVoid ?
                        $"{{{(isAwaitable ? "await " : string.Empty)}On{methodNameSyntax}(UnderlyingObject.{methodNameSyntax}{string.Join(", ", new[] { string.Empty }.Concat(methodSymbol.Parameters.Select(p => $"({p.Type.ToDisplayString()})p[\"{p.Name}\"]")))}); return null;}}"
                        :
                        $"{(isAwaitable ? "await " : string.Empty)}On{methodNameSyntax}(UnderlyingObject.{methodNameSyntax}{string.Join(", ", new[] { string.Empty }.Concat(methodSymbol.Parameters.Select(p => $"({p.Type.ToDisplayString()})p[\"{p.Name}\"]")))})";

                    strBuilder.AppendLine($$"""
                                protected virtual {{methodSymbol.ReturnType.ToDisplayString()}} On{{methodNameSyntax}}({{methodDelegate}} baseMethod{{string.Join(", ", new[] { string.Empty }.Concat(methodSymbol.Parameters.Select(p => $"{p.Type} {p.Name}")))}})
                                    {{methodDeclarationSyntax.ConstraintClauses}}
                                {
                                    {{(methodSymbol.ReturnsVoid ? string.Empty : "return ")}}baseMethod({{string.Join(", ", methodSymbol.Parameters.Select(p => p.Name))}});
                                }
                                {{declerationString}}
                                {
                                    if ({{interceptMethod}} != null)
                                        {{(returnsVoid ? string.Empty : $"return ({resultType.ToDisplayString()})")}}{{(isAwaitable ? "await " : string.Empty)}}{{interceptMethod}}(
                                            "{{methodNameSyntax}}", 
                                            {{(isAwaitable ? "async " : string.Empty)}}p => {{callUnderlyingObjectMethod}},
                                            new Dictionary<string, object> {
                                                {{string.Join(",\n", methodSymbol.Parameters.Select(p => $"[\"{p.Name}\"] = {p.Name}"))}}
                                            }
                                            );
                                    else
                                        {{(returnsVoid ? string.Empty : "return ")}}{{(isAwaitable ? "await " : string.Empty)}}On{{methodNameSyntax}}(UnderlyingObject.{{methodNameSyntax}}{{string.Join(", ", new[] { string.Empty }.Concat(methodSymbol.Parameters.Select(p => p.Name)))}});
                                }
                        """);
                    strBuilder.AppendLine($$"""
                                #endregion //{{declerationString}} Method
                        """);
                }
            }

            strBuilder.AppendLine("""
                    }
                }
                """);

            context.AddSource($"{proxyClassName.Replace("<", "{").Replace(">", "}")}.g.cs", SourceText.From(strBuilder.ToString(), Encoding.UTF8));


            static IEnumerable<ISymbol> RecursiveFindMembers(INamedTypeSymbol typeSymbol)
            {
                foreach (var member in typeSymbol.GetMembers())
                {
                    yield return member;
                }

                if (typeSymbol?.BaseType != null && typeSymbol.BaseType.Name != "Object" && typeSymbol.BaseType.ContainingNamespace.Name != "System")
                {
                    foreach (var member in RecursiveFindMembers(typeSymbol.BaseType))
                    {
                        yield return member;
                    }
                }
                if (typeSymbol != null && typeSymbol.BaseType == null && typeSymbol.AllInterfaces.Length > 0)
                {
                    foreach (var interfaceType in typeSymbol.AllInterfaces)
                    {
                        foreach (var member in RecursiveFindMembers(interfaceType))
                        {
                            yield return member;
                        }
                    }
                }
            }

            static bool IsMethodAwaitable(IMethodSymbol method, Compilation compilation)
            {
                if (method == null)
                    return false;

                if (method.IsAsync)
                    return true;

                var returnType = method.ReturnType;
                return IsTypeAwaitable(returnType, compilation);
            }

            static bool IsTypeAwaitable(ITypeSymbol type, Compilation compilation)
            {
                if (type == null || type.SpecialType == SpecialType.System_Void)
                    return false;

                var taskType = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task");
                var taskOfTType = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");
                var valueTaskType = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask");
                var valueTaskOfTType = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1");

                if (taskType != null && SymbolEqualityComparer.Default.Equals(type, taskType))
                    return true;

                if (taskOfTType != null && SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, taskOfTType))
                    return true;

                if (valueTaskType != null && SymbolEqualityComparer.Default.Equals(type, valueTaskType))
                    return true;

                if (valueTaskOfTType != null && SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, valueTaskOfTType))
                    return true;

                var getAwaiter = type
                    .GetMembers("GetAwaiter")
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.Parameters.Length == 0);

                if (getAwaiter == null)
                    return false;

                var awaiterType = getAwaiter.ReturnType;
                if (awaiterType == null)
                    return false;

                var hasIsCompleted = awaiterType
                    .GetMembers("IsCompleted")
                    .OfType<IPropertySymbol>()
                    .Any(p => p.Type.SpecialType == SpecialType.System_Boolean && !p.IsWriteOnly);

                if (!hasIsCompleted)
                    return false;

                var hasGetResult = awaiterType
                    .GetMembers("GetResult")
                    .OfType<IMethodSymbol>()
                    .Any(m => m.Parameters.Length == 0);

                if (!hasGetResult)
                    return false;

                var notifyCompletion = compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.INotifyCompletion");
                if (notifyCompletion != null &&
                    !awaiterType.AllInterfaces.Contains(notifyCompletion, SymbolEqualityComparer.Default))
                {
                    return false;
                }

                return true;
            }

            ITypeSymbol? GetAsyncResultType(ITypeSymbol typeSymbol, Compilation compilation)
            {
                // Task<TResult> tanımı
                var taskOfT = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");
                // ValueTask<TResult> tanımı
                var valueTaskOfT = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1");

                if (typeSymbol is not INamedTypeSymbol named)
                    return null;

                if (taskOfT is not null &&
                    SymbolEqualityComparer.Default.Equals(named.OriginalDefinition, taskOfT))
                {
                    // Task<TResult>
                    return named.TypeArguments[0];
                }

                if (valueTaskOfT is not null &&
                    SymbolEqualityComparer.Default.Equals(named.OriginalDefinition, valueTaskOfT))
                {
                    // ValueTask<TResult>
                    return named.TypeArguments[0];
                }

                return null;
            }
        }
    }

}
