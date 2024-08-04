using lab9_hub.Models;
using Microsoft.EntityFrameworkCore;

namespace lab9_hub.Services;

using Grpc.Core;
using System.Threading.Tasks;

public class ChatServiceImpl : ChatService.ChatServiceBase
{
    private readonly ChatContext _context;

    public ChatServiceImpl(ChatContext context)
    {
        _context = context;
    }

    public override async Task<Empty> SendMessage(Message request, ServerCallContext context)
    {
        var chatMessage = new Models.Message()
        {
            SenderUsername = request.SenderId,
            ReceiverUsername = request.ReceiverId,
            Text = request.Text,
            Timestamp = DateTime.Parse(request.Timestamp).ToUniversalTime()
        };

        _context.Messages.Add(chatMessage);
        await _context.SaveChangesAsync();

        return new Empty();
    }

    public override async Task<ChatHistoryResponse> GetMessages(ChatHistoryRequest request, ServerCallContext context)
    {
        var messages = await _context.Messages
            .Where(m => m.SenderUsername == request.SenderId || m.ReceiverUsername == request.ReceiverId)
            .ToListAsync();

        var response = new ChatHistoryResponse();
        response.Messages.AddRange(messages.Select(m => new Message
        {
            SenderId = m.SenderUsername,
            ReceiverId = m.ReceiverUsername,
            Text = m.Text,
            Timestamp = m.Timestamp.ToString("o")
        }));

        return response;
    }
}
