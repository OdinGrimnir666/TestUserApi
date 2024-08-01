
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestUserApi.Interfaces;
using TestUserApi.Models.DTO;

namespace TestUserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    private IGroupRepositoryService _groupRepositoryService;

    public GroupController(IGroupRepositoryService groupRepositoryService)
    {
        this._groupRepositoryService = groupRepositoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroup()
    {
         var group = await _groupRepositoryService.GetGroupAsync();

         return Ok(group);
    }
    [HttpPost]
    public async Task<IActionResult> GetGroup(GroupAddDTO groupDTO)
    {
        
        var group = await _groupRepositoryService.AddGroupAsync(groupDTO.NameGroup);

        return Ok(group);
    }
}