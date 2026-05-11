# ShowTracker
(Swedish Version below)

This is an ongoing project for a web-application that can be used to track TV-shows, with the goal of having a user that is able to 
rate, review, and keep track of which shows and episodes they have watched or are watching, as well as those they want to watch in the future.

<img width="1918" height="992" alt="Screenshot 2026-05-11 091145" src="https://github.com/user-attachments/assets/7fba48fb-7456-4aca-a582-bd5192b79e0e" />
<br>
<img width="1918" height="991" alt="Screenshot 2026-05-11 091200" src="https://github.com/user-attachments/assets/90cc8a3c-6d3e-48bf-8de1-0765ec85daf0" />

## Tech stack includes:
- C# / .NET 10
- ASP.NET Core
- ASP.NET Core Identity
- Entity Framework Core
- PostgreSQL
- Bootstrap 5
- TVMaze API

## Implemented features so far (Aside from the account features included in Identity):
- User registration and login with username or email.
- View that shows the first page of shows (250) available from the online TVMaze API.
- A details view with fetched information about any given show, which shows up when it is clicked.
- Adding the show to a user's personal watchlist, which is stored in a PostgreSQL database.

## Architecture:
- Built on ASP.NET core MVC and Identity
- Dependency injection throughout the project
- Entity Framework core migrations for schema management
- Local persistence of external API data

## How to run the project locally:
**Prerequisites**
  - .NET 10 SDK installed (https://dotnet.microsoft.com/download)
  - PostgreSQL 18 installed and running
  - Install the EF core tools if not already installed:
    `dotnet tool install --global dotnet-ef`
1. Clone the repository 
   ```
   git clone https://github.com/Noli0032/ShowTracker.git
   cd ShowTracker
   ```
2. Set up PostgreSQL
  - Create a database called myprojectdb
  - Create a dedicated user with access to that database
3. Configure user secrets (The connection string is set to go in user-secrets rather than in appsettings.json to avoid version control entirely)
    ```
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:ApplicationDbContextConnection" "Host=localhost;Port=5432;Database=myprojectdb;Username=your_db_username;Password=your_db_password"
    ```
4. Run migrations
    ```
    dotnet ef database update
    ```
5. Run the application
    ```
    dotnet run
    ```

## Planned future features:
  - User specific ratings for shows and episodes
  - Tracking watched shows and episodes
  - Sorting shows by categories, popularity, rating etc.
  - A home menu screen
  - Search function
  - Being able to view more pages of shows
  - Improving the UI (focus on Identity (Account Management Screen) and Watchlist screen)

---
<details>
<summary><h2>Svenska</h2></summary>

# ShowTracker

Detta är ett pågående projekt för en webb-applikation som är tänkt till att användas för att man som en användare ska kunna hålla
reda på vilka serier man sett, ska kolla på, eller har kollat på, samt att kunna ge betyg och recensioner.

<img width="1918" height="992" alt="Screenshot 2026-05-11 091145" src="https://github.com/user-attachments/assets/7fba48fb-7456-4aca-a582-bd5192b79e0e" />
<br>
<img width="1918" height="991" alt="Screenshot 2026-05-11 091200" src="https://github.com/user-attachments/assets/90cc8a3c-6d3e-48bf-8de1-0765ec85daf0" />

## Teknikstack:
- C# / .NET 10
- ASP.NET Core
- ASP.NET Core Identity
- Entity Framework Core
- PostgreSQL
- Bootstrap 5
- TVMaze API

## Implementerade funktioner hittills (förutom de användar-funktioner som ingår i Identity):
- Användarregistrering och inloggning med användarnamn eller email.
- Vy som visar den första sidan av tv-serier (250) tillgängliga från TVMaze API som finns online.
- En detalj vy som visar hämtad information om vilken serie som helst som visas på vyn om man klickar på den.
- Kan lägga till en serie till användarens personliga lista av serier som de vill kolla på, som sedan förvaras i PostgreSQL databasen.

## Arkitektur:
- Byggt med ASP.NET core MVC och Identity som bas
- Dependency injection genom hela projektet
- Entity Framework core migrations för databas-schema hantering
- Sparar data från externt API lokalt

## Hur man kör projektet lokalt:
**Förutsättningar för att kunna köra projektet**
  - .NET 10 SDK installerat (https://dotnet.microsoft.com/download)
  - PostgreSQL 18 installerat och igång
  - Installera EF core verktygen om de inte redan är installerade
    `dotnet tool install --global dotnet-ef`
1. Klona repositoryn 
   ```
   git clone https://github.com/Noli0032/ShowTracker.git
   cd ShowTracker
   ```
2. Sätt upp PostgreSQL
  - Skapa en databas och kalla den för myprojectdb
  - Skapa en dedikerad användare som har tillgång till den databasen
3. Konfigurera User Secrets/användarhemligheter ("Connection string" är konfigurerad i användarhemligheterna istället för i appsettings.json för att undvika kontakt med versionshanteringsverktyg)
    ```
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:ApplicationDbContextConnection" "Host=localhost;Port=5432;Database=myprojectdb;Username=your_db_username;Password=your_db_password"
    ```
4. Kör "migrations"
    ```
    dotnet ef database update
    ```
5. Starta applikationen
    ```
    dotnet run
    ```

## Planerade funktioner att lägga till:
  - Användarspecifika betyg för serier och episoder
  - Hålla koll på vilka serier man sett, samt episoder
  - Sortera serier via kategori, popularitet, betyg, etc.
  - En vy för hem-skärmen
  - Sökfunktion
  - Att kunna bläddra mellan fler sidor av serier
  - Förbättra gränssnittet (med fokus på Identity (användarhanteringsskärmen) och vyn för listan av serier man sett)
</details>
