using MudBlazor;

namespace TSITSolutions.ContactSite.Client.Infrastructure;

public class TechnologyService
{
    private readonly Dictionary<string, Color> _colorMap = new();
    private readonly Color[] _colors = { Color.Primary, Color.Secondary, Color.Tertiary, Color.Info, Color.Success, Color.Warning, Color.Error, Color.Dark, Color.Surface };
    private int _colorIndex;
    
    public Color GetColor(string technology)
    {
        if (_colorMap.TryGetValue(technology, out var value))
        {
            return value;
        }
        
        _colorMap.Add(technology, _colors[_colorIndex]);
        _colorIndex = (_colorIndex + 1) % _colors.Length;
        return _colorMap[technology];
    }
}