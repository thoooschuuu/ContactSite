﻿namespace TSITSolutions.ContactSite.Server.Projects.Contracts;

public class GetProjectRequest
{
    public Guid Id { get; set; }
    public string? Language { get; set; }
}