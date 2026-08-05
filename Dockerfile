FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY BooksAPIDapper.sln ./
COPY BooksAPIDapper/BooksAPIDapper.csproj BooksAPIDapper/
COPY BooksAPIDapper.Tests/BooksAPIDapper.Tests.csproj BooksAPIDapper.Tests/

RUN dotnet restore

COPY . .

RUN dotnet publish BooksAPIDapper/BooksAPIDapper.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BooksAPIDapper.dll"]