<Query Kind="Statements">
  <Connection>
    <ID>nexawork-db</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-token">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>(localdb)\mssqllocaldb</Server>
    <Database>NexaWorksConsole</Database>
    <DisplayName>NexaWork Database</DisplayName>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

// ===============================================
// NEXAWORK - 20 REQUÊTES LINQ POUR LINQPAD
// ===============================================
// Fichier : RequetesNexaWork.linq
// Description : Les 20 requêtes demandées pour le système de tickets NexaWork
// Compatible avec : LINQPad 6+ et Entity Framework Core
// ===============================================

// ===============================================
// 1. OBTENIR TOUS LES PROBLÈMES EN COURS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("=== REQUÊTE 1 : Tous les problèmes en cours (tous produits) ===");
var requete1 = from ticket in Tickets
               where ticket.Statut.State == "En cours"
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
               };
requete1.Dump("Requête 1 - Problèmes en cours");

// ===============================================
// 2. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 2 : Problèmes en cours pour un produit ===");
var produitRecherche = "Planificateur d'Entraînement";
var requete2 = from ticket in Tickets
               where ticket.Statut.State == "En cours" 
                     && ticket.Product.ProductName == produitRecherche
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
               };
requete2.Dump($"Requête 2 - Problèmes en cours pour {produitRecherche}");

// ===============================================
// 3. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 3 : Problèmes en cours pour un produit et une version ===");
var versionRecherche = "2.0";
var requete3 = from ticket in Tickets
               where ticket.Statut.State == "En cours" 
                     && ticket.Product.ProductName == produitRecherche
                     && ticket.Version.VersionName == versionRecherche
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
               };
requete3.Dump($"Requête 3 - Problèmes en cours pour {produitRecherche} v{versionRecherche}");

// ===============================================
// 4. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 4 : Problèmes par période pour un produit ===");
var dateDebut = new DateTime(2024, 3, 1);
var dateFin = new DateTime(2024, 4, 30);
var requete4 = from ticket in Tickets
               where ticket.DateCreation >= dateDebut 
                     && ticket.DateCreation <= dateFin
                     && ticket.Product.ProductName == produitRecherche
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   Statut = ticket.Statut.State,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
               };
requete4.Dump($"Requête 4 - Problèmes du {dateDebut:dd/MM/yyyy} au {dateFin:dd/MM/yyyy} pour {produitRecherche}");

// ===============================================
// 5. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 5 : Problèmes par période pour un produit et une version ===");
var requete5 = from ticket in Tickets
               where ticket.DateCreation >= dateDebut 
                     && ticket.DateCreation <= dateFin
                     && ticket.Product.ProductName == produitRecherche
                     && ticket.Version.VersionName == versionRecherche
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   Statut = ticket.Statut.State,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
               };
requete5.Dump($"Requête 5 - Problèmes du {dateDebut:dd/MM/yyyy} au {dateFin:dd/MM/yyyy} pour {produitRecherche} v{versionRecherche}");

