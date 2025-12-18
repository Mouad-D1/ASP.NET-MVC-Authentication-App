# 🛒 Application Web ASP.NET Core MVC - Gestion des Achats

Application web complète de gestion des achats développée avec **ASP.NET Core MVC**, **Entity Framework Core** et **ASP.NET Core Identity** pour l'authentification et la gestion des rôles.



---

## 📋 Table des matières

- [Fonctionnalités](#-fonctionnalités)
- [Technologies utilisées](#-technologies-utilisées)
- [Architecture](#-architecture)
- [Prérequis](#-prérequis)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Utilisation](#-utilisation)
- [Gestion des rôles](#-gestion-des-rôles)
- [Structure du projet](#-structure-du-projet)
- [Captures d'écran](#-captures-décran)
- [Auteur](#-auteur)

---

## ✨ Fonctionnalités

### Gestion des Produits
- ✅ CRUD complet (Create, Read, Update, Delete)
- ✅ Association avec Catégories et Marques
- ✅ Recherche et filtrage
- ✅ Affichage des détails produits

### Authentification & Autorisation
- 🔐 Système d'authentification complet avec ASP.NET Core Identity
- 👥 Gestion de 3 rôles : **Admin**, **Manager**, **User**
- 🔒 Autorisations granulaires par action
- 🚪 Pages de Login/Register/Logout intégrées

### Base de données
- 📊 Deux bases de données séparées :
  - `BD_VENTE_MIG` : Données métier (Produits, Catégories, Marques, Clients, Commandes)
  - `BD_VENTE_AUTH` : Authentification (Utilisateurs, Rôles)
- 🔄 Migrations Entity Framework Core (Code First & Database First)
- 🔗 Relations entre entités avec clés étrangères

---

## 🛠 Technologies utilisées

- **Framework** : ASP.NET Core 10.0 MVC
- **ORM** : Entity Framework Core 10.0
- **Base de données** : SQL Server (LocalDB)
- **Authentification** : ASP.NET Core Identity
- **Frontend** : Razor Views, Bootstrap 5
- **IDE** : Visual Studio 2022

---

## 🏗 Architecture

L'application suit le pattern **MVC (Model-View-Controller)** :

```
WebApplicationAchats/
├── Controllers/          # Logique métier et gestion des requêtes
├── Models/              # Entités et contextes de base de données
│   ├── Produit.cs
│   ├── Categorie.cs
│   ├── Marque.cs
│   ├── Client.cs
│   ├── Commande.cs
│   └── VenteContext.cs  # Contexte métier
├── Views/               # Vues Razor (UI)
│   ├── Produits/
│   ├── Categories/
│   └── Marques/
├── Data/
│   └── AuthDbContext.cs # Contexte d'authentification
├── Migrations/          # Migrations EF Core
└── wwwroot/            # Ressources statiques (CSS, JS, images)
```

---

## 📦 Prérequis

Avant de commencer, assurez-vous d'avoir installé :

- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0) (ou .NET 9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/sql-server) ou SQL Server LocalDB (inclus avec Visual Studio)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) (optionnel, pour gérer la base de données)

---

## 🚀 Installation

### 1. Cloner le repository

```bash
git clone https://github.com/Mouad-D1/ASP.NET-MVC-Authentication-App
cd ASP.NET-MVC-Authentication-App
```

### 2. Restaurer les packages NuGet

```bash
dotnet restore
```

### 3. Configurer les chaînes de connexion

Ouvrez `appsettings.json` et vérifiez/modifiez les chaînes de connexion :

```json
{
  "ConnectionStrings": {
    "VenteDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BD_VENTE_MIG;Integrated Security=True;TrustServerCertificate=True",
    "AuthDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BD_VENTE_AUTH;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

### 4. Appliquer les migrations

**Pour la base de données métier :**
```bash
dotnet ef database update --context VenteContext
```

**Pour la base d'authentification :**
```bash
dotnet ef database update --context AuthDbContext
```

### 5. Lancer l'application

```bash
dotnet run
```

Ou appuyez sur **F5** dans Visual Studio.

L'application sera accessible sur : `https://localhost:7109` (le port peut varier !)

---

## ⚙️ Configuration

### Comptes par défaut

Au premier démarrage, l'application crée automatiquement 2 comptes :

| Rôle    | Email               | Mot de passe   |
|---------|---------------------|----------------|
| Admin   | admin@emsi.ma       | Test@123       |
| Manager | manager@emsi.ma     | Manager@123    |

Les utilisateurs normaux peuvent s'inscrire via la page **Register**.

### Ajouter des rôles aux utilisateurs

Les rôles sont automatiquement créés : **Admin**, **Manager**, **User**.

Pour assigner un rôle manuellement via SQL :

```sql
USE BD_VENTE_AUTH;

DECLARE @UserId NVARCHAR(450);
DECLARE @RoleId NVARCHAR(450);

-- Remplacez par l'email de l'utilisateur et le nom du rôle
SELECT @UserId = Id FROM AspNetUsers WHERE Email = 'user@example.com';
SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'User';

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserId, @RoleId);
```

---

## 🔐 Gestion des rôles

### Permissions par rôle

| Action          | Admin | Manager | User |
|-----------------|-------|---------|------|
| Voir la liste   | ✅    | ✅      | ✅   |
| Voir les détails| ✅    | ✅      | ✅   |
| Créer           | ✅    | ❌      | ❌   |
| Modifier        | ✅    | ✅      | ❌   |
| Supprimer       | ✅    | ❌      | ❌   |

### Attributs d'autorisation

Les autorisations sont gérées via les attributs `[Authorize]` dans les contrôleurs :

```csharp
[Authorize(Roles = "Admin")]           // Admin uniquement
[Authorize(Roles = "Admin,Manager")]   // Admin et Manager
[Authorize(Roles = "Admin,Manager,User")] // Tous les utilisateurs connectés
```

---

## 📁 Structure du projet

```
WebApplicationAchats/
│
├── Controllers/
│   ├── ProduitsController.cs       # Gestion des produits
│   ├── CategoriesController.cs     # Gestion des catégories
│   ├── MarquesController.cs        # Gestion des marques
│   ├── ClientsController.cs        # Gestion des clients
│   └── CommandesController.cs      # Gestion des commandes
│
├── Models/
│   ├── Produit.cs                  # Entité Produit
│   ├── Categorie.cs                # Entité Catégorie
│   ├── Marque.cs                   # Entité Marque
│   ├── Client.cs                   # Entité Client
│   ├── Commande.cs                 # Entité Commande
│   ├── DetailCommande.cs           # Entité DetailCommande
│   └── VenteContext.cs             # Contexte EF Core (métier)
│
├── Data/
│   └── AuthDbContext.cs            # Contexte Identity (auth)
│
├── Views/
│   ├── Produits/
│   │   ├── Index.cshtml            # Liste des produits
│   │   ├── Create.cshtml           # Créer un produit
│   │   ├── Edit.cshtml             # Modifier un produit
│   │   ├── Details.cshtml          # Détails d'un produit
│   │   └── Delete.cshtml           # Supprimer un produit
│   └── Shared/
│       ├── _Layout.cshtml          # Layout principal
│       └── _LoginPartial.cshtml    # Partial view pour le login
│
├── Migrations/                     # Migrations EF Core
│   ├── (migrations VenteContext)
│   └── AuthDb/
│       └── (migrations AuthDbContext)
│
├── wwwroot/                        # Ressources statiques
│   ├── css/
│   ├── js/
│   └── lib/
│
├── appsettings.json                # Configuration (connexions DB)
├── Program.cs                      # Point d'entrée et configuration
└── README.md                       # Ce fichier
```

---



---

## 🎓 Concepts couverts

Ce projet démontre la maîtrise de :

- ✅ Architecture MVC avec ASP.NET Core
- ✅ Entity Framework Core (Code First & Database First)
- ✅ Migrations de base de données
- ✅ Relations entre entités (One-to-Many, Many-to-Many)
- ✅ ASP.NET Core Identity (authentification)
- ✅ Gestion des rôles et autorisations
- ✅ Razor Views et Tag Helpers
- ✅ Scaffolding de contrôleurs et vues
- ✅ Séparation des contextes de base de données
- ✅ Configuration et injection de dépendances

---

## 🔧 Fonctionnalités avancées

### Autorisations granulaires
Les permissions sont définies au niveau de chaque action du contrôleur, permettant un contrôle fin des accès.

### Deux bases de données séparées
- **BD_VENTE_MIG** : Contient toutes les données métier
- **BD_VENTE_AUTH** : Contient uniquement les données d'authentification

Cette séparation améliore la sécurité et la maintenabilité.

### Création automatique des rôles
Au démarrage de l'application, les rôles et les comptes admin/manager sont créés automatiquement si ils n'existent pas.

---

## 🐛 Dépannage

### Problème : "Cannot open database"
**Solution** : Vérifiez que SQL Server LocalDB est installé et que les chaînes de connexion sont correctes.

### Problème : "Login failed"
**Solution** : Assurez-vous que les migrations ont été appliquées avec `dotnet ef database update`.

### Problème : "Access Denied"
**Solution** : Vérifiez que l'utilisateur a le bon rôle assigné dans la base de données `BD_VENTE_AUTH`.

---

## 📝 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 👨‍💻 Auteur

**Mouad** - Étudiant en développement .NET

- GitHub: [@Mouad-D1](https://github.com/Mouad-D1)
- LinkedIn: [Mouad Diouane](https://www.linkedin.com/in/mouad-diouane)

---

## 🙏 Remerciements

- Mme. AIT BENNACER Fatima-Ezzahra pour l'encadrement et mon Binome Mohamed Eddih
- La communauté ASP.NET Core
---

## 📚 Ressources utiles

- [Documentation ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [Bootstrap 5](https://getbootstrap.com/docs/5.0)
- [Razor Syntax](https://docs.microsoft.com/aspnet/core/mvc/views/razor)

---

## 🚀 Évolutions futures

Améliorations possibles pour ce projet :

- [ ] Ajouter une API REST pour les opérations CRUD
- [ ] Implémenter une pagination sur la liste des produits
- [ ] Ajouter des graphiques de statistiques (Dashboard)
- [ ] Implémenter l'envoi d'emails de confirmation
- [ ] Ajouter une fonctionnalité de panier d'achat
- [ ] Intégrer un système de paiement
- [ ] Ajouter des tests unitaires et d'intégration
- [ ] Déployer sur Azure ou AWS

---

## 📞 Contact

Pour toute question ou suggestion concernant ce projet :

- Email : mouaddiouane1@gmail.com
- Issues GitHub : [Créer une issue](https://github.com/Mouad-D1/ASP.NET-MVC-Authentication-App/issues)

---

⭐ Si ce projet vous a aidé, n'hésitez pas à lui donner une étoile sur GitHub !

---

**Dernière mise à jour** : Décembre 2025
