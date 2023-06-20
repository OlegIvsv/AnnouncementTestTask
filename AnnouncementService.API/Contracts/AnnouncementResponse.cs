using Announcement.App.Entities;

namespace Announcement.API.Contracts;

public class AnnouncementResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DateAdded { get; init; }

    public static AnnouncementResponse FromModel(AnnouncementModel announcement)
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