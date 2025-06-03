<Query Kind="Statements">
  <Connection>
    <ID>50132f2b-54f5-4d97-b0b1-bd1e1c616067</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Database>NexaWorksConsole</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

// ===============================================
// NEXAWORK - 20 REQUÊTES LINQ POUR LINQPAD (VERSION CORRIGÉE)
// ===============================================
// Description : Les 20 requêtes demandées pour le système de tickets NexaWork
// Compatible avec : LINQPad 6+ et Entity Framework Core
// Base de données : NexaWorksConsole
// Correction : Problème "Local sequence cannot be used in LINQ to SQL" résolu
// ===============================================

// Variables de configuration - Modifiez selon vos besoins
var produitRecherche = "Planificateur d'Entraînement";
var versionRecherche = "2.0";
var produitResolu = "Trader en Herbe";
var versionResolue = "1.2";
var produitMotsCles = "Planificateur d'Anxiété Sociale";
var versionMotsCles = "1.0";

var dateDebut = new DateTime(2024, 3, 1);
var dateFin = new DateTime(2024, 4, 30);
var dateDebutResolution = new DateTime(2024, 2, 1);
var dateFinResolution = new DateTime(2024, 4, 30);

var motsCles = new List<string> { "mémoire", "crash", "GPS", "audio" };

Console.WriteLine("🔍 DÉMARRAGE DES REQUÊTES NEXAWORK");
Console.WriteLine("=" + new string('=', 50));

// ===============================================
// 1. OBTENIR TOUS LES PROBLÈMES EN COURS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 1 : Tous les problèmes en cours (tous produits) ===");
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
requete1.Dump("1. Problèmes en cours (tous produits)");

// ===============================================
// 2. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 2 : Problèmes en cours pour un produit ===");
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
requete2.Dump($"2. Problèmes en cours pour {produitRecherche}");

// ===============================================
// 3. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 3 : Problèmes en cours pour un produit et une version ===");
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
requete3.Dump($"3. Problèmes en cours pour {produitRecherche} v{versionRecherche}");

// ===============================================
// 4. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 4 : Problèmes par période pour un produit ===");
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
requete4.Dump($"4. Problèmes du {dateDebut:dd/MM/yyyy} au {dateFin:dd/MM/yyyy} pour {produitRecherche}");

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
requete5.Dump($"5. Problèmes du {dateDebut:dd/MM/yyyy} au {dateFin:dd/MM/yyyy} pour {produitRecherche} v{versionRecherche}");

