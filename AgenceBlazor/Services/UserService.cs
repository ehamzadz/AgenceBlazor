// Services/UserService.cs
using AgenceBlazor.Data;
using AgenceBlazor.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> ValidateUser(string username, string password)
    {
        try
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
            {
                Console.WriteLine($"User {username} not found");
                return null;
            }

            Console.WriteLine($"Found user: {username}");
            Console.WriteLine($"Stored hash: {user.PasswordHash}");

            var isValid = VerifyPassword(password, user.PasswordHash);
            Console.WriteLine($"Password valid: {isValid}");

            if (isValid)
                return user;

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    // Services/UserService.cs - Update CreateUser method
    public async Task<bool> CreateUser(string username, string password, string role = "User")
    {
        try
        {
            // Check if user exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null)
            {
                // Update existing user's password
                existingUser.PasswordHash = HashPassword(password);
                existingUser.Role = role;
                existingUser.IsActive = true;
                await _context.SaveChangesAsync();
                return true;
            }

            // Create new user
            var user = new ApplicationUser
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Role = role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating/updating user: {ex.Message}");
            throw;
        }
    }

    private string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hash == hashed;
    }
}