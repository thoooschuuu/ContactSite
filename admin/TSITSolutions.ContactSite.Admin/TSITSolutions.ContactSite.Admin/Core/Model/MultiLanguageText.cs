namespace TSITSolutions.ContactSite.Admin.Core.Model;

public class MultiLanguageText : Dictionary<string, string>
{
    private MultiLanguageText()
    {
    }
    
    private MultiLanguageText(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }
    
    public static MultiLanguageText Empty => new();

    public static MultiLanguageText Create(params (string language, string text)[] texts) =>
        new(texts.ToDictionary(x => x.language, x => x.text));
    
    public string GetDefaultText() => this["de-DE"];
}