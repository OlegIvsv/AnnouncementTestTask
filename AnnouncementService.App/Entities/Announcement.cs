namespace AnnouncementService.App.Entities;

public class Announcement   
{
    public Guid Id { get; set; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DateAdded { get; init; }
}