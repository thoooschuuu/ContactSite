using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TSITSolutions.ContactSite.Server.MongoDb.Model;

public record CultureSpecificStoreProject(
    [property:BsonGuidRepresentation(GuidRepresentation.Standard)] Guid Id,
    [property:BsonGuidRepresentation(GuidRepresentation.Standard)] Guid ProjectId,
    string Title,
    string Description,
    string Role,
    string CustomerDomain,
    string Culture
);