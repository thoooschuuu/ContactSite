namespace TSITSolutions.ContactSite.Server.Caching;

public class CachingSettings
{
    public bool Enabled { get; set; } = true;
    public int Duration { get; set; } = 3600;
}