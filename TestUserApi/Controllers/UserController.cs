using Microsoft.AspNetCore.Mvc;
using TestUserApi.Interfaces;
using TestUserApi.Models.DTO;

namespace TestUserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private IUserRepositoryService _userRepositoryService;

    public UserController(IUserRepositoryService userRepositoryService)
    {
        this._userRepositoryService = userRepositoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(int pages)
    {
        var users = await _userRepositoryService.GetUsers(pages);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> AddUsers(UserAddDTO userAddDto)
    {
        var user = await _userRepositoryService.UserAdd(userAddDto);
        return Ok(user);
    }

    [HttpGet("/CountUsers")]
    public async Task<IActionResult> GetUsersCount()
    {
        var countUser =  await _userRepositoryService.GetCountUsers();
        return Ok(new {count = countUser});
    }
    

}