using AnnouncementService.App.Entities;
using FluentResults;

namespace AnnouncementService.Infrastructure.Database;

public interface IAnnouncementRepo
{
    public Task<Result<Announcement>> GetById(Guid id);
    public Task<bool> Delete(Guid id);
    public Task Update(Announcement announcement);
    public Task Add(Announcement announcement);
    public Task<Result<IList<Announcement>>> GetSimilar(Guid id, int n);
}