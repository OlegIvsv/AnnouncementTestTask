using Microsoft.AspNetCore.Mvc;

namespace Announcement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AnnouncementsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAnnouncement()
    {
        
    }
    
    [HttpDelete]
    public async Task<IActionResult> RemoveAnnouncement()
    {
        
    }

    [HttpPut]
    public async Task<IActionResult> RemoveAnnouncement()
    {

    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> RemoveAnnouncement(Guid id)
    {

    }

    [HttpGet("similar-to/{id}")]
    public async Task<IActionResult> SimilarTo(Guid id, [FromQuery] int length)
    {
        
    }
}