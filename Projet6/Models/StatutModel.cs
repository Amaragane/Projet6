using System.ComponentModel.DataAnnotations;

namespace Projet6.Models
{
    /// <summary>
    /// Modèle représentant le statut d'un ticket de support
    /// Statuts possibles : "En cours", "Résolu"
    /// </summary>
    public class StatutModel
    {
        // Clé primaire
        [Key]
        public int StatutId { get; set; }
        
        // Propriété scalaire - État du ticket
        [Required(ErrorMessage = "L'Etat est requis.")]
        public string State { get; set; } = null!;

        // Propriété de navigation - Collection des tickets ayant ce statut
        public virtual ICollection<TicketModel> Tickets { get; set; } = new List<TicketModel>();
    }
}
