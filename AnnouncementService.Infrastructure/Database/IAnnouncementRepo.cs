using Announcement.App.Entities;
using FluentResults;

namespace Announcement.Infrastructure.Database;

public interface IAnnouncementRepo
{
    public Task<AnnouncementModel?> GetById(Guid id);
    public Task<bool> Delete(Guid id);
    public Task Update(AnnouncementModel announcement);
    public Task Add(AnnouncementModel announcement);
    public Task<Result<IList<AnnouncementModel>>> GetSimilar(Guid id, int length);
}