# Clean-Architecture-Template
Quick start for .net based Clean Architecture Project

## Clean Architecture Layers

### Core Layer
- **Purpose**: Business rules that are reusable and globally applicable
- **Contains**: 
  - Entity definitions with validation rules (e.g., max length for user first name)
  - Business logic (e.g., updating student table on parent information change)
- **Benefits**: Logic is reusable across multiple projects
- **Dependencies**: None (pure business logic)

### Infrastructure Layer  
- **Purpose**: Communication with external systems
- **Responsibilities**:
  - Database access, HttpContext, external APIs, Redis, notification services
  - Implement interfaces defined in Application layer
  - Entity mapping between data models and Core entities
- **Dependencies**: Depends on Application interfaces, implements for Core entities

### Application Layer
- **Purpose**: Orchestrate use cases and define contracts
- **Contains**:
  - Use case handlers (query/command handlers)
  - Interface definitions (ports) for Infrastructure layer
  - Argument validation and business workflow orchestration
- **Dependencies**: Depends on Core entities, defines contracts for Infrastructure

### Presentation Layer
- **Purpose**: Handle user interaction and HTTP concerns
- **Responsibilities**:
  - Extract arguments from HTTP requests
  - Build query/command objects
  - Execute use cases (via MediatR or direct service calls)
  - Return appropriate HTTP status codes, error messages, and data
- **Dependencies**: Depends on Application layer only
