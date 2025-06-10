# Product API

Sample ASP.NET Core Web API using Entity Framework Core with SQL Server.

## Features
- CRUD endpoints for products and variants
- JWT authentication
- Swagger/OpenAPI documentation
- Code first migrations

## Migrations
To create the initial migration and update the database, run:

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Make sure the connection string in `appsettings.json` points to your SQL Server instance.
