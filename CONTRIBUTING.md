# Guide de Contribution - TechMailGuard 🛡️

Merci de l'intérêt que vous portez à **TechMailGuard** ! Nous sommes ravis d'accueillir des contributions pour améliorer cet outil de veille pour les développeurs.

En tant que projet basé sur le **Domain-Driven Design (DDD)** et l'**Architecture Hexagonale**, nous avons des règles strictes sur la structure du code pour garantir l'isolation de la logique métier.

---

## 🏛️ Structure du Projet

Avant de contribuer, assurez-vous de placer votre code dans la bonne couche :



1. **TechMailGuard.Domain (Le Cœur)** : 
   - Contient la logique métier pure : Agrégats, Entités, Value Objects et Événements de Domaine.
   - Définit les **Ports** (Interfaces) de services et de repositories.
   - **Règle** : AUCUNE dépendance externe (pas d'EF Core, pas de MediatR, pas d'API).

2. **TechMailGuard.Application (L'Orchestrateur)** : 
   - Contient les Commandes, Requêtes et leurs Handlers respectifs via **MediatR**.
   - Orchestre les cas d'utilisation en faisant le pont entre le Domaine et les interfaces externes.

3. **TechMailGuard.Infrastructure (Les Adaptateurs)** : 
   - Contient les implémentations techniques : Persistance (EF Core), Messagerie (RabbitMQ) et Services externes (MailKit).

4. **TechMailGuard.Worker (Le Consommateur)** : 
   - Background Service (.NET Worker) dédié au traitement asynchrone.
   - Consomme les messages de **RabbitMQ** pour exécuter les tâches lourdes déclenchées par les Domain Events (ex: désabonnement réel, scan de flux).

5. **TechMailGuard.API (L'Interface Programmable)**
   - API REST servant de passerelle entre le monde extérieur et la couche Application.
   - Responsable de l'authentification et de la validation des requêtes HTTP.

5. **TechMailGuard.Web (L'Interface Utilisateur)**

   - Client Blazor WebAssembly fournissant une expérience utilisateur interactive.
   - Consomme l'API pour afficher les données et envoyer des commandes.

---

## 🚀 Comment contribuer ?

### 1. Signaler un bug ou proposer une fonctionnalité
Utilisez les **GitHub Issues** pour discuter des changements que vous souhaitez apporter.

### 2. Flux de travail (Workflow)
1. Forkez le projet.
2. Créez une branche descriptive (`feature/ajout-source-rss` ou `fix/correction-classification`).
3. Travaillez sur vos changements.
4. **Important** : Assurez-vous que le projet compile avec `dotnet build`.
5. Soumettez une **Pull Request** (PR).

---

## 📏 Règles de Codage

* **C# 12 & .NET 8 LTS** : Utilisez les fonctionnalités stables de cette version (Primary Constructors, Collection Expressions, etc.).
* **Immuabilité** : Utilisez des `record` pour les Value Objects, les Commands et les Événements.
* **Nommage** :
  - Commandes : `[Action][Entité]Command` (ex: `CreateMailboxCommand`).
  - Handlers : `[Commande]Handler`.
* **Tests** : Toute nouvelle logique métier dans le Domaine devrait idéalement être accompagnée d'un test unitaire.

---

## 💬 Communication
Si vous avez des questions sur l'architecture ou un choix technique, n'hésitez pas à ouvrir une discussion dans l'onglet "Discussions" ou à contacter **https://www.linkedin.com/in/kokou-jacques-agbla/**.

---

## 📜 Code de Conduite
En participant à ce projet, vous acceptez de maintenir un environnement respectueux et inclusif pour tous les contributeurs.