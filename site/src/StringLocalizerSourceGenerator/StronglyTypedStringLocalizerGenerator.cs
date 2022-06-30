using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using TSITSolutions.StringLocalizerSourceGenerator;

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

    internal static readonly DiagnosticDescriptor FoundResourcesDescriptor =
        new("STSLG002",
            "Found Resource types",
            "Found {0} Resource types",
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
//#if DEBUG
//        if (!Debugger.IsAttached)
//        {
//            Debugger.Launch();
//        }
//#endif 
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
                var source = TemplateFileRenderer.RenderContent("StronglyTypedStringLocalizer.sbntxt",
                    new StronglyTypedStringLocalizerRendererModel(
                        type.ContainingNamespace.ToDisplayString(),
                        type.Name,
                        GetResourceEntries(type).ToArray()
                    ));

                context.AddSource($"StronglyTypedStringLocalizerFor{type.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
    }

    private static IEnumerable<string> GetResourceEntries(ITypeSymbol type)
        => type.GetMembers().OfType<IPropertySymbol>()
            .Where(p => 
                p.IsDefinition &&
                p.DeclaredAccessibility == Accessibility.Public &&
                p.Kind == SymbolKind.Property &&
                p.Type.ToDisplayString().Equals("string", StringComparison.OrdinalIgnoreCase))
            .Select(p => p.Name);
}

internal class StronglyTypedStringLocalizerRendererModel
{
    public StronglyTypedStringLocalizerRendererModel(string ns, string resourceTypeName, IReadOnlyList<string> resources)
    {
        Resources = resources;
        ResourceTypeName = resourceTypeName;
        Namespace = ns;
    }

    public string Namespace { get; }
    public string ResourceTypeName { get; }
    public IReadOnlyList<string> Resources { get; }
}