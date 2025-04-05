```bash
Civitas/
│
├── Civitas.Api/                 # API layer (Minimal API)
│   ├── Controllers/             # If you add traditional controllers later
│   ├── Program.cs               # Entry point with API configuration
│   ├── appsettings.json         # Configuration for development/production
│   └── SwaggerConfig.cs         # Optional: Swagger setup
│
├── Civitas.Application/         # Application layer (business logic)
│   ├── Interfaces/              # Interfaces for services/repositories
│   ├── Services/                # Application services (business logic)
│   └── DTOs/                    # Data Transfer Objects (optional)
│
├── Civitas.Domain/              # Domain layer (Entities, Aggregates, Domain logic)
│   ├── Entities/                # Domain entities (e.g., Employee, Salary)
│   ├── ValueObjects/            # Value objects (e.g., Money, Address)
│   └── Enums/                   # Enums or constants (e.g., AccessLevel)
│
├── Civitas.Infrastructure/      # Infrastructure layer (data access, external services)
│   ├── Data/                    # EF Core DbContext, migrations, repositories
│   ├── Services/                # External services like email, logging, etc.
│   └── Repositories/            # Data repositories (implementing domain interfaces)
│
├── Civitas.Tests/               # Unit tests, integration tests
│   ├── Application/             # Tests for Application layer
│   ├── Domain/                  # Tests for Domain layer
│   └── Infrastructure/          # Tests for Infrastructure layer
│
├── Civitas.sln                  # Solution file
└── README.md                    # Project overview
```