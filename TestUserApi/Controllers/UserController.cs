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
    public async Task<IActionResult> GetUsers(int pages, CancellationToken token)
    {
        var users = await _userRepositoryService.GetUsersAsync(pages,token);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> AddUsers(UserAddDTO userAddDto,CancellationToken token)
    {
        var user = await _userRepositoryService.UserAddAsync(userAddDto,token);
        return Ok(user);
    }

    [HttpGet("/CountUsers")]
    public async Task<IActionResult> GetUsersCountAsync(CancellationToken token)
    {
        var countUser =  await _userRepositoryService.GetCountUsersAsync(token);
        return Ok(new {count = countUser});
    }
    

}