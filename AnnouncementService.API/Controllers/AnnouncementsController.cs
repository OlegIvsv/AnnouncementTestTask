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
    public async Task<IActionResult> AddAnnouncement([FromBody] AddAnnouncementRequest request)
    {
        var announcement = AddAnnouncementRequest.ToModel(request, _dateTimeProvider);
        await _repo.Add(announcement);
        return CreatedAtAction(nameof(AddAnnouncement), announcement);
    }
    
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> RemoveAnnouncement(Guid id)
    {
        if(await _repo.Delete(id))
            return Ok();
        return NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAnnouncement([FromBody] UpdateAnnouncementRequest request)
    {
        var announcement = UpdateAnnouncementRequest.ToModel(request, _dateTimeProvider);
        var existingAnnouncement = await _repo.GetById(announcement.Id);
        if (existingAnnouncement is null)
            return NotFound();
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
        var announcement = await _repo.GetById(id);
        if (announcement is null)
            return BadRequest();
        var queryText = $"{announcement.Title} {announcement.Description}";
        var listOfSimilar = await _repo.GetSimilar(queryText, length);
        return Ok(listOfSimilar);
    }
}