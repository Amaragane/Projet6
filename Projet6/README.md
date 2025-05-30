# 🔍 Application Console NexaWork - Gestion des Tickets

## Vue d'ensemble

Cette application console .NET 8 reproduit exactement la même logique de base de données et les mêmes requêtes que le projet ASP.NET Core, mais dans une interface console simple et efficace.

## 🏗️ Architecture

### Structure du projet
```
Projet6/
├── Models/                    # Modèles de données (identiques au projet ASP.NET)
│   ├── TicketModel.cs
│   ├── ProductModel.cs
│   ├── VersionModel.cs
│   ├── OsModel.cs
│   ├── StatutModel.cs
│   └── ProductVersionOsSupport.cs
├── Data/                      # Contexte et données
│   ├── NexaWorksContext.cs
│   └── Seed/
│       └── SeedData.cs       # ⚠️ À compléter par copier-coller
├── Services/                  # Services de requêtes
│   └── TicketQueryService.cs
├── Program.cs                 # Point d'entrée avec interface console
└── appsettings.json          # Configuration de la base de données
```

## 🚀 Installation et démarrage

### Prérequis
- .NET 8 SDK
- SQL Server ou SQL Server LocalDB

### Étapes d'installation

1. **Compléter le fichier SeedData.cs**
   ```bash
   # Copier le contenu complet depuis le projet ASP.NET :
   # ModelisezEtcreezUneBaseDeDonnee/Data/Seed/SeedData.cs
   # vers Projet6/Data/Seed/SeedData.cs
   ```

2. **Restaurer les packages NuGet**
   ```bash
   cd Projet6
   dotnet restore
   ```

3. **Configurer la base de données** (optionnel)
   ```json
   // Dans appsettings.json, modifier si nécessaire :
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NexaWorksConsole;Trusted_Connection=true"
   }
   ```

4. **Lancer l'application**
   ```bash
   dotnet run
   ```

## 🎯 Fonctionnalités

### Menu principal
L'application propose un menu intuitif avec emojis organisé en sections :

#### 📋 Problèmes en cours
- **1️⃣** Tous les problèmes en cours (tous produits)
- **2️⃣** Problèmes en cours par produit
- **3️⃣** Problèmes en cours par produit et version

#### 📅 Problèmes par période
- **4️⃣** Problèmes par période et produit
- **5️⃣** Problèmes par période, produit et version

#### 🔎 Recherche par mots-clés
- **6️⃣** Recherche dans tous les produits
- **7️⃣** Recherche dans un produit
- **8️⃣** Recherche dans un produit et version

#### ✅ Problèmes résolus
- **11** Tous les problèmes résolus
- **12** Problèmes résolus par produit
- **13** Problèmes résolus par produit et version

#### 📊 Statistiques
- **20** Statistiques par produit
- **21** Liste des produits
- **22** Liste des versions
- **23** Liste des OS

## 🔧 Utilisation

### Exemple d'exécution de requête

1. **Sélection de menu**
   ```
   Choisissez une option (0-23) : 2
   ```

2. **Sélection de produit**
   ```
   🏢 Sélection du produit :
   1. Trader en Herbe
   2. Maître des Investissements
   3. Planificateur d'Entraînement
   4. Planificateur d'Anxiété Sociale
   
   Choisissez un produit (numéro) : 3
   ```

3. **Affichage des résultats**
   ```
   📊 5 ticket(s) trouvé(s) :
   ────────────────────────────────────────
   🎫 Ticket #1 | Planificateur d'Entraînement v1.1 | iOS | En cours
      📅 Créé: 03/03/2024 | Résolu: En cours
      📝 L'application se ferme brutalement lors de la tentative d'importation...
   ```

### Formats de saisie

#### Dates
```
Format attendu : AAAA-MM-JJ
Exemple : 2024-01-01
```

#### Mots-clés
```
Format : mots séparés par des virgules
Exemple : mémoire, crash, GPS
```

## 🎨 Interface

