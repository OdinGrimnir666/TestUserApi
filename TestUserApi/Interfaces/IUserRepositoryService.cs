using TestUserApi.Models;
using TestUserApi.Models.DTO;

namespace TestUserApi.Interfaces;

public interface IUserRepositoryService
{
    Task<IEnumerable<User>> GetUsersAsync(int pages,CancellationToken token);
    
    Task<User> UserAddAsync(UserAddDTO userAddDto,CancellationToken token); 
    Task<int> GetCountUsersAsync(CancellationToken token);
}