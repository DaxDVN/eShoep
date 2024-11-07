using Shoep.Management.Models.Auth;

namespace Shoep.Management.Interfaces;

public interface IUserService
{
    Task<(List<User> Users, int TotalPages)> GetUsers(int pageNumber = 1, int pageSize = 4);
    Task<User?> GetUserById(string id);
    Task<User?> GetUserByEmail(string email);
    Task<bool> AddUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(string id);
}