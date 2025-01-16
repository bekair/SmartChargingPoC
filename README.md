# Smart Charging PoC

A RESTful API implementation for managing electric vehicle charging stations, groups, and connectors with capacity management.

## Domain Model

### Group
- Unique identifier (immutable)
- Name (mutable)
- Capacity in Amps (mutable, > 0)
- Contains multiple charge stations

### Charge Station
- Unique identifier (immutable)
- Name (mutable)
- Contains 1-5 connectors
- Must belong to one group

### Connector
- Numerical identifier (1-5, unique per station)
- Max current in Amps (mutable, > 0)
- Must belong to a charge station

## Key Features

- CRUD operations for Groups, Charge Stations, and Connectors
- Automatic cascade deletion for group-station relationships
- Single station group assignment management
- Capacity validation across group hierarchy
- Current management for connectors

## Technical Stack

- ASP.NET Core 6.0
- Entity Framework Core
- SQLite Database
- Swagger/OpenAPI
- xUnit for testing
- FluentValidation

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. Clone the repository
```bash
git clone https://github.com/bekair/SmartChargingPoC.git
```

2. Navigate to the project directory
```bash
cd SmartChargingPoC
```

3. Run the application
```bash
dotnet run --project src/SmartCharging.API
```

4. Access Swagger UI
```
https://localhost:7095/swagger/index.html
```

## Project Structure
```
SmartChargingPoC/
├── src/
│   ├── SmartCharging.API/
│   ├── SmartCharging.Core/
│   └── SmartCharging.Infrastructure/
└── tests/
    └── SmartCharging.Tests/
```

## API Endpoints

### Groups
- POST /api/groups - Create group
- PUT /api/groups/{id} - Update group
- DELETE /api/groups/{id} - Delete group and stations

### Charge Stations
- POST /api/stations - Create station
- PUT /api/stations/{id} - Update station
- DELETE /api/stations/{id} - Delete station

### Connectors
- POST /api/connectors - Create connector
- PUT /api/connectors/{id} - Update connector
- DELETE /api/connectors/{id} - Delete connector

## Testing

Run the tests using:
```bash
dotnet test
```

Tests cover:
- Domain model validations
- Business rules enforcement
- Capacity management
- CRUD operations
