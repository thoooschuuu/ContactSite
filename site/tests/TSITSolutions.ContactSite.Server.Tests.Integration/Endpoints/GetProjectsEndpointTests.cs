using System.Net.Http.Json;
using FluentAssertions;
using TSITSolutions.ContactSite.Server.MongoDb.Model;
using TSITSolutions.ContactSite.Shared.Projects;
using Xunit.Abstractions;

namespace TSITSolutions.ContactSite.Server.Tests.Integration.Endpoints;

public class GetProjectsEndpointTests : IClassFixture<ContactSiteApplicationFactory>
{
    private readonly ContactSiteApplicationFactory _factory;

    public GetProjectsEndpointTests(ContactSiteApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _factory.Output = output;
    }
    
    [Fact]
    public async Task Endpoint_ReturnsAllProjects_WhenAskedTo()
    {
        await _factory.AddProject(new StoreProject(Guid.NewGuid(), "p1", "d1", "r1", "cd1", new DateTime(2023,1,1), null, new []{ "t1", "t2" }));
        await _factory.AddProject(new StoreProject(Guid.NewGuid(), "p2", "d2", "r2", "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31), new []{ "t3", "t4" }));
        
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/projects");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var projectsResponse = await response.Content.ReadFromJsonAsync<ProjectsResponse>();
        projectsResponse.Should().NotBeNull();
        projectsResponse!.Projects.Should().HaveCount(2);
    }
}