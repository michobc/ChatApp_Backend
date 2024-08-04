using lab9_hub.Models;

namespace lab9_hub.Services.Abstractions;

public interface IGroupService
{
    Task<string> GetGroupAsync(int id);
    Task<IEnumerable<ChatRoom>> GetAllGroupsAsync();
}