# NDIS Food Ordering API

Backend services for a food ordering platform built with C# and ASP.NET Core.
This project was originally developed during a software bootcamp and has been
continuously improved afterwards.

## Tech Stack
- C#, ASP.NET Core
- REST APIs, Swagger
- Entity Framework Core
- SQL Server
- RabbitMQ, MassTransit
- JWT Authentication

## Services
- NDIS.User.API
- (Order / Payment / Notification services in progress)

## Configuration
Secrets are **not** committed to the repository.

1. Copy `NDIS.User.API/appsettings.Development.example.json`
2. Rename it to `NDIS.User.API/appsettings.Development.json`
3. Replace placeholder values with your own settings

## Run with Docker (Recommended)
```bash
docker compose up --build
