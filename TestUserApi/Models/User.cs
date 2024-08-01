namespace TestUserApi.Models;

public class User
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }

    public List<UserGroup> UserGroups { get; set; } = new List<UserGroup>();


}

public class UserGroup
{
    public string IdGroup { get; set; }
    public string GroupName { get; set; }
}