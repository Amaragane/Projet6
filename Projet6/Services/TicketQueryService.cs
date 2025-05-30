using Microsoft.EntityFrameworkCore;
using Projet6.Data;
using Projet6.Models;
using System;

namespace Projet6.Services
{
    /// <summary>
    /// Service optimisé contenant les 20 requêtes LINQ pour les tickets
    /// Chaque méthode peut gérer plusieurs cas d'usage avec des paramètres optionnels
    /// </summary>
    public class TicketQueryService
    {
        private readonly NexaWorksContext _context;

        public TicketQueryService(NexaWorksContext context)
        {
            _context = context;
        }

        #region Requête principale optimisée

        /// <summary>
        /// Requête universelle pour obtenir des tickets avec filtres flexibles
        /// Remplace les 20 requêtes selon les paramètres utilisés
        /// </summary>
        /// <param name="statut">Statut des tickets ("En cours", "Résolu", null pour tous)</param>
        /// <param name="productId">ID du produit (null pour tous les produits)</param>
        /// <param name="versionId">ID de la version (null pour toutes les versions)</param>
        /// <param name="osId">ID de l'OS (null pour tous les OS)</param>
        /// <param name="startDate">Date de début (null pour ignorer)</param>
        /// <param name="endDate">Date de fin (null pour ignorer)</param>
        /// <param name="keywords">Liste de mots-clés à rechercher (null ou vide pour ignorer)</param>
        /// <param name="useDateResolution">Utiliser DateResolution au lieu de DateCreation pour les filtres de date</param>
        /// <returns>Liste des tickets correspondant aux critères</returns>
        public async Task<List<TicketModel>> GetTicketsAsync(
            string? statut = null,
            int? productId = null, 
            int? versionId = null,
            int? osId = null,
            DateTime? startDate = null, 
            DateTime? endDate = null,
            List<string>? keywords = null,
            bool useDateResolution = false)
        {
            var query = _context.Tickets
                .Include(t => t.Product)
                .Include(t => t.Version)
                .Include(t => t.Os)
                .Include(t => t.Statut)
                .AsQueryable();

            // Filtre par statut
            if (!string.IsNullOrEmpty(statut))
            {
                query = query.Where(t => t.Statut.State == statut);
            }

            // Filtre par produit
            if (productId.HasValue)
            {
                query = query.Where(t => t.ProductId == productId.Value);
            }

            // Filtre par version
            if (versionId.HasValue)
            {
                query = query.Where(t => t.VersionId == versionId.Value);
            }

            // Filtre par OS
            if (osId.HasValue)
            {
                query = query.Where(t => t.OsId == osId.Value);
            }

            // Filtre par période
            if (startDate.HasValue || endDate.HasValue)
            {
                if (useDateResolution)
                {
                    // Utiliser DateResolution pour les tickets résolus
                    if (startDate.HasValue)
                        query = query.Where(t => t.DateResolution >= startDate.Value);
                    if (endDate.HasValue)
                        query = query.Where(t => t.DateResolution <= endDate.Value);
                }
                else
                {
                    // Utiliser DateCreation par défaut
                    if (startDate.HasValue)
                        query = query.Where(t => t.DateCreation >= startDate.Value);
                    if (endDate.HasValue)
                        query = query.Where(t => t.DateCreation <= endDate.Value);
                }
            }

            // Filtre par mots-clés
            if (keywords != null && keywords.Any())
            {
                foreach (var keyword in keywords.Where(k => !string.IsNullOrWhiteSpace(k)))
                {
                    var lowerKeyword = keyword.Trim().ToLower();
                    query = query.Where(t => 
                        EF.Functions.Like(t.Description.ToLower(), $"%{lowerKeyword}%") ||
                        (t.Resolution != null && EF.Functions.Like(t.Resolution.ToLower(), $"%{lowerKeyword}%")));
                }
            }

            // Tri selon le type de requête
            if (useDateResolution && !string.IsNullOrEmpty(statut) && statut == "Résolu")
            {
                query = query.OrderBy(t => t.DateResolution);
            }
            else
            {
                query = query.OrderBy(t => t.DateCreation);
            }

            return await query.ToListAsync();
        }

        #endregion

        #region Les 20 requêtes demandées (wrappers optimisés)

        /// <summary>
        /// Requête 3: Obtenir tous les problèmes en cours pour un produit (une seule version)
        /// OPTIMISÉ: productId ou versionId peuvent être null pour plus de flexibilité
        /// </summary>
        public async Task<List<TicketModel>> GetProblemsInProgressByProductVersionAsync(string? statut,int? productId, int? versionId)
        {
            return await GetTicketsAsync(statut: statut, productId: productId, versionId: versionId);
        }


        /// <summary>
        /// Requête 5: Obtenir tous les problèmes rencontrés au cours d'une période donnée pour un produit (une seule version)
        /// </summary>
        public async Task<List<TicketModel>> GetProblemsByPeriodProductVersionAsync(string? statut, int? productId, int? versionId, DateTime startDate, DateTime endDate)
        {
            return await GetTicketsAsync(statut: statut, productId: productId, versionId: versionId, startDate: startDate, endDate: endDate);
        }


        /// <summary>
        /// Requête 8: Obtenir tous les problèmes en cours pour un produit contenant une liste de mots-clés (une seule version)
        /// </summary>
        public async Task<List<TicketModel>> GetProblemsByProductVersionKeywordsAsync(string? statut, int? productId, int? versionId, List<string> keywords)
        {
            return await GetTicketsAsync(statut: statut, productId: productId, versionId: versionId, keywords: keywords);
        }

        /// <summary>
        /// Requête 10: Obtenir tous les problèmes rencontrés au cours d'une période donnée pour un produit contenant une liste de mots-clés (une seule version)
        /// </summary>
        public async Task<List<TicketModel>> GetProblemsByPeriodProductVersionKeywordsAsync(string? statut,int? productId, int? versionId, DateTime startDate, DateTime endDate, List<string> keywords)
        {
            return await GetTicketsAsync(statut: statut, productId: productId, versionId: versionId, startDate: startDate, endDate: endDate, keywords: keywords);
        }

        #endregion

        #region Méthodes utilitaires essentielles

        /// <summary>
        /// Obtenir tous les produits
        /// </summary>
        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            return await _context.Products.OrderBy(p => p.ProductName).ToListAsync();
        }

        /// <summary>
        /// Obtenir les versions en fonctions du produit
        /// </summary>
        public async Task<List<VersionModel>> GetVersionsAsync(int? productId)
        {
            return await _context.Tickets
                .Where(t =>
                (!productId.HasValue || t.ProductId == productId.Value))
                .Select(t => t.Version)
                .Distinct()
                .OrderBy(v => v.VersionName)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtenir tous les OS
        /// </summary>
        public async Task<List<OsModel>> GetAllOsAsync()
        {
            return await _context.Os.OrderBy(o => o.OsName).ToListAsync();
        }

        #endregion
    }
}