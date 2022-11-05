using System.Text;

namespace TSITSolutions.StringLocalizerSourceGenerator.Tests.Integration.Helper;

internal sealed class SourceCodeBuilder
{
    private readonly HashSet<string> _namespaceImports = new();
    private string? _code;
    private string? _namespace;

    public string Build()
    {
        StringBuilder builder = new();

        if (_namespaceImports.Any())
        {
            foreach (var namespaceImport in _namespaceImports)
            {
                builder.AppendLine($"using {namespaceImport};");
            }

            builder.AppendLine();
        }

        if (_namespace != null)
        {
            builder.AppendLine($"namespace {_namespace};");
            builder.AppendLine();
        }

        if (_code != null)
        {
            builder.Append(_code);
        }

        return builder.ToString();
    }

    public SourceCodeBuilder InNamespace(string @namespace)
    {
        _namespace = @namespace;
        return this;
    }

    public SourceCodeBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public SourceCodeBuilder WithNamespaceImportFor(Type type)
    {
        _namespaceImports.Add(type.Namespace!);
        return this;
    }
}
