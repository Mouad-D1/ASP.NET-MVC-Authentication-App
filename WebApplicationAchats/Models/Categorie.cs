namespace WebApplicationAchats.Models
{
    public class Categorie
    {
        public int CategorieId { get; set; }
        public string Nom { get; set; }

        // Relation : Une catégorie contient plusieurs produits
        public ICollection<Produit>? Produits { get; set; }
    }
}