namespace Announcement.App.Entities;

public class AnnouncementModel   
{
    public Guid Id { get; set; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DateAdded { get; init; }
}