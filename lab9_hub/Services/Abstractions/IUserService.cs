using lab9_hub.Models;

namespace lab9_hub.Services.Abstractions;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
}
