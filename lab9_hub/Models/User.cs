using System;
using System.Collections.Generic;

namespace lab9_hub.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public virtual ICollection<Message> MessageReceiverUsernameNavigations { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenderUsernameNavigations { get; set; } = new List<Message>();
}
