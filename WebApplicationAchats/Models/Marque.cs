namespace WebApplicationAchats.Models
{
    public class Marque
    {
        public int MarqueId { get; set; }
        public string Nom { get; set; }

        // Relation : Une marque peut avoir plusieurs produits
        public ICollection<Produit>? Produits { get; set; }
    }
}