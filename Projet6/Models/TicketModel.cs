using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projet6.Models
{
    /// <summary>
    /// Modèle représentant un ticket de support technique
    /// Entité centrale du système reliant produits, versions, OS et statuts
    /// </summary>
    public class TicketModel
    {
        // Clé primaire
        [Key]
        public int TicketId { get; set; }

        // Propriété scalaire - Date de création du ticket
        [Required(ErrorMessage ="Date de création requise")]
        public DateTime DateCreation { get; set; }

        // Propriété scalaire - Date de résolution (nullable - null si non résolu)
        public DateTime? DateResolution { get; set; }

        // Propriété scalaire - Description du problème
        [Required(ErrorMessage = "Description requise")]
        public string Description { get; set; } = null!;

        // Propriété scalaire - Description de la résolution (nullable)
        public string? Resolution { get; set; }

        // Clé étrangère vers VersionModel
        [Required]
        [ForeignKey("Version")]
        public int VersionId { get; set; }

        // Clé étrangère vers OsModel
        [Required]
        [ForeignKey("Os")]
        public int OsId { get; set; }
        
        // Clé étrangère vers ProductModel
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        // Clé étrangère vers StatutModel
        [Required]
        [ForeignKey("Statut")]
        public int StatutId { get; set; }

        // Propriétés de navigation - Références vers les entités liées
        public virtual VersionModel Version { get; set; } = null!;
        public virtual ProductModel Product { get; set; } = null!;
        public virtual OsModel Os { get; set; } = null!;
        public virtual StatutModel Statut { get; set; } = null!;
    }
}
