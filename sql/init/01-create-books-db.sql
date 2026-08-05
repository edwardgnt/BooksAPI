IF DB_ID(N'BooksDb') IS NULL
BEGIN
    CREATE DATABASE BooksDb;
END
GO

USE BooksDb;
GO

IF OBJECT_ID(N'dbo.Books', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Author NVARCHAR(200) NOT NULL,
        YearPublished INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Books_CreatedAt DEFAULT SYSUTCDATETIME(),
        IsArchived BIT NOT NULL CONSTRAINT DF_Books_IsArchived DEFAULT 0,
        Price DECIMAL(10,2) NOT NULL CONSTRAINT DF_Books_Price DEFAULT 0
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Books)
BEGIN
    INSERT INTO dbo.Books (Title, Author, YearPublished, CreatedAt, IsArchived, Price)
    VALUES
        ('Clean Code', 'Robert C. Martin', 2008, SYSUTCDATETIME(), 0, 39.99),
        ('The Pragmatic Programmer', 'Andrew Hunt and David Thomas', 1999, SYSUTCDATETIME(), 0, 42.50),
        ('Design Patterns', 'Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides', 1994, SYSUTCDATETIME(), 0, 54.99),
        ('Refactoring', 'Martin Fowler', 1999, SYSUTCDATETIME(), 0, 47.25),
        ('Domain-Driven Design', 'Eric Evans', 2003, SYSUTCDATETIME(), 0, 59.99);
END
GO