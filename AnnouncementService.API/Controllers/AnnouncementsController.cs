using Microsoft.AspNetCore.Mvc;

namespace AnnouncementService.API.Controllers;

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
    public async Task<IActionResult> UpdateAnnouncement()
    {

    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetAnnouncementById(Guid id)
    {

    }

    [HttpGet("similar-to/{id}")]
    public async Task<IActionResult> GetSimilar(Guid id, [FromQuery] int length)
    {
        
    }
}