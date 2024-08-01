using Dapper;
using Npgsql;
using TestUserApi.Interfaces;
using TestUserApi.Models;

namespace TestUserApi.Services;

public class GroupRepositoryService : IGroupRepositoryService
{
    private string _connectionString;
    
    public GroupRepositoryService(IConfiguration configuration)
    {
        this._connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<IEnumerable<UserGroup>> GetGroupAsync(CancellationToken token)
    {
        
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(token);
        var sqlGetGroup = @"SELECT group_id as idGroup, group_name as groupName FROM public.groups";
        var group = await connection.QueryAsync<UserGroup>(sqlGetGroup,token);
        await connection.CloseAsync();
        return group;
    }
    
     public async Task<UserGroup> AddGroupAsync(string groupName,CancellationToken token)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string insertGroupSql = @"
            INSERT INTO groups (group_name)
            VALUES (@groupName)
            RETURNING group_id;";

        const string getGroup = @"SELECT group_id as IdGroup, group_name as GroupName
                                FROM public.""groups""
                                WHERE group_id = @group_id;";
        
        await connection.OpenAsync(token);
        using (var transaction = await connection.BeginTransactionAsync(token))
        {
            

            var groupId = await connection.QueryAsync<int>(insertGroupSql,
                param: new { groupName = groupName }, transaction);
            
            var groups = await connection.QuerySingleAsync<UserGroup>(getGroup,new { group_id = groupId.FirstOrDefault() });
            await transaction.CommitAsync(token);
            await connection.CloseAsync();
            return groups;
        }
    }
}