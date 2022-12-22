using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSITSolutions.StringLocalizerSourceGenerator;

internal class ResourceSyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> ClassesToAugment { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax cds)
        {
            return;
        }

        var attributes = cds.AttributeLists.Where(al =>
                al.Attributes.Any(a =>
                    a.Name.ToFullString().Equals("global::System.CodeDom.Compiler.GeneratedCodeAttribute")))
            .SelectMany(a => a.Attributes)
            .ToArray();

        if (attributes.Any(a => a.ArgumentList?.Arguments.Any(arg => arg.Expression.ToFullString().Equals("\"System.Resources.Tools.StronglyTypedResourceBuilder\"")) == true))
        {
            ClassesToAugment.Add(cds);
        }
    }
}