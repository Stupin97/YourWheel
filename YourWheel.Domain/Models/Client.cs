using System;
using System.Collections.Generic;

namespace YourWheel.Domain.Models;

public partial class Client
{
    public Guid Clientid { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
