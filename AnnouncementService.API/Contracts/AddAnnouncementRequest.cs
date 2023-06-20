using Announcement.Infrastructure.DateAndTime;

namespace Announcement.API.Contracts;

public class AddAnnouncementRequest
{
    public string Title { get; init; }
    public string Description { get; init; }

    public static App.Entities.AnnouncementModel ToModel(AddAnnouncementRequest updateAnnouncement, IDateTimeProvider datetime)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Title = updateAnnouncement.Title,
            Description = updateAnnouncement.Description,
            DateAdded = datetime.Now()
        };
    }
}