// ===============================================
// 6. OBTENIR TOUS LES PROBLÈMES EN COURS CONTENANT UNE LISTE DE MOTS-CLÉS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 6 : Problèmes en cours contenant des mots-clés ===");
var motsCles = new List<string> { "mémoire", "crash", "GPS", "audio" };
var requete6 = from ticket in Tickets
               where ticket.Statut.State == "En cours"
                     && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                           (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                   MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                         (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
               };
requete6.Dump("Requête 6 - Problèmes en cours avec mots-clés");

// ===============================================
// 7. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 7 : Problèmes en cours pour un produit avec mots-clés ===");
var requete7 = from ticket in Tickets
               where ticket.Statut.State == "En cours"
                     && ticket.Product.ProductName == produitRecherche
                     && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                           (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                   MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                         (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
               };
requete7.Dump($"Requête 7 - Problèmes en cours pour {produitRecherche} avec mots-clés");

// ===============================================
// 8. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 8 : Problèmes en cours pour un produit/version avec mots-clés ===");
var requete8 = from ticket in Tickets
               where ticket.Statut.State == "En cours"
                     && ticket.Product.ProductName == produitRecherche
                     && ticket.Version.VersionName == versionRecherche
                     && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                           (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                   MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                         (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
               };
requete8.Dump($"Requête 8 - Problèmes en cours pour {produitRecherche} v{versionRecherche} avec mots-clés");

// ===============================================
// 9. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 9 : Problèmes par période pour un produit avec mots-clés ===");
var requete9 = from ticket in Tickets
               where ticket.DateCreation >= dateDebut 
                     && ticket.DateCreation <= dateFin
                     && ticket.Product.ProductName == produitRecherche
                     && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                           (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
               select new {
                   TicketId = ticket.TicketId,
                   Produit = ticket.Product.ProductName,
                   Version = ticket.Version.VersionName,
                   OS = ticket.Os.OsName,
                   Statut = ticket.Statut.State,
                   DateCreation = ticket.DateCreation,
                   Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                   MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                         (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
               };
requete9.Dump($"Requête 9 - Problèmes période + {produitRecherche} + mots-clés");

// ===============================================
// 10. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 10 : Problèmes par période pour un produit/version avec mots-clés ===");
var requete10 = from ticket in Tickets
                where ticket.DateCreation >= dateDebut 
                      && ticket.DateCreation <= dateFin
                      && ticket.Product.ProductName == produitRecherche
                      && ticket.Version.VersionName == versionRecherche
                      && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                            (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    Statut = ticket.Statut.State,
                    DateCreation = ticket.DateCreation,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                    MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                          (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
                };
requete10.Dump($"Requête 10 - Problèmes période + {produitRecherche} v{versionRecherche} + mots-clés");

// ===============================================
// 11. OBTENIR TOUS LES PROBLÈMES RÉSOLUS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 11 : Tous les problèmes résolus ===");
var requete11 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                };
requete11.Dump("Requête 11 - Tous les problèmes résolus");

// ===============================================
// 12. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 12 : Problèmes résolus pour un produit ===");
var produitResolu = "Trader en Herbe";
var requete12 = from ticket in Tickets
                where ticket.Statut.State == "Résolu" 
                      && ticket.Product.ProductName == produitResolu
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                };
requete12.Dump($"Requête 12 - Problèmes résolus pour {produitResolu}");

// ===============================================
// 13. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 13 : Problèmes résolus pour un produit/version ===");
var versionResolue = "1.2";
var requete13 = from ticket in Tickets
                where ticket.Statut.State == "Résolu" 
                      && ticket.Product.ProductName == produitResolu
                      && ticket.Version.VersionName == versionResolue
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                };
requete13.Dump($"Requête 13 - Problèmes résolus pour {produitResolu} v{versionResolue}");

// ===============================================
// 14. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 14 : Problèmes résolus par période pour un produit ===");
var dateDebutResolution = new DateTime(2024, 2, 1);
var dateFinResolution = new DateTime(2024, 4, 30);
var requete14 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && ticket.DateResolution >= dateDebutResolution 
                      && ticket.DateResolution <= dateFinResolution
                      && ticket.Product.ProductName == produitResolu
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                };
requete14.Dump($"Requête 14 - Problèmes résolus entre {dateDebutResolution:dd/MM/yyyy} et {dateFinResolution:dd/MM/yyyy}");

// ===============================================
// 15. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 15 : Problèmes résolus par période pour un produit/version ===");
var requete15 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && ticket.DateResolution >= dateDebutResolution 
                      && ticket.DateResolution <= dateFinResolution
                      && ticket.Product.ProductName == produitResolu
                      && ticket.Version.VersionName == versionResolue
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                };
requete15.Dump($"Requête 15 - Problèmes résolus période + {produitResolu} v{versionResolue}");

// ===============================================
// 16. OBTENIR TOUS LES PROBLÈMES RÉSOLUS CONTENANT UNE LISTE DE MOTS-CLÉS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 16 : Problèmes résolus avec mots-clés ===");
var requete16 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                            (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                    MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                          (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
                };
requete16.Dump("Requête 16 - Problèmes résolus avec mots-clés");

// ===============================================
// 17. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 17 : Problèmes résolus pour un produit avec mots-clés ===");
var produitMotsCles = "Planificateur d'Anxiété Sociale";
var requete17 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && ticket.Product.ProductName == produitMotsCles
                      && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                            (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                    MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                          (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
                };
requete17.Dump($"Requête 17 - Problèmes résolus pour {produitMotsCles} avec mots-clés");

// ===============================================
// 18. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 18 : Problèmes résolus pour un produit/version avec mots-clés ===");
var versionMotsCles = "1.0";
var requete18 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && ticket.Product.ProductName == produitMotsCles
                      && ticket.Version.VersionName == versionMotsCles
                      && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                            (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                    MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                          (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
                };
requete18.Dump($"Requête 18 - Problèmes résolus pour {produitMotsCles} v{versionMotsCles} avec mots-clés");

// ===============================================
// 19. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 19 : Problèmes résolus par période pour un produit avec mots-clés ===");
var requete19 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && ticket.DateResolution >= dateDebutResolution 
                      && ticket.DateResolution <= dateFinResolution
                      && ticket.Product.ProductName == produitResolu
                      && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                            (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                    MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                          (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
                };
requete19.Dump($"Requête 19 - Problèmes résolus période + {produitResolu} + mots-clés");

// ===============================================
// 20. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 20 : Problèmes résolus par période pour un produit/version avec mots-clés ===");
var requete20 = from ticket in Tickets
                where ticket.Statut.State == "Résolu"
                      && ticket.DateResolution >= dateDebutResolution 
                      && ticket.DateResolution <= dateFinResolution
                      && ticket.Product.ProductName == produitResolu
                      && ticket.Version.VersionName == versionResolue
                      && motsCles.Any(mot => ticket.Description.Contains(mot) || 
                                            (ticket.Resolution != null && ticket.Resolution.Contains(mot)))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    DateResolution = ticket.DateResolution,
                    DureeResolution = ticket.DateResolution.HasValue ? 
                                     (ticket.DateResolution.Value - ticket.DateCreation).Days : 0,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "...",
                    MotsClesTouves = motsCles.Where(mot => ticket.Description.Contains(mot) || 
                                                          (ticket.Resolution != null && ticket.Resolution.Contains(mot))).ToList()
                };
requete20.Dump($"Requête 20 - Problèmes résolus période + {produitResolu} v{versionResolue} + mots-clés");

Console.WriteLine("\n=== FIN DES 20 REQUÊTES NEXAWORK ===");
Console.WriteLine("Toutes les requêtes ont été exécutées avec succès !");
Console.WriteLine($"Date d'exécution : {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

// ===============================================
// NOTES D'UTILISATION POUR LINQPAD :
// ===============================================
/*
INSTRUCTIONS POUR UTILISER CE FICHIER DANS LINQPAD :

1. CONFIGURATION DE LA CONNEXION :
   - Ouvrez LINQPad 6 ou supérieur
   - Cliquez sur "Add connection" 
   - Sélectionnez "Entity Framework Core"
   - Configurez la chaîne de connexion vers votre base NexaWorksConsole
   - Server: (localdb)\mssqllocaldb ou .\SQLEXPRESS
   - Database: NexaWorksConsole

2. EXÉCUTION :
   - Collez ce code dans un nouveau fichier LINQPad
   - Changez le type de requête en "C# Statements"
   - Sélectionnez votre connexion NexaWork dans la liste déroulante
   - Appuyez sur F5 pour exécuter

3. PERSONNALISATION :
   - Modifiez les variables en haut du fichier :
     * produitRecherche, versionRecherche
     * dateDebut, dateFin, dateDebutResolution, dateFinResolution
     * motsCles (liste des mots à rechercher)
   
4. RÉSULTATS :
   - Chaque requête affiche ses résultats dans un tableau séparé
   - Les descriptions sont tronquées à 100 caractères pour la lisibilité
   - Les mots-clés trouvés sont affichés dans une colonne dédiée
   - Les durées de résolution sont calculées en jours

5. DÉPANNAGE :
   - Si erreur de connexion : vérifiez que la base de données existe
   - Si tables non trouvées : assurez-vous que les migrations ont été appliquées
   - Si pas de données : vérifiez que le seed data a été exécuté

6. ADAPTATION :
   - Pour utiliser avec une autre base : modifiez la connexion
   - Pour d'autres produits : changez les noms dans les variables
   - Pour d'autres périodes : ajustez les dates
   - Pour d'autres mots-clés : modifiez la liste motsCles

EXEMPLE DE VARIABLES À PERSONNALISER :
var produitRecherche = "Votre Produit";
var versionRecherche = "1.0";
var dateDebut = new DateTime(2024, 1, 1);
var dateFin = new DateTime(2024, 12, 31);
var motsCles = new List<string> { "erreur", "bug", "crash" };
*/