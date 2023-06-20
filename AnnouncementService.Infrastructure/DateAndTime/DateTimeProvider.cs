namespace Announcement.Infrastructure.DateAndTime;

public class DateTimeProvider : IDateTimeProvider
{
    public System.DateTime Now() => System.DateTime.Now;
}