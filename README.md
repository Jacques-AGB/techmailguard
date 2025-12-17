# 🛡️ TechMailGuard : The Developer's Focused News Aggregator

[![.NET](https://img.shields.io/badge/.NET-8.0%20LTS-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Architecture](https://img.shields.io/badge/Architecture-Hexagonal%20%2F%20DDD-blue)](https://en.wikipedia.org/wiki/Domain-driven_design)

**TechMailGuard** est une plateforme auto-hébergée conçue pour les développeurs afin de centraliser, filtrer et gérer leur flux d'informations techniques. Ne vous laissez plus submerger par des newsletters illisibles et des flux RSS désordonnés.

Ce projet est une démonstration d'**Architecture Hexagonale** et de **Domain-Driven Design (DDD)** utilisant les dernières fonctionnalités de **..NET 8 LTS.**.

---

## 🏗️ Architecture & Concepts Clés

Le projet adhère à une architecture robuste, isolant strictement la logique métier des détails techniques.



### Principes Appliqués :
* **Domain-Driven Design (DDD)** : Logique métier riche (Agrégats, Entités, Value Objects).
* **CQRS (MediatR)** : Séparation stricte entre les Commandes (Écriture) et les Requêtes (Lecture).
* **Architecture Événementielle (EDA)** : Découplage via **RabbitMQ** pour les tâches lourdes.
* **Architecture Hexagonale** : Utilisation de "Ports" (interfaces) et d' "Adaptateurs" (implémentations).

---

## 📂 Structure du Core Domain

Le domaine est divisé en deux contextes métier clairement délimités :

### 1. Subscription Management (Newsletters)
Gère le cycle de vie des abonnements détectés dans les boîtes mail.
* **Mailbox (Aggregate Root)** : Gère le catalogue des abonnements.
* **Subscription (Entity)** : États `Active`, `PendingUnsubscribe`, `Unsubscribed`.
* **Events** : `UnsubscribeRequestedEvent`, `SubscriptionDetectedEvent`.

### 2. Veille Automation (Flux RSS/Atom)
Automatise la surveillance et la collecte des sources d'informations.
* **FeedSource (Aggregate Root)** : Gère l'état de santé et la fréquence de scan.
* **FeedItemData (Value Object)** : Conteneur immuable pour les articles extraits.

---

## 🛠️ Tech Stack

| Couche | Technologie | Rôle |
| :--- | :--- | :--- |
| **Presentation** | Blazor WebAssembly | Interface utilisateur réactive. |
| **API** | ASP.NET Core Web API | Point d'entrée et dispatching MediatR. |
| **Application** | MediatR | Orchestration des cas d'utilisation (Handlers). |
| **Worker** | .NET Worker Service | Traitement asynchrone (Consommateur RabbitMQ). |
| **Infrastructure** | EF Core / RabbitMQ | Persistance et messagerie. |
| **Domain** | **C# 12 / .NET 8 LTS** | Cœur métier : Agrégats, Entités, Value Objects et Événements. |

---

## ⚙️ Démarrage Rapide (En cours de développement)

### Prérequis
* **.NET 8.0 SDK (LTS)**
* Docker Desktop (pour RabbitMQ & PostgreSQL)

### Setup & Développement

1. **Cloner le Repository :**
    ```bash
    git clone [https://github.com/Jacques-AGB/techmailguard.git](https://github.com/Jacques-AGB/techmailguard.git)
    cd TechMailGuard

2. **Lancer les dépendances (Docker) :**

    ```bash
    docker-compose up -d

3. **Restaurer les dépendances :**

    ```Bash
    dotnet restore

4. **Lancer l'application :**

    ```Bash
    dotnet run --project TechMailGuard.API

📝 Roadmap

[x] Phase 1 : Modélisation du Domaine (Entités, Agrégats, Events)

[ ] Phase 2 : Couche Application (Commands/Queries avec MediatR)

[ ] Phase 3 : Infrastructure (Persistence EF Core & Messagerie RabbitMQ)

[ ] Phase 4 : Interface Web (Composants Blazor)

🤝 Contribution
Les contributions sont les bienvenues ! Consultez le fichier CONTRIBUTING.md pour plus de détails.

📜 Licence
Ce projet est sous licence MIT - voir le fichier LICENSE pour plus de détails.

Auteur
Jacques Kokou AGBLA (KokouInTech) - Initial Work & Architecture