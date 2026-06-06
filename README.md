# EmployeeAPI

## Overview

EmployeeAPI is a RESTful Web API built using ASP.NET Core and Entity Framework Core for managing employee records.

## Features

* Employee CRUD Operations
* JWT Authentication & Authorization
* Repository Pattern
* Dependency Injection
* DTOs
* Global Exception Middleware
* Custom Filters
* API Versioning
* Response Caching
* Logging with ILogger
* Entity Framework Core
* SQL Server Integration

## Tech Stack

* ASP.NET Core Web API
* C#
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger / OpenAPI
* Git & GitHub

## Project Structure

Controller
→ Service
→ Repository
→ DbContext
→ SQL Server

## API Endpoints

### Employee

* GET /api/employee
* GET /api/employee/{id}
* POST /api/employee
* PUT /api/employee/{id}
* DELETE /api/employee/{id}

### Authentication

* POST /api/auth/register
* POST /api/auth/login

## Concepts Implemented

* Dependency Injection
* Repository Pattern
* DTO Mapping
* Middleware
* Filters
* Authentication & Authorization
* Response Caching
* API Versioning
* Logging

## Getting Started

1. Clone the repository
2. Update the connection string in appsettings.json
3. Run database migrations
4. Start the application
5. Open Swagger UI

## Author

Lohith Bonumaddi
