using System;
using System.Collections.Generic;

namespace WebApplicationAchats.Models;

public partial class Client
{
    public int Numcli { get; set; }

    public string? Nomcli { get; set; }

    public string? Ville { get; set; }

    public string? Categorie { get; set; }

    public string? Compte { get; set; }

    public virtual Clientadress? Clientadress { get; set; }

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}
