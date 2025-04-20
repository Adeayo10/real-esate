# Real Estate Property Listing Platform

A comprehensive full-stack application for property listings and real estate management built using ASP.NET Core backend and React frontend.

## Project Overview

This platform allows users to browse property listings, register accounts, manage their profiles, and contact property agents through a modern, responsive interface. The system features user authentication, property search and filtering, and contact management.

## Features

### User Features
- User registration and authentication with JWT token-based security
- Browse property listings with search and filtering options
- Property categorization (Apartments, Townhouses, Villas)
- Contact form for inquiries
- User profile management

### Admin Features
- Property listing management (add, edit, delete)
- User management
- Contact request management

## Architecture

### Backend (ASP.NET Core 9.0)
- **Architecture**: RESTful API following clean architecture principles
- **Authentication**: JWT token-based authentication with refresh tokens
- **Database**: Microsoft SQL Server with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI

### Frontend (React)
- **Framework**: React with Vite for fast development
- **Routing**: React Router Dom for client-side navigation
- **State Management**: React Hooks for local state management
- **API Communication**: Axios for HTTP requests
- **Styling**: Custom CSS with responsive design
- **Notifications**: React-Toastify for user notifications

## Tech Stack

### Backend
- ASP.NET Core 9.0
- Entity Framework Core for ORM
- Microsoft SQL Server
- Identity Framework for user management
- JWT Authentication

### Frontend
- React 18
- Vite for build tooling
- React Router for navigation
- Axios for API calls
- React-Toastify for notifications

## Setup and Installation

### Prerequisites
- .NET 9.0 SDK
- Node.js (v18+) and npm/yarn
- SQL Server
- Visual Studio 2022 or VS Code

### Backend Setup
1. Clone the repository
2. Navigate to the backend directory: `cd backend/server-real-estate`
3. Set up your SQL Server connection string in environment variables:
   ```bash
   setx REAL_ESTATE_DB_CONNECTION_STRING "Your_Connection_String_Here"
   ```
4. Run database migrations:
   ```bash
   dotnet ef database update
   ```
5. Start the backend server:
   ```bash
   dotnet run
   ```
   The API will be available at https://localhost:5205

### Frontend Setup
1. Navigate to the frontend directory: `cd frontend/client-real-estate`
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm run dev
   ```
   The frontend will be available at http://localhost:5173

## API Endpoints

### Authentication
- `POST /api/auth/register`: Register a new user
- `POST /api/auth/login`: Log in a user
- `POST /api/auth/logout`: Log out a user
- `POST /api/auth/refresh`: Refresh JWT token

### Properties
- `GET /api/list`: Get all properties
- `GET /api/list/search`: Search properties by criteria
- `GET /api/list/{id}`: Get property by ID
- `POST /api/list`: Add a new property (admin)
- `PUT /api/list/{id}`: Update a property (admin)
- `DELETE /api/list/{id}`: Delete a property (admin)

### Contact
- `POST /api/contact`: Submit a contact request

### User
- `GET /api/user/{id}`: Get user details
- `PUT /api/user/{id}`: Update user details

## Database Schema

### Key Tables
- **Users**: User account information
- **Properties**: Property listing details
- **RefreshTokens**: JWT refresh tokens
- **ContactUs**: Customer inquiries

## Folder Structure

### Backend
```
server-real-estate/
├── Controllers/         # API controllers
├── Database/            # DbContext and entity models
├── Extensions/          # Extension methods
├── Migrations/          # Database migrations
├── Models/              # Request/response models
├── Services/            # Business logic services
└── Program.cs           # Application entry point
```

### Frontend
```
client-real-estate/
├── public/              # Static files
├── src/
│   ├── api/             # API service layers
│   ├── assets/          # Images and static assets
│   ├── Components/      # Reusable UI components
│   ├── pages/           # Page components
│   ├── utils/           # Helper utilities
│   ├── App.jsx          # Main component
│   ├── App.css          # Global styles
│   └── main.jsx         # Application entry point
```

## Future Enhancements
- Advanced property search with map integration
- Booking and scheduling system for property viewings
- Property recommendation engine
- Mobile application
- Payment integration for rentals
- Property comparison feature

## References
- ASP.NET Core (2025) *A cross-platform framework for building modern, cloud-based, internet-connected applications*. Available at: https://dotnet.microsoft.com/apps/aspnet (Accessed: 19 April 2025).  
- React (2025) *A JavaScript library for building user interfaces*. Available at: https://reactjs.org/ (Accessed: 19 April 2025).  
- Entity Framework Core (2025) *Object-relational mapper for .NET*. Available at: https://learn.microsoft.com/en-us/ef/ (Accessed: 19 April 2025).  
- React Router (2025) *Declarative routing for React*. Available at: https://reactrouter.com/ (Accessed: 19 April 2025).  
- Vite (2025) *Next Generation Frontend Tooling*. Available at: https://vitejs.dev/ (Accessed: 19 April 2025).  
- JWT Authentication (2025) *JSON Web Tokens for secure authentication*. Available at: https://jwt.io/ (Accessed: 19 April 2025).  

## Contributors
- Adeayo Kola-Adeyemi
- Nibu George
- Binsy Badarudeen
- Mohammed Javeed
- 

## License
