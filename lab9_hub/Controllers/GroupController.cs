using lab9_hub.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace lab9_hub.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var group = await _groupService.GetAllGroupsAsync();
        return Ok(group);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _groupService.GetGroupAsync(id);
        return Ok(group);
    }
}