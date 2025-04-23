// Services/IUserService.cs
using PedalParadise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User> RegisterUserAsync(User user, string password);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesByRoleAsync(string role);
    Task<Client> CreateClientAsync(Client client);
}