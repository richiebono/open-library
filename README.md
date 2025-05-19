# LaunchQ - Take Home Project

This project is an implementation of a technical challenge to explore the OpenLibrary API and display information about authors and their books. It includes a complete CI/CD pipeline for deployment to Azure Container Apps.

## Implemented Features

1. **Books Listing Page** (`/Books`)
   - Displays detailed information about a selected author
   - Shows a list of all books written by the author
   - Includes biographical information and external links
   - Displays author photo when available
   - Features pagination for easier navigation through large book lists

2. **Book Details Page** (`/Books/{id}`)
   - Shows complete information about a specific book
   - Displays the book cover when available
   - Presents description, excerpt, publication date, and related subjects
   - Provides a link back to the book listing

## Technologies Used

- ASP.NET Core 9.0
- Blazor Server Components
- HttpClient for external API communication
- Bootstrap for styling
- GitHub Actions for CI/CD
- Terraform for infrastructure as code
- SonarQube for code quality analysis
- Docker for containerization

## Project Structure

This project follows Clean Architecture principles, with a clear separation of concerns across multiple layers:

### Domain Layer (`/src/Domain/`)
The core business logic and entities, independent of external frameworks.

- **Models**
  - `Author.cs`: Core domain entity for author information
  - `Book.cs`: Core domain entity for book details
  - `BookSummary.cs`: Simplified representation of a book
  - `Description.cs`: Domain model for content descriptions

- **Interfaces**
  - Various interfaces defining the contracts between layers

### Application Layer (`/src/Application/`)
Business rules and use cases that orchestrate the flow of data.

- **DTOs (Data Transfer Objects)**
  - `AuthorInfoDto.cs`, `BookResponseDto.cs`, etc.: Objects for data exchange between layers
  
- **Mappers**
  - `AuthorMapper.cs`: Transforms domain Author entities to/from DTOs
  - `BookMapper.cs`: Transforms domain Book entities to/from DTOs
  - `WorksMapper.cs`: Handles mapping of book collections
  - `IMapper.cs`: Common interface for all mappers

- **Services**
  - `AuthorService.cs`: Business logic for author-related operations
  - `BookService.cs`: Business logic for book-related operations

### Infrastructure Layer (`/src/Infrastructure/`)
External concerns implementation like data access and API communications.

- **Adapters**
  - `OpenLibraryAuthorAdapter.cs`: Adapter for OpenLibrary author API
  - `OpenLibraryBookAdapter.cs`: Adapter for OpenLibrary book API
  - `DescriptionJsonConverter.cs`: JSON conversion for complex description types

- **Configuration**
  - `ApiSettings.cs`: Configuration for external API connections

### Presentation Layer (`/src/Presentation/`)
User interface and external API controllers.

- **Blazor**
  - `Components/Pages`: Razor components for UI
    - `Books.razor`: Author's books listing page with pagination
    - `BookDetail.razor`: Book details page with detailed information
  - `ViewModels`: View models following MVVM pattern
  - `Shared`: Reusable UI components and layouts

### Tests

- **UnitTests**
  - Tests for individual components and business logic in isolation
  
- **IntegrationTests**
  - Tests for interactions between components and layers

### Infrastructure as Code

- **Terraform** (`/infra/terraform/`)
  - `main.tf`: Main infrastructure definition
  - `variables.tf`: Infrastructure parameters
  - `outputs.tf`: Output values from the infrastructure

- **Scripts** (`/infra/`)
  - Setup scripts for Azure resources and CI/CD configuration

## OpenLibrary API

The project uses the following OpenLibrary APIs:
- `/authors/{key}.json`: To fetch author information
- `/authors/{key}/works.json`: To list the author's books
- `/works/{key}.json`: To get details of a specific book

## Running the Project

### Local Development

1. Make sure you have .NET 9.0 SDK installed
2. Clone the repository
3. Run `dotnet run` from the `/src/Presentation/Blazor` directory
4. Access `https://localhost:5001` in your browser

### Running Tests

To run only the unit tests:

```bash
# Run using the convenience script
./run-unit-tests.sh

# Or run using the dotnet CLI directly
dotnet test src/Tests/UnitTests/LaunchQ.TakeHomeProject.UnitTests.csproj
```

To run all tests (unit and integration):

```bash
# Run all tests with summary report
./run-all-tests.sh
```

### Docker Deployment

The application can be run in Docker:

