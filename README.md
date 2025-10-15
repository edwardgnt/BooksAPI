# 📚 Books API (Dapper, .NET 9)

![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)
![License](https://img.shields.io/badge/License-MIT-green)
![Dapper](https://img.shields.io/badge/Dapper-ORM-orange)
![Status](https://img.shields.io/badge/Status-Stable-brightgreen)
![Build](https://github.com/edwardgnt/BooksAPI/actions/workflows/dotnet-ci.yml/badge.svg)

Production-style REST API with **DTOs**, **Repository Pattern**, and **Filtering + Sorting + Pagination**.
- Data access: **Dapper** (`Microsoft.Data.SqlClient`)
- Error shape: **ProblemDetails** (RFC 7807)
- Extras: **Soft delete**, **Search**, **Date-range guardrails**

## 🧰 Tech Stack

- **.NET 9 Web API** — Backend framework  
- **Dapper** — Lightweight data access  
- **SQL Server** — Database  
- **Repository Pattern** — Clean architecture & separation of concerns  
- **DTOs** — Safe data transfer between layers  
- **Dependency Injection** — For maintainable, testable code  
- **OpenAPI** — For API exploration and testing

## 🚀 Features
- Thin controllers, repository behind `IBookRepository`
- DTOs: `BookCreateDto`, `BookUpdateDto`, `BookReadDto`, `BookFilterDto`
- Query params: `search`, `minPrice`, `maxPrice`, `sort`, `start`, `end`, `page`, `pageSize`
- Sorting: `price_asc|price_desc|title_asc|title_desc|year_asc|year_desc|created_asc|created_desc`
- Pagination wrapper: `PagedResult<T>` → `{ items, totalCount, page, pageSize }`
- Soft delete via `IsArchived`

## 🧭 Example Endpoints
```http
GET /api/books
GET /api/books?search=king&sort=price_desc
GET /api/books?minPrice=10&maxPrice=50
GET /api/books?startDate=2024-01-01&endDate=2024-12-31&page=1&pageSize=10
POST /api/books
PUT /api/books/{id}
DELETE /api/books/{id}

🏁 Getting Started
git clone https://github.com/<your-username>/BooksAPIDapper.git
cd BooksAPIDapper
dotnet restore
dotnet run
# App will print e.g. https://localhost:7205


