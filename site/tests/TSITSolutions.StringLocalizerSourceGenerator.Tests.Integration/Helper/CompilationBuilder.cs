using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace TSITSolutions.StringLocalizerSourceGenerator.Tests.Integration.Helper;

internal sealed class CompilationBuilder
{
    private static readonly CSharpCompilationOptions DefaultOptions = new(OutputKind.DynamicallyLinkedLibrary);

    private readonly HashSet<MetadataReference> _references = new();
    private readonly HashSet<SyntaxTree> _syntaxTrees = new();

    public Compilation Build() => CSharpCompilation.Create("compilation", _syntaxTrees, _references, DefaultOptions);

    public CompilationBuilder WithDefaultReferences()
    {
        WithSystemReferences();

        return this;
    }

    public CompilationBuilder WithNetStandard20References()
    {
        foreach (var netStandard20Assembly in ReferenceAssemblies.NetStandard.NetStandard20.Assemblies)
        {
            _references.Add(MetadataReference.CreateFromFile(Assembly.Load(netStandard20Assembly).Location));
        }

        return this;
    }

    public CompilationBuilder WithSourceCode(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        _syntaxTrees.Add(syntaxTree);

        return this;
    }

    public CompilationBuilder WithSystemReferences()
    {
        var objectLocation = typeof(object).Assembly.Location;

        _references.Add(CreateFromFile("System.Runtime.dll", typeof(object).Assembly.Location));
        _references.Add(MetadataReference.CreateFromFile(objectLocation));

        return this;
    }

    private MetadataReference CreateFromFile(string fileName, string? assemblyLocation = null)
    {
        var currentLocation = assemblyLocation ?? GetType().Assembly.Location;

        return MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(currentLocation)!, fileName));
    }
}
