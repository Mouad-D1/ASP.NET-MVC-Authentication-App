using System;
using System.Collections.Generic;

namespace WebApplicationAchats.Models;

public partial class Clientadress
{
    public int Numcli { get; set; }

    public string? Address1 { get; set; }

    public string? State { get; set; }

    public virtual Client NumcliNavigation { get; set; } = null!;
}
