namespace TSITSolutions.StringLocalizerSourceGenerator;

using Scriban;

public static class TemplateFileRenderer
{
    public static string RenderContent<T>(string fileName, T? model = null)
        where T : class
    {
        var template = Template.Parse(EmbeddedResource.GetContent(fileName));

        return template.Render(model, member => member.Name);
    }

    public static string RenderContent(string fileName)
    {
        var template = Template.Parse(EmbeddedResource.GetContent(fileName));

        return template.Render();
    }
}
