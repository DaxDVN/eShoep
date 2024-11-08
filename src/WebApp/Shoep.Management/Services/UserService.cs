using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Auth;

namespace Shoep.Management.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<(List<User> Users, int TotalPages)> GetUsers(int pageNumber = 1, int pageSize = 4)
    {
        var totalUsers = await userManager.Users.CountAsync();

        var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

        var users = await userManager.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalPages);
    }


    public async Task<User?> GetUserById(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found.");
        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found.");
        return user;
    }

    public async Task<bool> AddUser(User user)
    {
        var result = await userManager.CreateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> UpdateUser(User user)
    {
        var existingUser = await userManager.FindByIdAsync(user.Id);
        if (existingUser == null) throw new KeyNotFoundException("User not found.");

        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;

        var result = await userManager.UpdateAsync(existingUser);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found.");

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }


    public async Task<List<User>> SearchUsersByEmail(string email)
    {
        var user = await userManager.Users
            .Where(u => u.Email != null && u.Email.ToLower()
                .Contains(email.ToLower()))
            .ToListAsync();
        return user;
    }
}