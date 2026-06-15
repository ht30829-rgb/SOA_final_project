Anna Sweet Bakery System

Overview

Anna Sweet Bakery is a web-based backend system designed to digitalize bakery operations. It provides functionality for product management, order processing, and secure user authentication with role-based access control.

The system replaces manual order handling with a structured, secure, and scalable API-based solution.

Problem It Solves

This project addresses common bakery business challenges:

Lack of centralized product browsing system
Manual and error-prone order management
No structured user accounts or personalization
Missing secure authentication and authorization
 Technologies Used
ASP.NET Core Web API
Entity Framework Core
SQL Server
ASP.NET Core Identity
JWT Authentication (cookie-based storage)
xUnit (testing)
Moq (mocking)
C#

Project Architecture

The project follows a layered architecture:

AnnaSweetBakery.API - Backend Part of the project

├── Controllers                     → API endpoints (Account, Orders, Products, Admin)
├── Repositories                    → Data access layer (interfaces + EF Core implementation)
├── Models                          → Entities and DTOs
├── Data                            → DbContext and configuration
├── Services                        → Business logic layer
└── Program.cs                      → Dependency Injection & middleware setup

FinalProject - Frontend Part of the project

├── Controllers        
├── Repositories       
├── Models            
├── Data            
├── Services          
└── Program.cs        


Testing Project
AnnaSweetBakery.Tests
├── Controllers        → Unit tests for controllers 

Authentication & Authorization

Users authenticate using JWT tokens
Tokens are stored in HTTP-only cookies
Role-based authorization implemented:
Admin → full system access
Customer → order placement and viewing
Secure endpoints using [Authorize] and role policies

Repository Pattern

The system uses a repository pattern for data access:

Interfaces define contracts (e.g., IOrderRepository)
Implementations handle EF Core operations
Injected using ASP.NET Core Dependency Injection

Example:

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

Unit Testing
Framework: xUnit
Mocking: Moq
Scope: Controllers only
What is tested:
HTTP responses (Ok, BadRequest, NotFound)
Input validation
Authentication-related endpoints
Controller behavior in isolation
Core Features

User registration and login
JWT-based authentication
Role-based authorization
Product browsing and management
Order creation and tracking
Admin management panel (API level)

Future Improvements

Payment gateway integration (Stripe/PayPal)
Real-time order tracking (SignalR)
Frontend UI (React / Angular / Blazor)
CI/CD pipeline (GitHub Actions / Azure DevOps)
Expanded unit testing (services + repositories)

Limitations
No payment system currently implemented
No real-time updates
Testing limited to controller layer only
No deployed production version

1. Clone the repository
git clone https://github.com/ht30829-rgb/SOA_final_project.git


3. Run migrations
dotnet ef database update
4. Run the project
dotnet run

Developed by: Ema Gjeroska & Hristina Trpeska
