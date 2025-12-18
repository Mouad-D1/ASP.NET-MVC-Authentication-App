using System;
using System.Collections.Generic;

namespace WebApplicationAchats.Models;

public partial class DetailCommande
{
    public int IdDetail { get; set; }

    public int IdCommande { get; set; }

    public int IdProduit { get; set; }

    public int? Quantite { get; set; }

    public virtual Commande IdCommandeNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
