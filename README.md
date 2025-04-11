A .NET 8 Web API project that fetches cat images and breed information from TheCatAPI, stores them in a SQL Server database, and provides endpoints for searching and filtering cats based on breed temperament.
Prerequisites : .NET 8 SDK, .NET 8 SDK, Visual Studio , API Key from TheCatAPI
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Folder Structure

StealAllTheCats.sln
├── StealAllTheCats/             # API Layer (.NET Controllers, Program.cs, Swagger)
├── StealAllTheCats.Application/ # Application Layer (Services, Interfaces)
├── StealAllTheCats.DAL/         # Data Layer (DbContext, Models, DbExtension)

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

 Configure appsettings.json
 
Open StealAllTheCats/appsettings.json and locate the following section:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CatDb;Trusted_Connection=True;TrustServerCertificate=True;"
},
"CatApiKey": "YOUR_API_KEY_HERE"

Server=YOUR_SERVER_NAME: Replace this with your SQL Server instance name.
Database=CatDb: This is the name of the database that will be created automatically.
CatApiKey: Your API key from thecatapi.com, required to fetch cats with breed data. //left mine

Create DataBase 
dotnet ef database update

RunApplication
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Include a .gitignore to avoid uploading build and user-specific files.
