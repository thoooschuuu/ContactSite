using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TSITSolutions.ContactSite.Server.MongoDb.Model;

public record CultureSpecificStoreProject(
    [property:BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)] Guid Id,
    [property:BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)] Guid ProjectId,
    string Title,
    string Description,
    string Role,
    string CustomerDomain,
    string Culture
);