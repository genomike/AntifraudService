flowchart TD
    %% External Systems
    subgraph ExternalSystems["External Systems"]
        Kafka["Kafka Message Broker"]
        PostgreSQL["PostgreSQL Database"]
    end

    %% API Layer
    subgraph API["API Layer"]
        Controllers["Transaction Controllers"]
        Endpoints["HTTP Endpoints"]
    end
    
    %% Application Layer
    subgraph Application["Application Layer"]
        Commands["Commands
        - CreateTransaction
        - ValidateTransaction
        - UpdateTransactionState"]
        
        Handlers["Command Handlers"]
        ValidationService["Transaction Validation Service"]
    end
    
    %% Domain Layer
    subgraph Domain["Domain Layer"]
        Entities["Entities
        - Transaction
        - Account
        - TransferType"]
        
        ValueObjects["Value Objects
        - TransactionId
        - Money"]
        
        DomainServices["Domain Services
        - Fraud Detection Logic"]
    end
    
    %% Infrastructure Layer
    subgraph Infrastructure["Infrastructure Layer"]
        Repositories["Repositories
        - TransactionRepository"]
        
        DbContext["ApplicationDbContext"]
        MessageProducer["Transaction Event Producer"]
    end
    
    %% Flow connections
    Endpoints --> Controllers
    Controllers --> Commands
    Commands --> Handlers
    Handlers --> ValidationService
    ValidationService --> DomainServices
    DomainServices --> Entities
    Handlers --> Repositories
    Repositories --> DbContext
    DbContext --> PostgreSQL
    Handlers --> MessageProducer
    MessageProducer --> Kafka
    
    %% Transaction Flow
    Client([Client]) --"1. POST /api/transactions"--> API
    API --"2. CreateTransaction Command"--> Application
    Application --"3. Validate Transaction"--> Domain
    Domain --"4. Apply Business Rules"--> Domain
    Application --"5. Store Transaction"--> Infrastructure
    Infrastructure --"6. Save to Database"--> PostgreSQL
    Infrastructure --"7. Publish Event"--> Kafka
    API --"8. Return Response"--> Client
    
    %% Styling
    classDef green fill:#9f6,stroke:#333,stroke-width:2px;
    classDef blue fill:#69f,stroke:#333,stroke-width:2px;
    classDef yellow fill:#ff3,stroke:#333,stroke-width:2px;
    classDef red fill:#f69,stroke:#333,stroke-width:2px;
    
    class Domain green;
    class Application blue;
    class Infrastructure yellow;
    class API red;
    class ExternalSystems gray;