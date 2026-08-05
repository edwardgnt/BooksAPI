# 📚 Books API (Dapper, .NET 10)

![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet)
![License](https://img.shields.io/badge/License-MIT-green)
![Dapper](https://img.shields.io/badge/Dapper-ORM-orange)
![Docker](https://img.shields.io/badge/Docker-Compose-blue)
![Status](https://img.shields.io/badge/Status-Stable-brightgreen)
![Build](https://github.com/edwardgnt/BooksAPI/actions/workflows/dotnet-ci.yml/badge.svg)

Production-style REST API built with **.NET 10**, **Dapper**, **DTOs**, **Repository Pattern**, and **Filtering + Sorting + Pagination**.

This project also includes **xUnit integration tests**, **GitHub Actions CI**, and **Docker Compose support** for running the API with a containerized SQL Server database.

## 🧰 Tech Stack

- **.NET 10 Web API** — Backend framework
- **Dapper** — Lightweight data access
- **SQL Server** — Relational database
- **Docker Compose** — Local API + SQL Server orchestration
- **Repository Pattern** — Clean architecture and separation of concerns
- **DTOs** — Safe data transfer between layers
- **Dependency Injection** — Maintainable and testable services
- **ProblemDetails** — Standards-based API error responses
- **xUnit** — Automated integration testing
- **GitHub Actions** — CI pipeline for restore, build, and test

## 🚀 Features

- Thin controllers with repository abstraction behind `IBookRepository`
- DTOs: `BookCreateDto`, `BookUpdateDto`, `BookReadDto`, `BookFilterDto`
- Query params: `search`, `minPrice`, `maxPrice`, `sort`, `start`, `end`, `page`, `pageSize`
- Sorting: `price_asc`, `price_desc`, `title_asc`, `title_desc`, `year_asc`, `year_desc`, `created_asc`, `created_desc`
- Pagination wrapper: `PagedResult<T>` → `{ items, totalCount, page, pageSize }`
- Soft delete via `IsArchived`
- SQL Server container with persistent Docker volume
- Repeatable database setup script with seed data
- Integration tests using `WebApplicationFactory`

## 🧭 Example Endpoints

```http
GET /api/books
GET /api/books?search=clean&sort=price_desc
GET /api/books?minPrice=10&maxPrice=50
GET /api/books?start=2024-01-01&end=2024-12-31&page=1&pageSize=10
POST /api/books
PUT /api/books/{id}
DELETE /api/books/{id}
```

## 🏁 Getting Started

Clone the repository:

```bash
git clone https://github.com/edwardgnt/BooksAPI.git
cd BooksAPI
```

Restore and build:

```bash
dotnet restore
dotnet build
```

Run tests:

```bash
dotnet test
```

## 🐳 Running with Docker Compose

This project includes a Docker Compose setup for running the API with SQL Server.

### Services

- `api` — .NET 10 Web API container
- `sqlserver` — SQL Server 2022 Developer container
- `booksapi_sql_data` — Persistent Docker volume for SQL Server data

### 1. Create a `.env` file

Create a `.env` file in the solution root:

```env
SA_PASSWORD=YourStrongPassword123!
```

> `.env` is ignored by Git and should not be committed.

### 2. Start SQL Server

```bash
docker compose up -d sqlserver
```

Wait for SQL Server to finish starting. You can check the logs with:

```bash
docker logs booksapi-sqlserver
```

Look for a message indicating SQL Server is ready for client connections.

### 3. Load the `.env` value into your shell

```bash
set -a
source .env
set +a
```

### 4. Initialize the database

Run the database setup script:

```bash
docker exec -i booksapi-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost \
  -U sa \
  -P "$SA_PASSWORD" \
  -C \
  -i /dev/stdin < sql/init/01-create-books-db.sql
```

This creates:

- `BooksDb`
- `dbo.Books`
- Seed book records

### 5. Start the API

```bash
docker compose up --build
```

The API will be available at:

```text
http://localhost:8080
```

Example request:

```bash
curl http://localhost:8080/api/books
```

## 🗄️ Connecting with SQL Server Management Studio

To view the Docker SQL Server database from SSMS:

```text
Server: localhost,14333
Authentication: SQL Server Authentication
Login: sa
Password: your .env password
Database: BooksDb
```

Then run:

```sql
SELECT Id, Title, Author, YearPublished, CreatedAt, IsArchived, Price
FROM dbo.Books;
```

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

The project includes integration tests that validate API behavior through the ASP.NET Core test host.

## 🏗️ Architecture

The project follows a clean layered structure:

```text
Controllers
   ↓
DTOs
   ↓
IBookRepository
   ↓
BookRepository
   ↓
Dapper
   ↓
SQL Server
```

This keeps API contracts, business flow, and data access responsibilities separated and easier to maintain.

## 📌 Notes

- The app uses Dapper instead of Entity Framework Core for explicit SQL and lightweight data access.
- SQL Server runs in Docker for a repeatable local development environment.
- The API container connects to SQL Server through the Docker Compose service name: `sqlserver`.
- Local tools such as SSMS can connect through the mapped host port: `localhost,14333`.

## 📄 License

MIT