### Caractéristiques
- **Emojis** pour une navigation visuelle
- **Couleurs** via l'encodage UTF-8
- **Menus numérotés** pour une sélection rapide
- **Messages d'erreur** clairs et informatifs
- **Affichage paginé** des résultats

### Navigation
- **Retour automatique** au menu principal après chaque requête
- **Validation des saisies** avec messages d'erreur
- **Option 0** pour quitter à tout moment

## ⚙️ Configuration technique

### Base de données
- **Création automatique** au premier lancement
- **Seeding automatique** des données de test
- **Vérification de l'intégrité** au démarrage

### Injection de dépendances
```csharp
services.AddDbContext<NexaWorksContext>();
services.AddScoped<TicketQueryService>();
```

### Gestion d'erreurs
- **Try-catch** sur toutes les opérations
- **Messages utilisateur** compréhensibles
- **Logs** des erreurs techniques

## 📊 Données

### Structure identique au projet ASP.NET
- **25 tickets** de test provenant du document source
- **4 produits** NexaWork
- **6 versions** (1.0 à 2.1)
- **6 systèmes** d'exploitation
- **Matrice de compatibilité** produit/version/OS

### Exemples de données
```
Produits : Trader en Herbe, Maître des Investissements, 
          Planificateur d'Entraînement, Planificateur d'Anxiété Sociale
OS : Linux, Windows, MacOS, Android, iOS, WindowsMobile
Statuts : En cours, Résolu
```

## 🚀 Avantages de l'approche console

### Performance
- ✅ **Démarrage rapide** (< 2 secondes)
- ✅ **Consommation mémoire réduite**
- ✅ **Pas de serveur web** à gérer

### Simplicité
- ✅ **Moins de fichiers** que l'ASP.NET Core
- ✅ **Interface directe** sans HTML/CSS/JS
- ✅ **Debugging simplifié**

### Portabilité
- ✅ **Fonctionne partout** où .NET 8 est installé
- ✅ **Pas de dépendances web**
- ✅ **Déploiement simple** (single file possible)

## 🔍 Comparaison avec le projet ASP.NET

| Aspect | Console | ASP.NET Core |
|--------|---------|--------------|
| **Complexité** | ⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Performance** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| **Interface** | ⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Maintenance** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| **Déploiement** | ⭐⭐⭐⭐⭐ | ⭐⭐ |

## 🛠️ Extensions possibles

### Ajouts faciles
- **Export CSV** des résultats
- **Mode batch** pour requêtes automatisées
- **Historique** des requêtes exécutées
- **Configuration** via arguments de ligne de commande

### Code d'exemple pour export CSV
```csharp
static void ExportToCsv(List<TicketModel> tickets, string filename)
{
    var csv = new StringBuilder();
    csv.AppendLine("Id,Produit,Version,OS,Statut,DateCreation,Description");
    
    foreach (var ticket in tickets)
    {
        csv.AppendLine($"{ticket.TicketId},{ticket.Product?.ProductName}," +
                      $"{ticket.Version?.VersionName},{ticket.Os?.OsName}," +
                      $"{ticket.Statut?.State},{ticket.DateCreation:yyyy-MM-dd}," +
                      $"\"{ticket.Description.Replace("\"", "\"\"")}\"");
    }
    
    File.WriteAllText(filename, csv.ToString());
}
```

## 🎯 Cas d'usage recommandés

### Parfait pour
- ✅ **Démonstrations rapides** des requêtes LINQ
- ✅ **Tests de performance** de la base de données
- ✅ **Scripts d'administration** et maintenance
- ✅ **Prototypage** de nouvelles requêtes

### Moins adapté pour
- ❌ **Interface utilisateur grand public**
- ❌ **Accès concurrent** massif
- ❌ **Intégration web** ou APIs

## 🏁 Conclusion

Cette application console offre une **alternative légère et efficace** au projet ASP.NET Core, parfaite pour :
- **Comprendre** la logique de base de données
- **Tester** rapidement les requêtes LINQ
- **Démontrer** les fonctionnalités sans complexité web

Elle conserve **100% de la logique métier** tout en éliminant la complexité de l'infrastructure web, ce qui en fait un excellent complément au projet principal.
