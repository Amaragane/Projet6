using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet6.Models
{
    /// <summary>
    /// Table de jointure entre Product, Version et OS
    /// Représente quelles versions de quels produits sont supportées sur quels OS
    /// Permet de définir la matrice de compatibilité produit/version/OS
    /// </summary>
    public class ProductVersionOsSupport
    {
        // Clé étrangère composite - Référence vers ProductModel
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        // Clé étrangère composite - Référence vers VersionModel
        [Required]
        [ForeignKey("Version")]
        public int VersionId { get; set; }

        // Clé étrangère composite - Référence vers OsModel
        [Required]
        [ForeignKey("Os")]
        public int OsId { get; set; }

        // Propriétés de navigation - Références vers les entités liées
        public virtual ProductModel Product { get; set; } = null!;
        public virtual VersionModel Version { get; set; } = null!;
        public virtual OsModel Os { get; set; } = null!;
    }
}
