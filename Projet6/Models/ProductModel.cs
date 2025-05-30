using System.ComponentModel.DataAnnotations;

namespace Projet6.Models
{
    /// <summary>
    /// Modèle représentant un produit de l'écosystème NexaWork
    /// (Trader en Herbe, Maître des Investissements, Planificateur d'Entraînement, Planificateur d'Anxiété Sociale)
    /// </summary>
    public class ProductModel
    {
        // Clé primaire
        [Key]
        public int ProductId { get; set; }
        
        // Propriété scalaire - Nom du produit
        [Required(ErrorMessage = "Le nom est requis.")]
        public string ProductName { get; set; } = null!;
        
        // Propriété de navigation - Collection des tickets associés à ce produit
        public virtual ICollection<TicketModel> Tickets { get; set; } = new List<TicketModel>();
    }
}
