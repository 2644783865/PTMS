# PTMS
PTMS is a public transport management system based on .net core web api and angular 2+

## Getting started
The application consists of two folders. 
* PTMS.Server - backend part, built on .net core 2.2 and web.api
* PTNS.Web - frontend part, built on angular 2+

### Prerequisites
* Visual Studio 2017 (updated to last version)
* .NET Core SDK
* NodeJs v10.9.0 or later
* @angular/cli - to install it run **npm install -g @angular/cli** in the console window with admin rights

### How to start up - backend

1) Open solution in VS, run build command
2) Take Projects database backup and restore it in the directory specified in */PTMS.Server/PTMS.Api/appsettings.Development.json* file.
3) Open restored database and run the following script under it
```
--Add primary key to proj_routs
ALTER TABLE PROJ_ROUTS ADD CONSTRAINT PK_PROJ_ROUTS_1 PRIMARY KEY (IDS_);

--Add efhistory table for migrations
CREATE TABLE "__EFMigrationsHistory" ( 
"MigrationId" varchar(150) NOT NULL PRIMARY KEY, 
"ProductVersion" varchar(32) NOT NULL);
```
4) In Visual Studio, navigate to Tools -> Nuget Package Manager -> Package Manager Console. In the console run **update-database** command to apply migrations

### How to start up - frontend

1) Open PTMS.Web folder in the console window with admin rights
2) Run **npm install**
3) Run **npm run start**
