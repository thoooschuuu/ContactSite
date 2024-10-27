using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TSITSolutions.ContactSite.Server.Core;

namespace TSITSolutions.ContactSite.Server.MongoDb.Model;

public record StoreProject(
    [property:BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)] Guid Id, 
    string Title, 
    string Description, 
    string Role,
    string CustomerDomain,
    DateTime StartDate,
    DateTime? EndDate,
    IReadOnlyCollection<string> Technologies
)
{
    public Project ToProject(CultureSpecificStoreProject? projectOverride = null) =>
        new(
            Id,
            projectOverride?.Title ?? Title,
            projectOverride?.Description ?? Description,
            projectOverride?.Role ?? Role,
            projectOverride?.CustomerDomain ?? CustomerDomain,
            DateOnly.FromDateTime(StartDate), 
            EndDate.HasValue ? DateOnly.FromDateTime(EndDate.Value) : null,
            Technologies
        );
}