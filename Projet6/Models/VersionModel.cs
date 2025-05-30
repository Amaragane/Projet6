using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet6.Models
{
    /// <summary>
    /// Modèle représentant une version d'un produit
    /// Exemples : "1.0", "1.1", "2.0", "2.1", etc.
    /// </summary>
    public class VersionModel
    {
        // Clé primaire
        [Key]
        public int VersionId { get; set; }

        // Propriété scalaire - Numéro/nom de la version
        [Required(ErrorMessage ="Un numéro de version est nécéssaire")]
        public string VersionName { get; set; } = null!;

        // Propriété de navigation - Collection des tickets associés à cette version
        public virtual ICollection<TicketModel> Tickets { get; set; } = new List<TicketModel>();
    }
}