// ===============================================
// 6. OBTENIR TOUS LES PROBLÈMES EN COURS CONTENANT UNE LISTE DE MOTS-CLÉS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 6 : Problèmes en cours contenant des mots-clés ===");
var requete6 = (from ticket in Tickets
                where ticket.Statut.State == "En cours"
                      && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                          motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                          motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                          motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                }).ToList()
                .Select(t => new {
                    t.TicketId,
                    t.Produit,
                    t.Version,
                    t.OS,
                    t.DateCreation,
                    t.Description,
                    MotsClesTouves = motsCles.Where(mot => 
                        t.Description.Contains(mot) || 
                        (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                    ).ToList()
                });
requete6.Dump("6. Problèmes en cours avec mots-clés");

// ===============================================
// 7. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 7 : Problèmes en cours pour un produit avec mots-clés ===");
var requete7 = (from ticket in Tickets
                where ticket.Statut.State == "En cours"
                      && ticket.Product.ProductName == produitRecherche
                      && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                          motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                          motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                          motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                }).ToList()
                .Select(t => new {
                    t.TicketId,
                    t.Produit,
                    t.Version,
                    t.OS,
                    t.DateCreation,
                    t.Description,
                    MotsClesTouves = motsCles.Where(mot => 
                        t.Description.Contains(mot) || 
                        (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                    ).ToList()
                });
requete7.Dump($"7. Problèmes en cours pour {produitRecherche} avec mots-clés");

// ===============================================
// 8. OBTENIR TOUS LES PROBLÈMES EN COURS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 8 : Problèmes en cours pour un produit/version avec mots-clés ===");
var requete8 = (from ticket in Tickets
                where ticket.Statut.State == "En cours"
                      && ticket.Product.ProductName == produitRecherche
                      && ticket.Version.VersionName == versionRecherche
                      && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                          motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                          motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                          motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    DateCreation = ticket.DateCreation,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                }).ToList()
                .Select(t => new {
                    t.TicketId,
                    t.Produit,
                    t.Version,
                    t.OS,
                    t.DateCreation,
                    t.Description,
                    MotsClesTouves = motsCles.Where(mot => 
                        t.Description.Contains(mot) || 
                        (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                    ).ToList()
                });
requete8.Dump($"8. Problèmes en cours pour {produitRecherche} v{versionRecherche} avec mots-clés");

// ===============================================
// 9. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 9 : Problèmes par période pour un produit avec mots-clés ===");
var requete9 = (from ticket in Tickets
                where ticket.DateCreation >= dateDebut 
                      && ticket.DateCreation <= dateFin
                      && ticket.Product.ProductName == produitRecherche
                      && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                          motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                          motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                          motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
                select new {
                    TicketId = ticket.TicketId,
                    Produit = ticket.Product.ProductName,
                    Version = ticket.Version.VersionName,
                    OS = ticket.Os.OsName,
                    Statut = ticket.Statut.State,
                    DateCreation = ticket.DateCreation,
                    Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                }).ToList()
                .Select(t => new {
                    t.TicketId,
                    t.Produit,
                    t.Version,
                    t.OS,
                    t.Statut,
                    t.DateCreation,
                    t.Description,
                    MotsClesTouves = motsCles.Where(mot => 
                        t.Description.Contains(mot) || 
                        (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                    ).ToList()
                });
requete9.Dump($"9. Problèmes période + {produitRecherche} + mots-clés");

// ===============================================
// 10. OBTENIR TOUS LES PROBLÈMES RENCONTRÉS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 10 : Problèmes par période pour un produit/version avec mots-clés ===");
var requete10 = (from ticket in Tickets
                 where ticket.DateCreation >= dateDebut 
                       && ticket.DateCreation <= dateFin
                       && ticket.Product.ProductName == produitRecherche
                       && ticket.Version.VersionName == versionRecherche
                       && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                           motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                           motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                           motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
                 select new {
                     TicketId = ticket.TicketId,
                     Produit = ticket.Product.ProductName,
                     Version = ticket.Version.VersionName,
                     OS = ticket.Os.OsName,
                     Statut = ticket.Statut.State,
                     DateCreation = ticket.DateCreation,
                     Description = ticket.Description.Substring(0, Math.Min(100, ticket.Description.Length)) + "..."
                 }).ToList()
                 .Select(t => new {
                     t.TicketId,
                     t.Produit,
                     t.Version,
                     t.OS,
                     t.Statut,
                     t.DateCreation,
                     t.Description,
                     MotsClesTouves = motsCles.Where(mot => 
                         t.Description.Contains(mot) || 
                         (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                     ).ToList()
                 });
requete10.Dump($"10. Problèmes période + {produitRecherche} v{versionRecherche} + mots-clés");

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
requete11.Dump("11. Tous les problèmes résolus");

// ===============================================
// 12. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 12 : Problèmes résolus pour un produit ===");
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
requete12.Dump($"12. Problèmes résolus pour {produitResolu}");

// ===============================================
// 13. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 13 : Problèmes résolus pour un produit/version ===");
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
requete13.Dump($"13. Problèmes résolus pour {produitResolu} v{versionResolue}");

// ===============================================
// 14. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 14 : Problèmes résolus par période pour un produit ===");
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
requete14.Dump($"14. Problèmes résolus entre {dateDebutResolution:dd/MM/yyyy} et {dateFinResolution:dd/MM/yyyy}");

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
requete15.Dump($"15. Problèmes résolus période + {produitResolu} v{versionResolue}");

// ===============================================
// 16. OBTENIR TOUS LES PROBLÈMES RÉSOLUS CONTENANT UNE LISTE DE MOTS-CLÉS (TOUS LES PRODUITS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 16 : Problèmes résolus avec mots-clés ===");
var requete16 = (from ticket in Tickets
                 where ticket.Statut.State == "Résolu"
                       && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                           motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                           motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                           motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
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
                 }).ToList()
                 .Select(t => new {
                     t.TicketId,
                     t.Produit,
                     t.Version,
                     t.OS,
                     t.DateCreation,
                     t.DateResolution,
                     t.DureeResolution,
                     t.Description,
                     MotsClesTouves = motsCles.Where(mot => 
                         t.Description.Contains(mot) || 
                         (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                     ).ToList()
                 });
requete16.Dump("16. Problèmes résolus avec mots-clés");

// ===============================================
// 17. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 17 : Problèmes résolus pour un produit avec mots-clés ===");
var requete17 = (from ticket in Tickets
                 where ticket.Statut.State == "Résolu"
                       && ticket.Product.ProductName == produitMotsCles
                       && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                           motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                           motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                           motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
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
                 }).ToList()
                 .Select(t => new {
                     t.TicketId,
                     t.Produit,
                     t.Version,
                     t.OS,
                     t.DateCreation,
                     t.DateResolution,
                     t.DureeResolution,
                     t.Description,
                     MotsClesTouves = motsCles.Where(mot => 
                         t.Description.Contains(mot) || 
                         (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                     ).ToList()
                 });
requete17.Dump($"17. Problèmes résolus pour {produitMotsCles} avec mots-clés");

// ===============================================
// 18. OBTENIR TOUS LES PROBLÈMES RÉSOLUS POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 18 : Problèmes résolus pour un produit/version avec mots-clés ===");
var requete18 = (from ticket in Tickets
                 where ticket.Statut.State == "Résolu"
                       && ticket.Product.ProductName == produitMotsCles
                       && ticket.Version.VersionName == versionMotsCles
                       && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                           motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                           motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                           motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
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
                 }).ToList()
                 .Select(t => new {
                     t.TicketId,
                     t.Produit,
                     t.Version,
                     t.OS,
                     t.DateCreation,
                     t.DateResolution,
                     t.DureeResolution,
                     t.Description,
                     MotsClesTouves = motsCles.Where(mot => 
                         t.Description.Contains(mot) || 
                         (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                     ).ToList()
                 });
requete18.Dump($"18. Problèmes résolus pour {produitMotsCles} v{versionMotsCles} avec mots-clés");

// ===============================================
// 19. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (TOUTES LES VERSIONS)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 19 : Problèmes résolus par période pour un produit avec mots-clés ===");
var requete19 = (from ticket in Tickets
                 where ticket.Statut.State == "Résolu"
                       && ticket.DateResolution >= dateDebutResolution 
                       && ticket.DateResolution <= dateFinResolution
                       && ticket.Product.ProductName == produitResolu
                       && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                           motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                           motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                           motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
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
                 }).ToList()
                 .Select(t => new {
                     t.TicketId,
                     t.Produit,
                     t.Version,
                     t.OS,
                     t.DateCreation,
                     t.DateResolution,
                     t.DureeResolution,
                     t.Description,
                     MotsClesTouves = motsCles.Where(mot => 
                         t.Description.Contains(mot) || 
                         (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                     ).ToList()
                 });
requete19.Dump($"19. Problèmes résolus période + {produitResolu} + mots-clés");

// ===============================================
// 20. OBTENIR TOUS LES PROBLÈMES RÉSOLUS AU COURS D'UNE PÉRIODE DONNÉE POUR UN PRODUIT CONTENANT UNE LISTE DE MOTS-CLÉS (UNE SEULE VERSION)
// ===============================================
Console.WriteLine("\n=== REQUÊTE 20 : Problèmes résolus par période pour un produit/version avec mots-clés ===");
var requete20 = (from ticket in Tickets
                 where ticket.Statut.State == "Résolu"
                       && ticket.DateResolution >= dateDebutResolution 
                       && ticket.DateResolution <= dateFinResolution
                       && ticket.Product.ProductName == produitResolu
                       && ticket.Version.VersionName == versionResolue
                       && (motsCles.Contains("mémoire") && (ticket.Description.Contains("mémoire") || (ticket.Resolution != null && ticket.Resolution.Contains("mémoire"))) ||
                           motsCles.Contains("crash") && (ticket.Description.Contains("crash") || (ticket.Resolution != null && ticket.Resolution.Contains("crash"))) ||
                           motsCles.Contains("GPS") && (ticket.Description.Contains("GPS") || (ticket.Resolution != null && ticket.Resolution.Contains("GPS"))) ||
                           motsCles.Contains("audio") && (ticket.Description.Contains("audio") || (ticket.Resolution != null && ticket.Resolution.Contains("audio"))))
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
                 }).ToList()
                 .Select(t => new {
                     t.TicketId,
                     t.Produit,
                     t.Version,
                     t.OS,
                     t.DateCreation,
                     t.DateResolution,
                     t.DureeResolution,
                     t.Description,
                     MotsClesTouves = motsCles.Where(mot => 
                         t.Description.Contains(mot) || 
                         (Tickets.FirstOrDefault(tick => tick.TicketId == t.TicketId)?.Resolution?.Contains(mot) ?? false)
                     ).ToList()
                 });
requete20.Dump($"20. Problèmes résolus période + {produitResolu} v{versionResolue} + mots-clés");

Console.WriteLine("\n=== FIN DES 20 REQUÊTES NEXAWORK ===");
Console.WriteLine("Toutes les requêtes ont été exécutées avec succès !");
Console.WriteLine($"Date d'exécution : {DateTime.Now:dd/MM/yyyy HH:mm:ss}");