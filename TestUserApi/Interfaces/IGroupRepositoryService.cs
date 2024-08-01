using TestUserApi.Models;

namespace TestUserApi.Interfaces;

public interface IGroupRepositoryService
{
    public Task<IEnumerable<UserGroup>> GetGroupAsync(CancellationToken token);
     Task<UserGroup> AddGroupAsync(string groupName,CancellationToken token);
}