
using Microsoft.Extensions.Logging;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;
using StackExchange.Redis;

namespace Announcement.Infrastructure.Database;

public class RedisAnnouncementSearchSetup
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisAnnouncementSearchSetup> _log;
    public const string SearchIndexName = "announcement_index";

    public RedisAnnouncementSearchSetup(IConnectionMultiplexer multiplexer, ILogger<RedisAnnouncementSearchSetup> logger)
    {
        _db = multiplexer.GetDatabase();
        _log = logger;
    }
    
    private void DropIndex()
    {
        try
        {
            bool result = _db.FT().DropIndex(SearchIndexName);
            _log.LogInformation("Index dropped with the result: {result}", result);
        }
        catch
        {
            _log.LogInformation("Index was not dropped because it didn't exist");
        }
    }
    
    /// <summary>
    /// Ensures that a search index is created.
    /// </summary>
    public void Setup()
    {
        DropIndex();

        var createParams = new FTCreateParams()
            .On(IndexDataType.HASH)
            .Prefix($"{RedisAnnouncementRepo.KeyPrefix}:");
        var schema = new Schema()
            .AddTextField("title")
            .AddTextField("description");
        bool result = _db.FT().Create(SearchIndexName, createParams, schema);

        _log.LogInformation("Index created with the result: {result}", result);
    }
}