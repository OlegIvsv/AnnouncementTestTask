using AnnouncementService.App.Entities;

namespace AnnouncementService.API.Contracts;

public class AnnouncementRequest
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DateAdded { get; init; }

    public static Announcement ToModel(AnnouncementRequest announcement)
    {
        return new()
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Description = announcement.Description,
            DateAdded = announcement.DateAdded
        };
    }
}