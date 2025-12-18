using System;
using System.Collections.Generic;

namespace WebApplicationAchats.Models;

public partial class Produit
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Prix { get; set; }

    public int Quantite { get; set; }

    public DateOnly? DateAjout { get; set; }

    public bool? Disponible { get; set; }

    public virtual ICollection<DetailCommande> DetailCommandes { get; set; } = new List<DetailCommande>();
    // Clé étrangère vers Categorie
    public int? CategorieId { get; set; }

    // Propriété de navigation vers Categorie
    public Categorie? Categorie { get; set; }

    // Clé étrangère vers Marque
    public int? MarqueId { get; set; }

    // Propriété de navigation vers Marque
    public Marque? Marque { get; set; }
}
