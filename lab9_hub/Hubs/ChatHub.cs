using lab9_hub.Models;
using lab9_hub.Services;
using Microsoft.AspNetCore.SignalR;

namespace lab9_hub.Hubs;

public class ChatHub : Hub
{
    private readonly ChatServiceImpl _chatService;
    private static readonly Dictionary<string, string> UserConnections = new();
    private static readonly Dictionary<string, List<Message>> UnreadMessages = new();
    
    public ChatHub(ChatServiceImpl chatService)
    {
        _chatService = chatService;
    }

    public async Task Connect(string userId)
    {
        var connectionId = Context.ConnectionId;
        UserConnections[userId] = connectionId;
        
        // Clear unread messages when user reconnects
        if (UnreadMessages.ContainsKey(userId))
        {
            UnreadMessages[userId].Clear();
        }
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
            _chatService.SendMessage(chatMessage, null);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", chatMessage);

            // Add to unread messages
            if (!UnreadMessages.ContainsKey(receiverId))
            {
                UnreadMessages[receiverId] = new List<Message>();
            }
            UnreadMessages[receiverId].Add(chatMessage);

            // Optionally notify the receiver about new messages
            await Clients.Client(connectionId).SendAsync("ReceiveNotification", UnreadMessages[receiverId].Count);
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
    
    public async Task MarkMessagesAsRead(string userId)
    {
        if (UnreadMessages.ContainsKey(userId))
        {
            UnreadMessages[userId].Clear();
            // Notify client that unread count is cleared
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", 0);
            }
        }
    }

}