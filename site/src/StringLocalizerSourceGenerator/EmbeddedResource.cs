using System.Reflection;

namespace TSITSolutions.StringLocalizerSourceGenerator;

internal static class EmbeddedResource
{
    public static string GetContent(string relativePath)
    {
        var baseName = Assembly.GetExecutingAssembly().GetName().Name;
        var resourceName = relativePath
            .TrimStart('.')
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');

        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(baseName + "." + resourceName);

        if (stream == null)
        {
            throw new FileNotFoundException($"Template with name '{relativePath}' not found.");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
