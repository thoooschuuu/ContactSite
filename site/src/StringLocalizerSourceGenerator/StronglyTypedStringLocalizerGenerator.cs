using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TSITSolutions.StringLocalizerSourceGenerator;

[Generator]
public class StronglyTypedStringLocalizerGenerator : ISourceGenerator
{
    internal static readonly DiagnosticDescriptor NothingFoundDescriptor =
        new("STSLG001",
            "Nothing found for Resource generation",
            "No Resource file found",
            "STSLGenerator",
            DiagnosticSeverity.Info,
            true);

    internal static readonly DiagnosticDescriptor GenerationErrorDescriptor =
        new("STSLG003",
            "StronglyTypedStringLocalizer generator falied",
            "An exception was thrown by the Strongly Typed String Localizer generator: '{0}'",
            "STSLGenerator",
            DiagnosticSeverity.Error,
            true);

    public void Initialize(GeneratorInitializationContext context)
    {
// #if DEBUG
//         if (!Debugger.IsAttached)
//         {
//             Debugger.Launch();
//         }
// #endif 
        context.RegisterForSyntaxNotifications(() => new ResourceSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            Generate(context);
        }
        catch (Exception ex)
        {
            context.ReportDiagnostic(Diagnostic.Create(GenerationErrorDescriptor, Location.None, ex + ex.StackTrace));
        }
    }

    private static void Generate(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not ResourceSyntaxReceiver receiver || receiver.ClassesToAugment is { Count: 0 })
        {
            context.ReportDiagnostic(Diagnostic.Create(NothingFoundDescriptor, Location.None));
            return;
        }

        foreach (var cds in receiver.ClassesToAugment)
        {
            if (context.Compilation.GetSemanticModel(cds.SyntaxTree).GetDeclaredSymbol(cds) is ITypeSymbol type)
            {
                var properties = cds.Members.OfType<PropertyDeclarationSyntax>();

                var source = TemplateFileRenderer.RenderContent("StronglyTypedStringLocalizer.sbntxt",
                    new StronglyTypedStringLocalizerRendererModel(
                        type.ContainingNamespace.ToDisplayString(),
                        type.Name,
                        GetResourceEntries(properties, context.Compilation).ToArray()
                    ));

                context.AddSource($"StronglyTypedStringLocalizerFor{type.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
    }

    private static IEnumerable<Resource> GetResourceEntries(IEnumerable<PropertyDeclarationSyntax> properties, Compilation compilation)
        => properties
            .Select(p => new Property(p, compilation))
            .Where(p => 
                p.Symbol is
                {
                    IsDefinition: true, 
                    DeclaredAccessibility: Accessibility.Public, 
                    Kind: SymbolKind.Property
                } &&
                p.Symbol.Type.ToDisplayString().Equals("string", StringComparison.OrdinalIgnoreCase))
            .Select(GenerateResource);

    private static readonly Regex DocRegex = new(@"(?'doc'(?<=\/\/\/   ).+(?!<\/summary>))", RegexOptions.Compiled | RegexOptions.Multiline);
    private static Resource GenerateResource(Property property)
    {
        var trivia = property.Syntax
            .GetLeadingTrivia()
            .SingleOrDefault(t =>
                t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));

        var match = DocRegex.Match(trivia.ToFullString());
        var documentation = match.Groups["doc"].Value.Trim();
        return new Resource(property.Symbol!.Name, documentation);
    }
}

internal class Property
{
    public Property(PropertyDeclarationSyntax syntax, Compilation compilation)
    {
        Syntax = syntax;
        Symbol = ModelExtensions.GetDeclaredSymbol(compilation.GetSemanticModel(syntax.SyntaxTree), syntax) as IPropertySymbol;
    }
    public PropertyDeclarationSyntax Syntax { get; }
    public IPropertySymbol? Symbol { get; }
}

internal class StronglyTypedStringLocalizerRendererModel
{
    public StronglyTypedStringLocalizerRendererModel(string ns, string resourceTypeName, IReadOnlyList<Resource> resources)
    {
        Resources = resources;
        ResourceTypeName = resourceTypeName;
        Namespace = ns;
    }

    public string Namespace { get; }
    public string ResourceTypeName { get; }
    public IReadOnlyList<Resource> Resources { get; }
}

internal class Resource
{
    public Resource(string name, string documentation)
    {
        Name = name;
        Documentation = documentation;
    }

    public string Name { get; }
    public string Documentation { get; }
}