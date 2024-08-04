using System;
using System.Collections.Generic;

namespace lab9_hub.Models;

public partial class Message
{
    public int Id { get; set; }

    public string SenderUsername { get; set; } = null!;

    public string ReceiverUsername { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual User ReceiverUsernameNavigation { get; set; } = null!;

    public virtual User SenderUsernameNavigation { get; set; } = null!;

    public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
}
