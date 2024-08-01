namespace TestUserApi.Models.DTO;

public class UserAddDTO
{
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public List<int> IdGroups { get; set; }
    
}