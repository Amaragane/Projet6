using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projet6.Data;
using Projet6.Services;
using Projet6.Models;

namespace Projet6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine(" 🔍 NEXAWORK - SYSTÈME DE REQUÊTES TICKETS 🔍");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            try
            {
                // Configuration du host avec injection de dépendances
                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NexaWorksContext>();
                var queryService = scope.ServiceProvider.GetRequiredService<TicketQueryService>();

                // Vérifier et créer la base de données si nécessaire
                await EnsureDatabaseCreatedAsync(context);

                // Menu principal
                await ShowMainMenuAsync(queryService);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERREUR CRITIQUE AU DÉMARRAGE :");
                Console.WriteLine($"   {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("🔧 SOLUTIONS POSSIBLES :");
                Console.WriteLine("1. Vérifiez que SQL Server LocalDB est installé");
                Console.WriteLine("2. Essayez d'installer SQL Server Express");
                Console.WriteLine("3. Modifiez la chaîne de connexion dans appsettings.json");
                Console.WriteLine();
                Console.WriteLine("Appuyez sur une touche pour quitter...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Essayer plusieurs chaînes de connexion par priorité
                    var connectionString = GetBestConnectionString(context.Configuration);
                    
                    Console.WriteLine($"🔌 Tentative de connexion : {GetConnectionDisplayName(connectionString)}");
                    
                    services.AddDbContext<NexaWorksContext>(options =>
                        options.UseSqlServer(connectionString, sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 3,
                                maxRetryDelay: TimeSpan.FromSeconds(5),
                                errorNumbersToAdd: null);
                        }));
                    
                    services.AddScoped<TicketQueryService>();
                });

        static string GetBestConnectionString(IConfiguration configuration)
        {
            // Liste des chaînes de connexion par ordre de priorité
            var connectionStrings = new[]
            {
                // 1. LocalDB (par défaut)
                "Server=(localdb)\\mssqllocaldb;Database=NexaWorksConsole;Trusted_Connection=true;MultipleActiveResultSets=true;",
                
                // 2. SQL Server Express local
                "Server=.\\SQLEXPRESS;Database=NexaWorksConsole;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;",
                
                // 3. SQL Server local
                "Server=.;Database=NexaWorksConsole;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;",
                
                // 4. Configuration personnalisée
                configuration.GetConnectionString("DefaultConnection")
            };

            // Retourner la première qui n'est pas nulle
            return connectionStrings.FirstOrDefault(cs => !string.IsNullOrEmpty(cs)) 
                   ?? connectionStrings[0];
        }

        static string GetConnectionDisplayName(string connectionString)
        {
            if (connectionString.Contains("(localdb)"))
                return "SQL Server LocalDB";
            if (connectionString.Contains("SQLEXPRESS"))
                return "SQL Server Express";
            if (connectionString.Contains("Server=."))
                return "SQL Server Local";
            return "Configuration personnalisée";
        }

        static async Task EnsureDatabaseCreatedAsync(NexaWorksContext context)
        {
            Console.WriteLine("⚙️  Vérification de la base de données...");
            
            try
            {
                // Test de connexion
                Console.WriteLine("🔗 Test de connexion à la base de données...");
                await context.Database.CanConnectAsync();
                Console.WriteLine("✅ Connexion réussie !");

                // Création de la base si nécessaire
                Console.WriteLine("🏗️  Création de la base de données si nécessaire...");
                var created = await context.Database.EnsureCreatedAsync();
                
                if (created)
                {
                    Console.WriteLine("✅ Base de données créée avec succès !");
                }
                else
                {
                    Console.WriteLine("ℹ️  Base de données déjà existante.");
                }
                
                // Compter les tickets pour vérifier que les données sont présentes
                Console.WriteLine("📊 Vérification des données...");
                var ticketCount = await context.Tickets.CountAsync();
                Console.WriteLine($"📊 Nombre de tickets en base : {ticketCount}");
                
                if (ticketCount == 0)
                {
                    Console.WriteLine("⚠️  Aucun ticket trouvé. Les données de seed n'ont peut-être pas été créées.");
                    Console.WriteLine("🔄 Tentative de recréation de la base...");
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                    ticketCount = await context.Tickets.CountAsync();
                    Console.WriteLine($"📊 Nombre de tickets après recréation : {ticketCount}");
                }
                
                Console.WriteLine();
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                Console.WriteLine($"❌ Erreur SQL : {sqlEx.Message}");
                Console.WriteLine();
                Console.WriteLine("🔧 SOLUTIONS SUGGÉRÉES :");
                
                if (sqlEx.Message.Contains("LocalDB") || sqlEx.Message.Contains("localdb"))
                {
                    Console.WriteLine("📥 1. Installez SQL Server LocalDB :");
                    Console.WriteLine("   - Téléchargez depuis : https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb");
                    Console.WriteLine("   - Ou installez SQL Server Express");
                    Console.WriteLine();
                    Console.WriteLine("🔄 2. Ou modifiez appsettings.json avec cette chaîne :");
                    Console.WriteLine("   \"Server=.\\\\SQLEXPRESS;Database=NexaWorksConsole;Trusted_Connection=true;TrustServerCertificate=true;\"");
                }
                else if (sqlEx.Message.Contains("SQLEXPRESS"))
                {
                    Console.WriteLine("📥 1. Installez SQL Server Express");
                    Console.WriteLine("🔄 2. Ou utilisez une autre instance SQL Server");
                }
                else
                {
                    Console.WriteLine("🔄 1. Vérifiez que SQL Server est démarré");
                    Console.WriteLine("🔄 2. Vérifiez les permissions de connexion");
                    Console.WriteLine($"🔄 3. Erreur détaillée : {sqlEx.Number}");
                }
                
                throw;
            }
            catch (InvalidOperationException invOpEx)
            {
                Console.WriteLine($"❌ Erreur de configuration : {invOpEx.Message}");
                Console.WriteLine();
                Console.WriteLine("🔧 VÉRIFICATIONS :");
                Console.WriteLine("1. Chaîne de connexion dans appsettings.json");
                Console.WriteLine("2. Packages NuGet Entity Framework installés");
                Console.WriteLine("3. Configuration DbContext correcte");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur inattendue : {ex.Message}");
                Console.WriteLine($"🔍 Type : {ex.GetType().Name}");
                Console.WriteLine();
                Console.WriteLine("🔧 Informations de débogage :");
                Console.WriteLine($"   - Exception complète : {ex}");
                throw;
            }
        }

        static async Task ShowMainMenuAsync(TicketQueryService queryService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=".PadRight(60, '='));
                Console.WriteLine(" 🔍 MENU PRINCIPAL - REQUÊTES NEXAWORK");
                Console.WriteLine("=".PadRight(60, '='));
                Console.WriteLine();
                Console.WriteLine("📋 PROBLÈMES EN COURS");
                Console.WriteLine("1️  Tous les problèmes en cours par produit et version");
                Console.WriteLine();
                Console.WriteLine("📅 PAR PÉRIODE");
                Console.WriteLine("2  Problèmes par période, produit et version");
                Console.WriteLine();
                Console.WriteLine("🔎 PAR MOTS-CLÉS");
                Console.WriteLine("3  Recherche dans un produit et version");
                Console.WriteLine();
                Console.WriteLine("🔎📅 PAR PÉRIODE ET PAR MOTS-CLÉS");
                Console.WriteLine("4  Recherche dans un produit et version");
                Console.WriteLine();
                Console.WriteLine("0️  Quitter");
                Console.WriteLine();
                Console.Write("Choisissez une option (0-4) : ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ExecuteQuery1(queryService);
                            break;
                        case "2":
                            await ExecuteQuery2(queryService);
                            break;
                        case "3":
                            await ExecuteQuery3(queryService);
                            break;
                        case "4":
                            await ExecuteQuery4(queryService);
                            break;
                        case "0":
                            Console.WriteLine("\n👋 Au revoir !");
                            return;
                        default:
                            Console.WriteLine("\n❌ Option invalide. Appuyez sur une touche...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Erreur : {ex.Message}");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }
            }
        }

        #region Exécution des requêtes

        static async Task ExecuteQuery1(TicketQueryService queryService)
        {
            Console.Clear();
            Console.WriteLine("📋 Requête 1 : Problèmes en cours par produit et version");
            Console.WriteLine("=".PadRight(60, '='));
            var statut = SelectStatut();

            var productId = await SelectProductAsync(queryService);
            if (productId == -1) return;

            var versionId = await SelectVersionAsync(queryService,productId);
            if (versionId == -1) return;

            var tickets = await queryService.GetProblemsInProgressByProductVersionAsync(statut, productId, versionId);
            DisplayTickets(tickets);
            
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static async Task ExecuteQuery2(TicketQueryService queryService)
        {
            Console.Clear();
            Console.WriteLine("📅 Requête 2 : Problèmes par période, produit et version");
            Console.WriteLine("=".PadRight(60, '='));
            var statut = SelectStatut();

            var productId = await SelectProductAsync(queryService);
            if (productId == -1) return;

            var versionId = await SelectVersionAsync(queryService, productId);
            if (versionId == -1) return;

            var (startDate, endDate) = GetDateRange();
            if (startDate == null || endDate == null) return;

            var tickets = await queryService.GetProblemsByPeriodProductVersionAsync(statut, productId, versionId, startDate.Value, endDate.Value);
            DisplayTickets(tickets);
            
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static async Task ExecuteQuery3(TicketQueryService queryService)
        {
            Console.Clear();
            Console.WriteLine("🔎 Requête 3 : Recherche dans un produit et version");
            Console.WriteLine("=".PadRight(60, '='));
            var statut = SelectStatut();

            var productId = await SelectProductAsync(queryService);
            if (productId == -1) return;

            var versionId = await SelectVersionAsync(queryService, productId);
            if (versionId == -1) return;

            var keywords = GetKeywords();
            if (keywords == null || !keywords.Any()) return;

            var tickets = await queryService.GetProblemsByProductVersionKeywordsAsync(statut, productId, versionId, keywords);
            DisplayTickets(tickets);
            
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        static async Task ExecuteQuery4(TicketQueryService queryService)
        {
            Console.Clear();
            Console.WriteLine("🔎📅 Requête 4 : Recherche dans un produit et version");
            Console.WriteLine("=".PadRight(60, '='));
            var statut =  SelectStatut();

            var productId = await SelectProductAsync(queryService);
            if (productId == -1) return;

            var versionId = await SelectVersionAsync(queryService, productId);
            if (versionId == -1) return;

            var (startDate, endDate) = GetDateRange();
            if (startDate == null || endDate == null) return;

            var keywords = GetKeywords();
            if (keywords == null || !keywords.Any()) return;

            var tickets = await queryService.GetProblemsByPeriodProductVersionKeywordsAsync(statut, productId, versionId, startDate.Value, endDate.Value, keywords);
            DisplayTickets(tickets);

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        #endregion

        #region Méthodes utilitaires

        static async Task<int?> SelectProductAsync(TicketQueryService queryService)
        {
            Console.WriteLine("\n🏢 Sélection du produit :");
            var products = await queryService.GetAllProductsAsync();

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {products[i].ProductName}");
            }
            Console.WriteLine($"{products.Count+1}. Tous");

            Console.Write("\nChoisissez un produit (numéro) : ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= products.Count+1)
            {
                if (choice == products.Count + 1)
                {
                    // Si l'utilisateur choisit "Tous", retourner null pour indiquer tous les produits
                    return null;
                }
                
                return products[choice - 1].ProductId;
            }

            Console.WriteLine("❌ Sélection invalide.");
            return -1;
        }

        static async Task<int?> SelectVersionAsync(TicketQueryService queryService, int? productId)
        {
            Console.WriteLine("\n📱 Sélection de la version :");
            var versions = await queryService.GetVersionsAsync(productId);

            for (int i = 0; i < versions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {versions[i].VersionName}");
            }
            Console.WriteLine($"{versions.Count + 1}. Tous");
            Console.Write("\nChoisissez une version (numéro) : ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= versions.Count+1)
            {
                if (choice == versions.Count + 1)
                {
                    // Si l'utilisateur choisit "Tous", retourner null pour indiquer tous les produits
                    return null;
                }
                return versions[choice - 1].VersionId;
            }

            Console.WriteLine("❌ Sélection invalide.");
            return -1;
        }
        static string? SelectStatut()
        {
            Console.WriteLine("\n📱 Sélection du statut :");
            Console.WriteLine("1: En cours");
            Console.WriteLine("2: Résolus");
            Console.WriteLine("3: Les deux");
            Console.Write("\nChoisissez un statut : ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
            {
                switch (choice)
                {
                    case 1:
                        return "En cours";
                    case 2:
                        return "Résolus";
                    case 3:
                        return null;
                    default:
                        return null;
                }
            }

            Console.WriteLine("❌ Sélection invalide.");
            return null;
        }

        static (DateTime?, DateTime?) GetDateRange()
        {
            Console.WriteLine("\n📅 Saisie de la période :");
            
            Console.Write("Date de début (AAAA-MM-JJ) : ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("❌ Format de date invalide.");
                return (null, null);
            }

            Console.Write("Date de fin (AAAA-MM-JJ) : ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("❌ Format de date invalide.");
                return (null, null);
            }

            return (startDate, endDate);
        }

        static List<string>? GetKeywords()
        {
            Console.WriteLine("\n🔎 Saisie des mots-clés :");
            Console.Write("Entrez les mots-clés séparés par des virgules : ");
            
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Aucun mot-clé saisi.");
                return null;
            }

            return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(k => k.Trim())
                       .Where(k => !string.IsNullOrEmpty(k))
                       .ToList();
        }

        static void DisplayTickets(List<TicketModel> tickets)
        {
            if (!tickets.Any())
            {
                Console.WriteLine("\n📭 Aucun ticket trouvé.");
                return;
            }

            Console.WriteLine($"\n📊 {tickets.Count} ticket(s) trouvé(s) :");
            Console.WriteLine("-".PadRight(120, '-'));

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"🎫 Ticket #{ticket.TicketId} | {ticket.Product?.ProductName} v{ticket.Version?.VersionName} | {ticket.Os?.OsName} | {ticket.Statut?.State}");
                Console.WriteLine($"   📅 Créé: {ticket.DateCreation:dd/MM/yyyy} | Résolu: {(ticket.DateResolution?.ToString("dd/MM/yyyy") ?? "En cours")}");
                Console.WriteLine($"   📝 {TruncateText(ticket.Description, 100)}");
                Console.WriteLine();
            }
        }

        static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;
            
            return text.Substring(0, maxLength) + "...";
        }
        #endregion
    }
}
