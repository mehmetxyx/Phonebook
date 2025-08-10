# 📞 Phonebook Microservices Project

A containerized phonebook application built with .NET 9, EFCore, PostgreSQL, RabbitMQ, MassTransit, and Docker Compose — designed to demonstrate Clean Architecture within a microservices ecosystem. 
Each service is independently deployable, testable, and structured for long-term maintainability and developer-friendly onboarding.

---

## 🧱 Architecture Overview

This project follows a **Clean Architecture** approach within a **microservices architecture**, where each microservice encapsulates both synchronous API functionality and asynchronous background processing:

- **Contact Management Microservice**  
  - `ContactManagement.Contact.Api`: Handles CRUD operations for contacts and their details  
  - `ContactManagement.Contact.Messaging.Service`: Listens to `RabbitMQ` events by using `MassTransit` message bus, and processes events.
  - Follows Clean Architecture.

- **Report Management Microservice**  
  - `Report.Api`: Manages report generation and status tracking  
  - `Report.Messaging.Service`: Listens to `RabbitMQ` events by using `MassTransit` message bus, and processes events.
  - Follows Clean Architecture.

- **Frontend Web Client**  
  - `Phonebook.Web:` A lightweight interface for interacting with the system

- **Shared Infrastructure**  
  Includes messaging abstractions, event contracts, and common utilities used across services

### General Architecture Diagram:
 you can see the general workflow of the system in the diagram below:

