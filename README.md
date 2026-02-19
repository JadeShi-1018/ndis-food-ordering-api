# NDIS Food Ordering API (Microservices)

A microservices-based backend for an NDIS-style food ordering platform:
- Users can register/login and manage profiles
- Providers can publish fixed menus / time slots
- Users place orders and payments are processed
- Events are published via RabbitMQ and handled by Notification/Rebate services

## Solution Overview

This repository contains multiple .NET APIs and a shared class library:
- **NDIS.User.API** – Identity/JWT, user profile, auth
- **NDISS.Service.API** – Provider/service side: menu items, slots, etc.
- **NDIS.Order.API** – Orders (create/order lifecycle)
- **NDIS.Payment.API** – Payments (persist payment records, future integration)
- **NDISS.NotificationService.API** – Consumes events (RabbitMQ) and sends notifications (email etc.)
- **NDISS.UserRebate.API** – User rebate/discount logic (calls other APIs, persists rebate records)
- **NDIS.Shared** (ClassLibrary) – shared DTOs, middlewares, common helpers

> Current docker-compose spins up SQL Server + RabbitMQ + User API. Other services can be added incrementally.

---

## Tech Stack

- .NET (multi-project solution)
- SQL Server (Docker)
- RabbitMQ (Docker)
- Swagger/OpenAPI
- (Some services) Serilog, FluentValidation, MassTransit

---

## Prerequisites

- Docker Desktop
- .NET SDK (match repo target frameworks)
- (Optional) Visual Studio / Rider

---

## Quick Start (Docker)

### 1) Create `.env`

Copy from example:

```bash
cp .env.example .env
