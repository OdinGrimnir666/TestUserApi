using TestUserApi.Models;

namespace TestUserApi.Interfaces;

public interface IGroupRepositoryService
{
    public Task<IEnumerable<UserGroup>> GetGroupAsync();
     Task<UserGroup> AddGroupAsync(string groupName);
}