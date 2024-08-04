using lab9_hub.Models;
using lab9_hub.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace lab9_hub.Services;

public class UserService : IUserService
{
    private readonly  ChatContext _context;

    public UserService(ChatContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
