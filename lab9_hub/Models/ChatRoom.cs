using System;
using System.Collections.Generic;

namespace lab9_hub.Models;

public partial class ChatRoom
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
