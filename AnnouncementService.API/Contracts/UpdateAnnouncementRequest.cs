using Announcement.Infrastructure.DateAndTime;

namespace Announcement.API.Contracts;

public class UpdateAnnouncementRequest
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime? DateAdded { get; init; }

    public static App.Entities.AnnouncementModel ToModel(UpdateAnnouncementRequest updateAnnouncement, IDateTimeProvider datetime)
    {
        return new()
        {
            Id = updateAnnouncement.Id,
            Title = updateAnnouncement.Title,
            Description = updateAnnouncement.Description,
            DateAdded = datetime.Now()
        };
    }
}