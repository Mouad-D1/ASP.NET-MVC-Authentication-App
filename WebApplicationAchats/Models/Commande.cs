using System;
using System.Collections.Generic;

namespace WebApplicationAchats.Models;

public partial class Commande
{
    public int Numcom { get; set; }

    public int Numcli { get; set; }

    public DateOnly? Datecom { get; set; }

    public virtual ICollection<DetailCommande> DetailCommandes { get; set; } = new List<DetailCommande>();

    public virtual Client NumcliNavigation { get; set; } = null!;
}
