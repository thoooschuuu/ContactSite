using System.Net.Http.Json;
using FluentAssertions;
using TSITSolutions.ContactSite.Server.MongoDb.Model;
using TSITSolutions.ContactSite.Shared.Projects;
using Xunit.Abstractions;

namespace TSITSolutions.ContactSite.Server.Tests.Integration.Endpoints;

public class GetProjectEndpointTests : IClassFixture<ContactSiteApplicationFactory>, IAsyncLifetime
{
    private readonly ContactSiteApplicationFactory _factory;

    public GetProjectEndpointTests(ContactSiteApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _factory.Output = output;
    }
    
    [Fact]
    public async Task Endpoint_ReturnsSingleProject_WhenAskedTo()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        await _factory.AddProject(new StoreProject(id1, "p1", "d1", "r1", "cd1", new DateTime(2023,1,1), null, new []{ "t1", "t2" }));
        await _factory.AddProject(new StoreProject(id2, "p2", "d2", "r2", "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31), new []{ "t3", "t4" }));
        
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/projects/{id2}");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var projectsResponse = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        projectsResponse.Should().NotBeNull();
        projectsResponse!.Id.Should().Be(id2);
        projectsResponse.Title.Should().Be("p2");
    }

    [Fact] 
    public async Task Endpoint_ReturnsSingleProjectWithMarkdownInRoleAndDescription_WhenAskedTo()
    {
        var id = Guid.NewGuid();
        const string descriptionWithMarkdown = "This is a [desc](https://www.google.com)";
        const string roleWithMarkdown = "This is a [role](https://www.google.com)";
        await _factory.AddProject(new StoreProject(id, "p2", descriptionWithMarkdown, roleWithMarkdown, "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31), new []{ "t3", "t4" }));
        
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/projects/{id}");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var projectsResponse = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        projectsResponse.Should().NotBeNull();
        projectsResponse!.Description.Should().Be("<p>This is a <a href=\"https://www.google.com\">desc</a></p>");
        projectsResponse.Role.Should().Be("<p>This is a <a href=\"https://www.google.com\">role</a></p>");
    }
    
    [Fact]
    public async Task Endpoint_ReturnsSingleProjectWithLanguageSpecificOverrides_WhenAskedTo()
    {
        var id2 = Guid.NewGuid();
        await _factory.AddProject(new StoreProject(id2, "p2", "d2", "r2", "cd2", new DateTime(2022,1,1), new DateTime(2022,12,31), new []{ "t3", "t4" }));
        await _factory.AddLanguageSpecificProject(new LanguageSpecificStoreProject(Guid.NewGuid(), id2, "en", "p2-en", "d2-en", "r2-en", "en-US"));
        
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/projects/{id2}?language=en-US");
        
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