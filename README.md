# Books API (Dapper, .NET 9)

Production-style REST API with **DTOs**, **Repository Pattern**, and **Filtering + Sorting + Pagination**.
- Data access: **Dapper** (`Microsoft.Data.SqlClient`)
- Error shape: **ProblemDetails** (RFC 7807)
- Extras: **Soft delete**, **Search**, **Date-range guardrails**

## Features
- Thin controllers, repository behind `IBookRepository`
- DTOs: `BookCreateDto`, `BookUpdateDto`, `BookReadDto`, `BookFilterDto`
- Query params: `search`, `minPrice`, `maxPrice`, `sort`, `start`, `end`, `page`, `pageSize`
- Sorting: `price_asc|price_desc|title_asc|title_desc|year_asc|year_desc|created_asc|created_desc`
- Pagination wrapper: `PagedResult<T>` → `{ items, totalCount, page, pageSize }`
- Soft delete via `IsArchived`

## Quick Start
```bash
dotnet restore
dotnet run
# App will print e.g. https://localhost:7205
