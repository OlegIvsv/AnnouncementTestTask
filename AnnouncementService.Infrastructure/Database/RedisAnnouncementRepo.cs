using AnnouncementService.App.Entities;
using FluentResults;

namespace AnnouncementService.Infrastructure.Database;

public class RedisAnnouncementRepo : IAnnouncementRepo
{
    public Task<Result<Announcement>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Announcement>> Update(Announcement announcement)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Announcement>> Add(Announcement announcement)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IList<Announcement>>> GetSimilar(Guid id, int n)
    {
        throw new NotImplementedException();
    }
}