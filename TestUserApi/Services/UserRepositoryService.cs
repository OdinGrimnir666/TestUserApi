using System.Data.SqlClient;
using Dapper;
using Npgsql;
using TestUserApi.Interfaces;
using TestUserApi.Models;
using TestUserApi.Models.DTO;

namespace TestUserApi.Services;

public class UserRepositoryService(IConfiguration configuration) : IUserRepositoryService
{
    private int countPages = 10;
    private string? _connectionString = configuration.GetConnectionString("DefaultConnection");
    
    
    
    public async Task<IEnumerable<User>> GetUsers(int pages)
    {
        var skip = pages != 0 ? pages * this.countPages : pages;
        
        using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();
        
        var qwery = @"SELECT u.id, u.username, u.email, g.group_name as GroupName,g.group_id as IdGroup  
        FROM users u 
        JOIN public.usergroups  ug ON u.id = ug.user_id 
        join public.""groups"" g  ON ug.group_id = g.group_id
        WHERE u.id In
              (SELECT DISTINCT id FROM users ORDER BY id LIMIT @limit OFFSET @offset);
        ";
        
        var userDictionary = new Dictionary<int, User>();
        var listUsers = await connection.QueryAsync<User, UserGroup, User>(qwery, (user, group) =>
        {
            if (!userDictionary.TryGetValue(user.Id, out var existingUser))
            {
       
                existingUser = user;
                existingUser.UserGroups = new List<UserGroup>();
                userDictionary.Add(user.Id, existingUser);
            }

            if (group != null)
            {
                existingUser.UserGroups.Add(group);
            }

            return existingUser;
        }, splitOn: "IdGroup,GroupName", param: new { limit = this.countPages, offset = skip  });
        
        return userDictionary.Values;
    }


    public async Task<User> UserAdd(UserAddDTO userAddDto)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string insertUserSql = @"
            INSERT INTO users (username , email)
            VALUES (@UserName, @Email)
            RETURNING id;";

        var qwery =
            @"SELECT u.""id"", u.username, u.""email"", g.""group_name"" as GroupName,g.""group_id"" as IdGroup  
        FROM Users u 
        JOIN public.usergroups  ug ON u.""id"" = ug.user_id 
        join public.""groups"" g  ON ug.group_id = g.group_id
        WHERE u.""id"" = @user_id";

        const string SelectInsertGroupSql = @"INSERT INTO public.usergroups (user_id, group_id)
                VALUES
                 (@userdid, @groupid);";
        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync();
       


        var userId = await connection.QueryAsync<int>(insertUserSql,
            param: new { UserName = userAddDto.UserName, Email = userAddDto.Email }, transaction);

        foreach (var id_groups in userAddDto.IdGroups)
        {
            await connection.QueryAsync(SelectInsertGroupSql,
                new { userdid = userId.FirstOrDefault(), groupid = id_groups }, transaction);
        }

        var userDictionary = new Dictionary<int, User>();

        var user = await connection.QueryAsync<User, UserGroup, User>(sql: qwery,
            (user, group) =>
            {
                if (!userDictionary.TryGetValue(user.Id, out var existingUser))
                {
                    // Если нет, добавьте его в словарь
                    existingUser = user;
                    existingUser.UserGroups = new List<UserGroup>();
                    userDictionary.Add(user.Id, existingUser);
                }

                // Добавьте группу пользователю
                if (group != null)
                {
                    existingUser.UserGroups.Add(group);
                }

                return existingUser;

            }, splitOn: "IdGroup,GroupName", param: new { user_id = userId.FirstOrDefault() });

        await transaction.CommitAsync();
        await connection.CloseAsync();
        return userDictionary.Values.FirstOrDefault();
    }





    public async Task<int> GetCountUsers()
    {
        
        var sqlQwery = @"SELECT Count(*)FROM public.users;";
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var countUsers = await connection.QuerySingleAsync<int>(sqlQwery);
        await connection.CloseAsync();
        return countUsers;
    }
}
    

