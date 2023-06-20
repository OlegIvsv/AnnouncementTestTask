using Announcement.API.Contracts;
using Announcement.Infrastructure.Database;
using Announcement.Infrastructure.DateAndTime;
using Microsoft.AspNetCore.Mvc;

namespace Announcement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementRepo _repo;
    private readonly IDateTimeProvider _dateTimeProvider;


    public AnnouncementsController(IAnnouncementRepo announcementRepo, IDateTimeProvider dateTimeProvider)
    {
        _repo = announcementRepo;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpPost]
    public async Task<IActionResult> AddAnnouncement([FromBody] AnnouncementRequest request)
    {
        var announcement = AnnouncementRequest.ToModel(request, _dateTimeProvider);
        
        _repo.Add(announcement);
        return Ok();
    }
    
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> RemoveAnnouncement(Guid id)
    {
        var announcement = await _repo.GetById(id);
        if (announcement is null)
            return NotFound();
        await _repo.Delete(id);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAnnouncement([FromBody] AnnouncementRequest request)
    {
        var existingAnnouncement = await _repo.GetById(request.Id ??);
        if (existingAnnouncement is null)
            return NotFound();
        var announcement = AnnouncementRequest.ToModel(request, _dateTimeProvider);
        await _repo.Update(announcement);
        return Ok();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetAnnouncementById(Guid id)
    {
        var announcement = await _repo.GetById(id);
        if (announcement is null)
            return NotFound();
        return Ok(announcement);
    }

    [HttpGet("similar-to/{id}")]
    public async Task<IActionResult> GetSimilar(Guid id, [FromQuery] int length)
    {
        var existingAnnouncement = await _repo.GetById(id);
        if (existingAnnouncement is null)
            return BadRequest();
        var listOfSimilar = await _repo.GetSimilar(id, length);
        return Ok(listOfSimilar);
    }
}