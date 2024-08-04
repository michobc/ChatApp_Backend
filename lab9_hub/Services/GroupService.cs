using lab9_hub.Models;
using lab9_hub.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace lab9_hub.Services;

public class GroupService : IGroupService
{
    private readonly  ChatContext _context;

    public GroupService(ChatContext context)
    {
        _context = context;
    }

    public async Task<string> GetGroupAsync(int id)
    {
        var chatroom = await _context.ChatRooms.FindAsync(id);
        return chatroom.Name;
    }
    
    public async Task<IEnumerable<ChatRoom>> GetAllGroupsAsync()
    {
        return await _context.ChatRooms.ToListAsync();
    }
}