# Lead Management System

A clean ASP.NET Core 8 Lead Management System built for the Developer Technical Assessment.

## Requirements covered

- Login/logout with supplied test credentials.
- Lead creation with validation.
- Lead listing, search, filters and sorting.
- View, edit and delete.
- Follow-up history with multiple follow-ups per lead.
- Dashboard with Total Leads, New, Proposal Sent, Won, Lost and Potential Business Value.
- Status chart.
- REST APIs for leads, follow-ups and dashboard.
- EF Core database persistence.
- Duplicate lead protection using email/mobile.
- Cookie authentication and authorization.
- Swagger in Development.
- Responsive Bootstrap UI.

## Technology stack

- ASP.NET Core 8 MVC + Web API
- C#
- Entity Framework Core 8
- SQLite
- Bootstrap 5
- Chart.js
- Swagger/OpenAPI

SQLite was selected to keep local setup and deployment simple. The data-access layer is EF Core, so SQL Server/PostgreSQL can be introduced later by changing the provider and connection string.

## Architecture

Browser -> MVC/Razor UI -> Web API / Controllers -> EF Core -> SQLite

API endpoints:
- GET /api/leads
- GET /api/leads/{id}
- POST /api/leads
- PUT /api/leads/{id}
- DELETE /api/leads/{id}
- GET /api/leads/{leadId}/followups
- POST /api/leads/{leadId}/followups
- GET /api/dashboard

## Database

The application creates `leadmanagement.db` automatically on first run. Main tables:
- Users
- Leads
- FollowUps

Relationship:
- One Lead has many FollowUps.
- Deleting a Lead cascades to its FollowUps.

## Run locally

1. Install .NET 8 SDK.
2. Open this folder in Visual Studio 2022 or VS Code.
3. Run:

```bash
dotnet restore
dotnet build
dotnet run
```

4. Open the HTTPS URL shown in the terminal.
5. Login with:

Username: `admin`
Password: `Admin@123`

## GitHub upload

```bash
git init
git add .
git commit -m "Initial Lead Management System"
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/lead-management-system.git
git push -u origin main
```

Do not commit secrets, production connection strings, or local database files in a real project.

## Deployment

For a simple deployment, publish the repository to a .NET-capable hosting provider such as Azure App Service or Render. Configure the production database/connection string using the host's environment settings. Do not depend on an ephemeral local filesystem for production data if the hosting provider does not persist it.

The GitHub repository URL and live application URL are separate submission items.