1. Make sure you have Docker and Docker Compose installed
2. From the root directory, run `docker-compose up -d`
3. Access the application at http://localhost:8080

### Terraform Infrastructure Testing

To test the Terraform infrastructure locally:

```bash
cd infra/terraform
terraform init
terraform plan
```

## CI/CD Pipeline

The project includes a complete CI/CD pipeline implemented with GitHub Actions that consists of:

1. **Build and Tests** - Builds the application and executes unit and integration tests, collecting code coverage
2. **SonarQube Analysis** - Sends coverage results to SonarQube and performs static code analysis
3. **Terraform Plan** - Plans the infrastructure to be created/updated using Terraform
4. **Build and Publish Docker** - Builds the Docker image and publishes it to Azure Container Registry
5. **Terraform Apply** - Applies the planned changes to the infrastructure in Azure
6. **Deploy Container App** - Updates the Container App in Azure with the new image

The pipeline runs automatically when:
- There is a push to the `main`, `master`, or `develop` branches
- Pull requests are created for the `main` or `master` branches
- It is triggered manually through the GitHub Actions UI

> **Note on Pipeline Implementation:** The full CI/CD pipeline requires SonarQube tokens and Azure subscription information to complete all steps. Since this is a demonstration project, these elements are configured but not fully implemented as they would require paid accounts for both SonarQube and Azure services. In a production scenario, you would need to set up all the required secrets in your GitHub repository settings to enable the complete pipeline functionality.

## Azure Architecture

The application is deployed in Azure Container Apps, with the following infrastructure:

- **Resource Group**: Contains all resources
- **Container Registry**: Stores Docker images
- **Log Analytics Workspace**: Collects application logs
- **Container App Environment**: Environment for application execution
- **Container App**: The running application

## Initial Setup for CI/CD

### 1. Configure Storage for Terraform State

Run the script to configure the Azure Storage for Terraform state:

```bash
./infra/setup-terraform-state.sh
```

### 2. Create Service Principal for CI/CD

Run the script to create the service principal:

```bash
./infra/create-service-principal.sh
```

### 3. Configure Secrets in GitHub

Configure the following secrets in your GitHub repository:

- `AZURE_CREDENTIALS`: Complete JSON of service principal credentials
- `AZURE_SUBSCRIPTION_ID`: Azure subscription ID
- `ACR_USERNAME`: Azure Container Registry username
- `ACR_PASSWORD`: Azure Container Registry password
- `TF_STATE_STORAGE_ACCOUNT_NAME`: Storage account name for Terraform state
- `TF_STATE_CONTAINER_NAME`: Container name for Terraform state
- `TF_STATE_ACCESS_KEY`: Access key for the storage account
- `SONAR_TOKEN`: Access token for SonarCloud

### 4. Configure SonarCloud

1. Create an organization and project in [SonarCloud](https://sonarcloud.io/)
2. Update the `sonar-project.properties` file with your organization and project key
3. Configure the GitHub integration with SonarCloud

## Project Development Notes

I have made significant changes to this project to demonstrate my development approach:

1. **Clean Architecture Implementation**
   - Restructured the codebase to follow Clean Architecture principles
   - Moved the original project content to src/Presentation/Blazor
   - Created clear separation between Domain, Application, Infrastructure, and Presentation layers
   - Implemented ViewModel pattern and code-behind approach in Blazor to separate UI concerns from business logic
   - Designed components with single responsibility principle in mind

2. **Testing Strategy**
   - Added unit tests to validate core business logic
   - Implemented integration tests to ensure components work together correctly
   - E2E tests were planned but not implemented due to time constraints (these would have been the next addition with more time)

3. **CI/CD Pipeline**
   - Set up a complete CI/CD pipeline using GitHub Actions
   - Integrated SonarQube for code quality analysis
   - Automated testing and deployment processes
   - The pipeline still needs some refinements but demonstrates my approach to continuous integration

4. **Infrastructure as Code**
   - Added Terraform manifests to provision Azure resources
   - Implemented infrastructure setup scripts for Azure deployment
   - Created a containerized deployment strategy with Docker

This project showcases my preferred development workflow, focusing on maintainable architecture, comprehensive testing, automated deployments, and infrastructure as code principles.

## Future Improvements

- Implement author search functionality
- Enhance API error handling
- Add filtering and sorting options for the book list
- Consider caching strategies for improved performance
- Add more comprehensive test coverage
- Complete E2E test implementation with Playwright
