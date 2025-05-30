using Microsoft.EntityFrameworkCore;
using Projet6.Models;
using Projet6.Data.Seed;

namespace Projet6.Data
{
    public class NexaWorksContext : DbContext
    {
        public NexaWorksContext(DbContextOptions<NexaWorksContext> options)
            : base(options)
        {
        }

        // DbSets pour les entités
        public DbSet<ProductModel> Products { get; set; } = null!;
        public DbSet<VersionModel> Versions { get; set; } = null!;
        public DbSet<OsModel> Os { get; set; } = null!;
        public DbSet<TicketModel> Tickets { get; set; } = null!;
        public DbSet<StatutModel> Statuts { get; set; } = null!;
        public DbSet<ProductVersionOsSupport> ProductVersionOsSupport { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations
            ConfigureRelationships(modelBuilder);

            // Configuration des indexes et contraintes
            ConfigureIndexesAndConstraints(modelBuilder);

            // Appel de la classe SeedData pour l'amorçage
            SeedData.Configure(modelBuilder);
        }

        /// <summary>
        /// Configure toutes les relations entre les entités
        /// </summary>
        private static void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Relation OS -> Tickets (One-to-Many)
            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Os)
                .WithMany(os => os.Tickets)
                .HasForeignKey(t => t.OsId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation Statut -> Tickets (One-to-Many)
            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Statut)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.StatutId) 
                .OnDelete(DeleteBehavior.Restrict);

            // Relation Product -> Tickets (One-to-Many)
            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Product)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation Version -> Tickets (One-to-Many)
            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Version)
                .WithMany(v => v.Tickets)
                .HasForeignKey(t => t.VersionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration de la table de jointure ProductVersionOsSupport
            modelBuilder.Entity<ProductVersionOsSupport>(entity =>
            {
                // Clé primaire composée des 3 clés étrangères
                entity.HasKey(pvos => new { pvos.ProductId, pvos.VersionId, pvos.OsId });

                // Relation avec Product
                entity.HasOne(pvos => pvos.Product)
                    .WithMany()
                    .HasForeignKey(pvos => pvos.ProductId)
                    .HasConstraintName("FK_ProductVersionOsSupport_Product")
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation avec Version
                entity.HasOne(pvos => pvos.Version)
                    .WithMany()
                    .HasForeignKey(pvos => pvos.VersionId)
                    .HasConstraintName("FK_ProductVersionOsSupport_Version")
                    .OnDelete(DeleteBehavior.Restrict);

                // Relation avec OS
                entity.HasOne(pvos => pvos.Os)
                    .WithMany()
                    .HasForeignKey(pvos => pvos.OsId)
                    .HasConstraintName("FK_ProductVersionOsSupport_Os")
                    .OnDelete(DeleteBehavior.Restrict);

                // Nom de la table
                entity.ToTable("ProductVersionOsSupport");
            });
        }

        /// <summary>
        /// Configure les indexes et contraintes pour optimiser les performances et garantir l'intégrité
        /// </summary>
        private static void ConfigureIndexesAndConstraints(ModelBuilder modelBuilder)
        {
            // Index sur les clés étrangères pour améliorer les performances des jointures
            modelBuilder.Entity<TicketModel>()
                .HasIndex(t => t.VersionId)
                .HasDatabaseName("IX_Ticket_VersionId");

            modelBuilder.Entity<TicketModel>()
                .HasIndex(t => t.OsId)
                .HasDatabaseName("IX_Ticket_OsId");

            modelBuilder.Entity<TicketModel>()
                .HasIndex(t => t.ProductId)
                .HasDatabaseName("IX_Ticket_ProductId");

            // Index sur le statut des tickets pour les requêtes de filtrage
            modelBuilder.Entity<TicketModel>()
                .HasIndex(t => t.StatutId)
                .HasDatabaseName("IX_Ticket_StatutId");

            // Index sur la date de création pour les requêtes de tri chronologique
            modelBuilder.Entity<TicketModel>()
                .HasIndex(t => t.DateCreation)
                .HasDatabaseName("IX_Ticket_DateCreation");
        }
    }
}
