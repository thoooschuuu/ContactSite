using System.Net.Http.Json;
using FluentAssertions;
using TSITSolutions.ContactSite.Server.MongoDb.Model;
using TSITSolutions.ContactSite.Shared.Projects;
using Xunit.Abstractions;

namespace TSITSolutions.ContactSite.Server.Tests.Integration.Endpoints;

public class GetProjectsEndpointTests : IClassFixture<ContactSiteApplicationFactory>, IAsyncLifetime
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
        await _factory.AddProject(new StoreProject(Guid.NewGuid(), "p1", "d1", "r1", "cd1", new DateTime(2023,1,1), null,
            ["t1", "t2"]));
        await _factory.AddProject(new StoreProject(Guid.NewGuid(), "p2", "d2", "r2", "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31),
            ["t3", "t4"]));
        
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/projects");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var projectsResponse = await response.Content.ReadFromJsonAsync<ProjectsResponse>();
        projectsResponse.Should().NotBeNull();
        projectsResponse!.Projects.Should().HaveCount(2);
    }

    [Fact]
    public async Task Endpoint_ReturnsAllProjectWithMarkdownInRoleAndDescription_WhenAskedTo()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        await _factory.AddProject(new StoreProject(id1, "p1", "d1", "r1", "cd1", new DateTime(2023,1,1), null, ["t1", "t2"
        ]));
        await _factory.AddProject(new StoreProject(id2, "p2", "d2", "r2", "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31),
            ["t3", "t4"]));
        await _factory.AddCultureSpecificProject(new CultureSpecificStoreProject(Guid.NewGuid(), id1, "en1", "d1-en", "r1-en", "cd1-en", "en-US"));
        await _factory.AddCultureSpecificProject(new CultureSpecificStoreProject(Guid.NewGuid(), id2, "en2", "d2-en", "r2-en", "cd2-en", "en-US"));

        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/projects?culture=en-US");

        response.IsSuccessStatusCode.Should().BeTrue();

        var projectsResponse = await response.Content.ReadFromJsonAsync<ProjectsResponse>();
        projectsResponse.Should().NotBeNull();
        
        var first = projectsResponse!.Projects.First();
        first.Title.Should().Be("en1");
        first.Description.Should().Be("<p>d1-en</p>");
        first.Role.Should().Be("<p>r1-en</p>");
        first.CustomerDomain.Should().Be("cd1-en");
        
        var second = projectsResponse.Projects.Last();
        second.Title.Should().Be("en2");
        second.Description.Should().Be("<p>d2-en</p>");
        second.Role.Should().Be("<p>r2-en</p>");
        second.CustomerDomain.Should().Be("cd2-en");
    }
    
    [Fact]
    public async Task Endpoint_ReturnsAllProjectsWithCultureSpecificOverrides_WhenAskedTo()
    {
        var id2 = Guid.NewGuid();
        await _factory.AddProject(new StoreProject(id2, "p2", "d2", "r2", "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31),
            ["t3", "t4"]));
        await _factory.AddCultureSpecificProject(new CultureSpecificStoreProject(Guid.NewGuid(), id2, "en", "p2-en", "d2-en", "r2-en", "en-US"));
        
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/projects/{id2}?culture=en-US");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var projectsResponse = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        projectsResponse.Should().NotBeNull();
        projectsResponse!.Id.Should().Be(id2);
        projectsResponse.Title.Should().Be("en");
        projectsResponse.Description.Should().Be("<p>p2-en</p>");
        projectsResponse.Role.Should().Be("<p>d2-en</p>");
        projectsResponse.CustomerDomain.Should().Be("r2-en");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _factory.ClearCollections();
}