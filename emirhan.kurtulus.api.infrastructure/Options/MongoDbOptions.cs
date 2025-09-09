namespace emirhan.kurtulus.api.infrastructure.Options;

public class MongoDbOptions
{
    public const string DefaulSectionName = "Mongo";

    public required string ConnectionString { get; set; }

    public required string DatabaseName { get; set; }
}