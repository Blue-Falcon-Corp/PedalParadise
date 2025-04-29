// Services/UserService.cs
using Microsoft.EntityFrameworkCore;
using PedalParadise.Data;
using PedalParadise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PedalParadise.Services
{
    public class UserService : IUserService
    {
        private readonly PedalParadiseContext _context;

        public UserService(PedalParadiseContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            // In a real app, you'd use proper password hashing
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == email && u.Password == password);
            return user;
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            // In a real app, you'd hash the password
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null) return false;

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByRoleAsync(string role)
        {
            var employees = await _context.Employees
                .Include(e => e.User)
                .Where(e => e.User != null && e.User.Discriminator == role)
                .ToListAsync();
            return employees;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
    }
}