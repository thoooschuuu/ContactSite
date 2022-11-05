using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;

namespace TSITSolutions.StringLocalizerSourceGenerator.Tests.Integration.Helper;

internal static class GeneratorDriverRunResultExtensions
{
    internal static IEnumerable<string> GetGeneratedSources(this GeneratorDriverRunResult result) =>
        result.Results
            .SelectMany(r => r.GeneratedSources)
            .Select(s => s.SourceText.ToString());

    public static GeneratorDriverRunResultAssertions Should(this GeneratorDriverRunResult instance) => new(instance);

    internal sealed class GeneratorDriverRunResultAssertions : ReferenceTypeAssertions<GeneratorDriverRunResult, GeneratorDriverRunResultAssertions>
    {
        public GeneratorDriverRunResultAssertions(GeneratorDriverRunResult subject)
            : base(subject)
        {
        }

        protected override string Identifier => nameof(GeneratorDriverRunResult);

        public AndConstraint<GeneratorDriverRunResultAssertions> HaveProducedSourceCode(params string[] expectedCode)
        {
            var generatedSources = Subject.GetGeneratedSources().ToArray();
            foreach (var code in expectedCode)
            {
                var normalizedLineEndings = NormalizeLineEndings(code);
                var normalizedSources = generatedSources.Select(NormalizeLineEndings).ToArray();
                normalizedSources.Single().Should().Be(normalizedLineEndings);
            }

            return new AndConstraint<GeneratorDriverRunResultAssertions>(this);
        }

        public AndConstraint<GeneratorDriverRunResultAssertions> NotHaveDiagnosticErrors()
        {
            Subject.Diagnostics.Should().NotContain(d => d.Severity == DiagnosticSeverity.Error);

            return new AndConstraint<GeneratorDriverRunResultAssertions>(this);
        }

        public AndConstraint<GeneratorDriverRunResultAssertions> NotHaveDiagnostics()
        {
            Subject.Diagnostics.Should().BeEmpty();

            return new AndConstraint<GeneratorDriverRunResultAssertions>(this);
        }

        private static string? NormalizeLineEndings(string? text) => 
            text?
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");
    }
}
