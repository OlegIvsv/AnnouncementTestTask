﻿using Announcement.App.Entities;
using FluentResults;
using StackExchange.Redis;

namespace Announcement.Infrastructure.Database;

public class RedisAnnouncementRepo : IAnnouncementRepo
{
    private readonly IConnectionMultiplexer _redis;

    public RedisAnnouncementRepo(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer;
    }
    
    public async Task<Result<AnnouncementModel>> GetById(Guid id)
    {
        var db = _redis.GetDatabase();
        var key = KeyFor(id);
        var bookHash = await db.HashGetAllAsync(key);
        return HashToAnnouncement(bookHash);
    }

    public async Task<bool> Delete(Guid id)
    {
        var db = _redis.GetDatabase();
        var key = KeyFor(id);
        return await db.KeyDeleteAsync(key);
    }

    public async Task Update(AnnouncementModel announcement)
    {
        // The action is the same in case of redis. In case of other DBs
        // it's probably going to be different.
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

    public Task<Result<IList<AnnouncementModel>>> GetSimilar(Guid id, int n)
    {
        throw new NotImplementedException();
    }

    private string KeyFor(Guid id) => $"announcement:{id}";

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
}