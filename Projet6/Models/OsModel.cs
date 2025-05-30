using System.ComponentModel.DataAnnotations;

namespace Projet6.Models
{
    /// <summary>
    /// Modèle représentant un système d'exploitation
    /// Systèmes supportés : Linux, MacOS, Windows, Android, iOS, Windows Mobile
    /// </summary>
    public class OsModel
    {
        // Clé primaire
        [Key]
        public int OsId { get; set; }
        
        // Propriété scalaire - Nom du système d'exploitation
        [Required(ErrorMessage = "L'OS est requis.")]
        public string OsName { get; set; } = null!;

        // Propriété de navigation - Collection des tickets associés à ce système
        public virtual ICollection<TicketModel> Tickets { get; set; } = new List<TicketModel>();
    }
}
