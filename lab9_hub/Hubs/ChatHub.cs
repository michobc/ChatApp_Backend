using lab9_hub.Models;
using lab9_hub.Services;
using Microsoft.AspNetCore.SignalR;

namespace lab9_hub.Hubs;

public class ChatHub : Hub
{
    private readonly ChatServiceImpl _chatService;
    private static readonly Dictionary<string, string> UserConnections = new();
    
    public ChatHub(ChatServiceImpl chatService)
    {
        _chatService = chatService;
    }

    public async Task Connect(string userId)
    {
        var connectionId = Context.ConnectionId;
        UserConnections[userId] = connectionId;
    }

    public async Task Disconnect(string userId)
    {
        UserConnections.Remove(userId);
    }
    
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
    
    public async Task SendMessage(string senderId, string receiverId, string message)
    {
        var chatMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Text = message,
            Timestamp = DateTime.UtcNow.ToString("o")
        };

        if (UserConnections.TryGetValue(receiverId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", chatMessage);
        }
        else
        {
            Console.WriteLine($"User {receiverId} is not connected.");
        }
    }
    
    public async Task SendMessageToGroup(string groupName, string senderId, string message)
    {
        var chatMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = "Group",
            Text = message,
            Timestamp = DateTime.UtcNow.ToString("o")
        };

        await Clients.Group(groupName: groupName).SendAsync("ReceiveMessage", chatMessage);
    }

}