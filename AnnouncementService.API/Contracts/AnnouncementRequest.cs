using Announcement.Infrastructure.DateAndTime;

namespace Announcement.API.Contracts;

public class AnnouncementRequest
{
    public Guid? Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime? DateAdded { get; init; }

    public static App.Entities.AnnouncementModel ToModel(AnnouncementRequest announcement, IDateTimeProvider datetime)
    {
        return new()
        {
            Id = announcement.Id ?? Guid.NewGuid(),
            Title = announcement.Title,
            Description = announcement.Description,
            DateAdded = datetime.Now()
        };
    }
}