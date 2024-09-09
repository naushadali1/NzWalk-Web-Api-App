CRUD Application with ASP.NET Web API and ASP.NET MVC Integration

Welcome to the repository for the CRUD Application built with ASP.NET Web API and integrated into an ASP.NET MVC application.

 Overview

This project demonstrates a full-stack CRUD (Create, Read, Update, Delete) application leveraging the power of ASP.NET technologies. The backend is powered by ASP.NET Web API, providing a robust and scalable RESTful API for managing resources. The frontend is built using ASP.NET MVC, offering a user-friendly interface to interact with the API.

 Features

- **CRUD Operations:** Implemented create, read, update, and delete operations for managing resources.
- **ASP.NET Web API:** Provides the backend services and data management.
- **ASP.NET MVC Integration:** Connects the API with a frontend MVC application for seamless user interactions.
- **Authentication and Authorization:** Secure endpoints with JWT-based authentication.
- **Logging:** Integrated Serilog for comprehensive logging.
- **Swagger:** Provides API documentation and testing interface.

 Getting Started

To get started with this project, follow these steps:

1. Clone the Repository:
   ```bash
   git clone https://github.com/naushadali1/NzWalk-Web-Api-App.git
   ```

2. Configure Connection Strings:
   Update the `appsettings.json` file with your database connection strings for both the Web API and MVC applications.

3. Install Dependencies:
   Run `dotnet restore` to install all required packages.

4. Run Migrations:
   Apply database migrations using `dotnet ef database update` for the Web API.

5. Build and Run:
   Build and run the solution using Visual Studio or the `dotnet run` command.

6. Explore the Application:
   - Access the API endpoints through Swagger (available at `/swagger`).
   - Use the MVC application to interact with the API through a web interface.

 Contributing

Feel free to contribute to this project by submitting issues, feature requests, or pull requests. Your feedback and contributions are welcome!
