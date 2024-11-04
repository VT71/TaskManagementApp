# Task Management Application

This repository contains a Task Management Application built using a microservices architecture. It includes the following components:

- **TaskService Microservice**: A .NET Core microservice for managing tasks.
- **ClientApp**: An Angular frontend application for interacting with the TaskService.
- **Tests/TaskService.UnitTests Project**: Unit tests for the TaskService.

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (version compatibility: https://v17.angular.io/guide/versions)
- [Angular CLI](https://v17.angular.io/cli) (install globally with `npm install -g @angular/cli@17`)
- A code editor such as [Visual Studio Code](https://code.visualstudio.com/)

## How to run

### TaskService Microservice

cd TaskService

dotnet run

The TaskService should start and listen on the configured port: http://localhost:5283

### ClientApp

cd ClientApp

npm install

ng serve

The application should be accessible at http://localhost:4200

### TaskService Unit Tests

cd Tests/TaskService.UnitTests

dotnet test