![Architecture Overview](https://raw.githubusercontent.com/mehmetxyx/Phonebook/development/Docs/ArchitectureDiagram.png)

---
## 📦 Technologies Used

- **.NET 9** — backend services
- **EF Core** — ORM for PostgreSQL
- **RabbitMQ + MassTransit** — asynchronous messaging
- **Docker Compose** — container orchestration
- **PostgreSQL** — relational database
- **xUnit + NSubstitute + AutoFixture** — unit testing
- **coverlet.collector + ReportGenerator** — code coverage reporting

---


## 🧱 Solution Structure
```
Phonebook/
├── Clients/
│   └── Phonebook.Web/                           # Web frontend
├── Deployments/
│   └── docker-compose.yml                       # Deployment configuration
├── Docs/
│   ├── ArchitectureDiagram.png                  # System architecture diagram
│   └── CodeCoverageReport.pdf                   # Test coverage report
├── Services/
│   ├── ContactManagement/
│   │   ├── ContactManagement.Api/               # REST API for contacts
│   │   ├── ContactManagement.Messaging.Service/ # RabbitMQ consumer
│   │   └── ...                                  # Domain, Application, Infrastructure
│   └── ReportManagement/
│       ├── ReportManagement.Api/                # REST API for reports
│       ├── ReportManagement.Messaging.Service/  # RabbitMQ consumer
│       └── ...                                  # Domain, Application, Infrastructure
├── Shared/
│   └── ...                                      # Shared contracts, messaging, common utilities
```

---
## 🚀 Running the Project

### 🛠 Requirements

- Docker or Docker Desktop installed computer

### ▶️ Quick Start

1. Clone the repository
   ```bash
   git clone git@github.com:mehmetxyx/Phonebook.git
   ```
   ```bash
   cd Phonebook/Deployments
   ```

2. Build and run the solution
   ```bash
   docker-compose up
   ```
  
   With this command:
   - Docker images will be pulled for RabbitMQ, PostgreSQL, Dotnet SDK, Dotnet Runtime, and Dotnet AspNet.
   - phonebook.postgres and phonebook.rabbitmq servers will be started as containers.
   - Docker images will be built and started for each service:
	 - ContactManagement.Api
	 - ContactManagement.Messaging.Service
	 - ReportManagement.Api
	 - ReportManagement.Messaging.Service
	 - Phonebook.Web
   
   >!Depending on your internet speed and whether you have docker images locally it will took between 3 to 5 minutes to run

4. In another terminal, you can check whether the containers are ready or not by running:
   ```bash
   docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | grep 'phonebook\.' | column -t -s $'\t'
   ```

	```bash
	phonebook.phonebook.web                 Up 24 minutes   0.0.0.0:53000->8080/tcp
	phonebook.reportmanagement.api          Up 24 minutes   0.0.0.0:50502->8080/tcp
	phonebook.reportmanagement.messaging    Up 24 minutes
	phonebook.contactmanagement.messaging   Up 24 minutes
	phonebook.contactmanagement.api         Up 24 minutes   0.0.0.0:50501->8080/tcp
	phonebook.postgres                      Up 24 minutes   0.0.0.0:55432->5432/tcp
	phonebook.rabbitmq                      Up 24 minutes   4369/tcp, 5671/tcp, 15671/tcp, 15691-15692/tcp, 25672/tcp, 0.0.0.0:55672->5672/tcp, 0.0.0.0:15673->15672/tcp
	```
  > If you see `Up` status for all services, everything is running smoothly.

5. When the containers are ready, you can access the services at the following URLs:
   - Phonebook Web: http://localhost:53000
   - Contact API: http://localhost:50501/api/contacts
   - Report API: http://localhost:50502/api/reports
   - RabbitMQ UI: http://localhost:15673 (guest/guest)
---
## 🖥️ Using the Phonebook Web UI

Once the app is running, visit [http://localhost:53000](http://localhost:53000) to access the Phonebook Web interface.

### Available Features:
- **Add Contact**:
    <p>Enter contact name, surname and company</p>
	<p>Click "Add Contact" to save</p>

- **Add Contact Detail**: 
    <p>Select contact for adding details<p>
	<p>Select detail type such as PhoneNumber, Email, or Location</p>
	<p>Enter detail value and click "Add Detail"</p>
	<p>Details will be saved and displayed in the contact's details list</p>

- **Generate Report**:
    <p>Click "Requet Location Report" to create report request</p>
	<p>Report will be added to the queue for processing and shown in the list</p>
	<p>Report will be generated asynchronously</p>
	<p>Once completed, report status will be updated to `Completed`</p>

- **View Reports**:
	<p>Click to `Refresh Reports` button to see update the report list and their status</p>
 	<p>Click "View Report" to see the report details</p>

---
## 🧪 API Testing

You can test the APIs directly using tools like Postman or curl.

### Contact Management API
```
  GET     /api/contacts
  POST    /api/contacts
  GET     /api/contacts/{{contactid}}
  DELETE  /api/contacts/{{contactid}}

  GET    /api/contacts/{{contactid}}/{{contactid}}/details
  POST   /api/contacts/{{contactid}}/{{contactid}}/details
  DELETE /api/contacts/{{contactid}}/{{contactid}}/details/{{contact.details.id}}
```
### Report Management API
```
  POST /api/reports
  GET  /api/reports
  GET  /api/reports/{{report.id}}
  
  GET  /api/reports/{{report.id}}/data
```
> All endpoints are hosted locally:
- Contact Management API: [http://localhost:50501](http://localhost:50501)
- Report Management API: [http://localhost:50502](http://localhost:50502)


---
## 📊 Test Coverage Summary

This project includes comprehensive unit tests across all services. 
Below is the latest coverage snapshot:

| Metric             | Value                        |
|--------------------|------------------------------|
| **Line Coverage**  | 98.7% (841/852 lines)        |
| **Branch Coverage**| 82.9% (68/82 branches)       |
| **Assemblies**     | 14                           |
| **Classes**        | 57                           |
| **Files Analyzed** | 57                           |
| **Coverage Date**  | August 9, 2025 — 4:50 PM to 7:06 PM |

> Coverage generated from 30 Cobertura reports using MultiReport parser.

The Code coverage report generated with following command: 

```bash
dotnet test --collect:"XPlat Code Coverage" && \
reportgenerator -reports:"**/coverage.cobertura.xml" \
  -targetdir:"coverage-report" \
  -reporttypes:Html \
  -classfilters:"-*Migrations*" \
  -filefilters:"-*ServiceExtensions.cs;-*Program.cs" && \
start coverage-report/index.html
```

To ensure meaningful metrics and avoid noise from boilerplate or auto-generated code, the following were excluded from the coverage report:

- `Migrations/*` (via `-classfilters`)
- `Program.cs` (via `-filefilters`)
- `*ServiceExtensions.cs` (via `-filefilters`)

---
## 🚧 Future Improvements
- Add more integration tests
- Add metrics, tracing, and logging with OpenTelemetry
- Add prometheues, grafana, loki and jaeger for monitoring
- Add health checks for services
- Improve frontend styling and UX
- Add CI pipeline for automated testing and coverage reporting
- Implement authentication and authorization
