# Pinewood-Backend

## Overview

It's a .NET 8 application designed with Clean Architecture principles to ensure separation of concerns and maintainability. This project implements a RESTful API for managing customer data.

### Project Structure

- **API Layer**: Handles HTTP requests and responses. Contains the controllers.
- **Application Layer**: Manages business logic that includes the services.
- **Domain Layer**: Defines core business entities and interfaces.
- **Infrastructure Layer**: Manages data access that contains repositories and database context.
- **Shared Layer**: Contains the DTOs.
- **Tests Layer**: Contains unit tests using xUnit.

### Technologies Used

- **Framework**: .NET 8
- **Database**: Azure SQL Server
- **Testing**: xUnit
- **API tool**: Swagger

### Unit Testing

The project includes unit tests for controllers, services, and repositories:

- **Controller Testing**: Ensures that the API endpoints behave as expected under various conditions.
- **Service Testing**: Validates business logic and service methods, ensuring correct functionality and edge case handling.
- **Repository Testing**: Uses an in-memory database to test data access methods without relying on an external database, ensuring that repository interactions are correct and reliable.

Testing is performed using the xUnit framework, which provides a robust and flexible environment for writing and running unit tests.

### Swagger

Swagger is integrated into the project to provide interactive API.
Swagger URL: [https://pinewood-backend-dev.azurewebsites.net/swagger](https://pinewood-backend-dev.azurewebsites.net/swagger).

### Hosting Details

The project is deployed to Azure.

### Database

The project uses Azure SQL Server for its database needs. Below are the credentials that required to connect to the database from SSMS or from other IDE's.

- **Server Name**: `prince-dev-dbs.database.windows.net`
- **Database Name**: `Pinewood`
- **Login**: `prince-dev-adm`
- **Password**: `7YN$M@63TK`
