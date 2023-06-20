using Announcement.App.Entities;
using FluentResults;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;

namespace Announcement.Infrastructure.Database;

public class RedisAnnouncementRepo : IAnnouncementRepo
{
    private readonly IConnectionMultiplexer _redis;
    public const string KeyPrefix = "announcement";

    public RedisAnnouncementRepo(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer;
    }
    
    public async Task<AnnouncementModel?> GetById(Guid id)
    {
        var db = _redis.GetDatabase();
        var key = KeyFor(id);
        var announcementHash = await db.HashGetAllAsync(key);
        if (announcementHash.Length == 0)
            return null;
        return HashToAnnouncement(announcementHash);
    }

    public async Task<bool> Delete(Guid id)
    {
        var db = _redis.GetDatabase();
        var key = KeyFor(id);
        return await db.KeyDeleteAsync(key);
    }

    public async Task Update(AnnouncementModel announcement)
    {
        await Add(announcement);
    }

    public async Task Add(AnnouncementModel announcement)
    {
        if (announcement is null) 
            throw new NullReferenceException();

        var db = _redis.GetDatabase();
        var entries = AnnouncementToHash(announcement);
        await db.HashSetAsync(KeyFor(announcement.Id), entries);
    }

    public async Task<IList<AnnouncementModel>> GetSimilar(string queryText, int length)
    {
        var db = _redis.GetDatabase();
        var queryCommand = string.Join(
            " | ", 
            queryText.Split()
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .ToArray());
        var query = new Query(queryCommand).Limit(0, length);
        
        var searchResult = await db.FT().SearchAsync(RedisAnnouncementSearchSetup.SearchIndexName, query);

        return searchResult.Documents
            .Select(DocumentToAnnouncement)
            .ToList();
    }

    private string KeyFor(Guid id) => $"{KeyPrefix}:{id}";

    private HashEntry[] AnnouncementToHash(AnnouncementModel announcement)
    {
        return new[]
        {
            new HashEntry("id", announcement.Id.ToString()),
            new HashEntry("title", announcement.Title),
            new HashEntry("description", announcement.Description),
            new HashEntry("dateAdded", announcement.DateAdded.ToString())
        };
    }

    public AnnouncementModel HashToAnnouncement(HashEntry[] hashEntries)
    {
        var id = Guid.Parse(hashEntries.First(e => e.Name == "id").Value!);
        string title = hashEntries.First(e => e.Name == "title").Value!;
        string description = hashEntries.First(e => e.Name == "description").Value!;
        var dateAdded = DateTime.Parse(hashEntries.First(e => e.Name == "dateAdded").Value!);
        return new()
        {
            Id = id,
            Title = title,
            Description = description,
            DateAdded = dateAdded
        };
    }

    private AnnouncementModel DocumentToAnnouncement(Document document)
    {
        return new ()
        {
            Id = Guid.Parse(document["id"]),
            Title = document["title"],
            Description = document["description"],
            DateAdded = DateTime.Parse(document["dateAdded"])
        };
    }
}