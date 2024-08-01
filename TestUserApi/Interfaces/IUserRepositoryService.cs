using TestUserApi.Models;
using TestUserApi.Models.DTO;

namespace TestUserApi.Interfaces;

public interface IUserRepositoryService
{
    Task<IEnumerable<User>> GetUsers(int pages);
    
    Task<User> UserAdd(UserAddDTO userAddDto); 
    Task<int> GetCountUsers();
}