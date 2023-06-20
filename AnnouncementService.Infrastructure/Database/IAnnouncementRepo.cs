using AnnouncementService.App.Entities;
using FluentResults;

namespace AnnouncementService.Infrastructure.Database;

public interface IAnnouncementRepo
{
    public Task<Result<Announcement>> GetById(Guid id);
    public Task<bool> Delete(Guid id);
    public void Update(Announcement announcement);
    public void Add(Announcement announcement);
    public Task<Result<IList<Announcement>>> GetSimilar(Guid id, int n);
}