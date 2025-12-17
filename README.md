# Gym Management System

A comprehensive ASP.NET Core MVC web application for managing fitness centers with role-based authentication, appointment booking, and AI-powered workout planning.

## 🎯 Features

### Core Functionality
- ✅ **Complete CRUD Operations** for Gyms, Trainers, Services, and Availability
- ✅ **Role-Based Authentication** (Admin & Member roles)
- ✅ **Appointment Booking System** with overlap prevention
- ✅ **REST API** with LINQ queries
- ✅ **AI Workout Planner** using OpenAI API
- ✅ **Responsive Bootstrap 5 UI**

### Admin Features
- Manage gyms, trainers, services, and availability schedules
- View and manage all appointments
- Confirm or cancel appointments
- Dashboard with statistics

### Member Features
- Browse trainers and services
- Book appointments with real-time availability checking
- View and manage personal appointments
- Generate AI-powered personalized workout plans

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server with Entity Framework Core (Code First)
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Bootstrap 5, jQuery, HTML5, CSS3
- **API**: REST API with LINQ queries
- **AI Integration**: OpenAI API

## 📋 Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code (optional)
- OpenAI API Key (for AI workout planner feature)

## 🚀 Installation & Setup

### 1. Clone or Download the Project

```bash
cd /Users/imadabda/Documents/gym/GymManagementSystem
```

### 2. Configure Database Connection

Edit `appsettings.json` and update the connection string if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GymManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Configure OpenAI API Key (Optional)

Add your OpenAI API key in `appsettings.json`:

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  }
}
```

**Note**: The application will work without an API key, but the AI workout planner will show a fallback plan.

### 4. Restore NuGet Packages

```bash
dotnet restore
```

### 5. Apply Database Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 6. Run the Application

```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## 👤 Default Credentials

The database is seeded with default accounts:

### Admin Account
- **Email**: admin@gym.com
- **Password**: Admin@123

### Member Account
- **Email**: member@gym.com
- **Password**: Member@123

## 📚 API Endpoints

### Trainers API

- `GET /api/trainersapi` - Get all trainers
- `GET /api/trainersapi/{id}` - Get specific trainer
- `GET /api/trainersapi/available?date=YYYY-MM-DD` - Get available trainers for a date

### Appointments API

- `GET /api/appointmentsapi` - Get all appointments
- `GET /api/appointmentsapi/member/{memberId}` - Get member's appointments
- `GET /api/appointmentsapi/trainer/{trainerId}` - Get trainer's appointments
- `GET /api/appointmentsapi/statistics` - Get appointment statistics

### Example API Call

```bash
curl https://localhost:5001/api/trainersapi/available?date=2025-12-20
```

## 🗂️ Project Structure

```
GymManagementSystem/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── HomeController.cs
│   ├── GymsController.cs
│   ├── TrainersController.cs
│   ├── ServicesController.cs
│   ├── TrainerAvailabilityController.cs
│   ├── AppointmentsController.cs
│   └── WorkoutPlannerController.cs
├── API/                 # REST API Controllers
│   ├── TrainersApiController.cs
│   └── AppointmentsApiController.cs
├── Models/              # Entity Models & ViewModels
│   ├── ApplicationUser.cs
│   ├── Gym.cs
│   ├── Trainer.cs
│   ├── Service.cs
│   ├── TrainerService.cs
│   ├── TrainerAvailability.cs
│   ├── Appointment.cs
│   └── ViewModels/
├── Views/               # Razor Views
├── Data/                # DbContext & Migrations
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── Services/            # Business Logic Services
│   ├── AppointmentService.cs
│   └── OpenAIService.cs
└── wwwroot/             # Static Files
```

## 🔑 Key Features Explained

### 1. Appointment Booking with Overlap Prevention

The system uses LINQ queries to:
- Check trainer availability based on day and time
- Detect overlapping appointments
- Generate available time slots dynamically

### 2. REST API with LINQ

All API endpoints use LINQ for:
- Filtering data (`Where`, `Any`)
- Selecting specific fields (`Select`)
- Including related data (`Include`, `ThenInclude`)
- Ordering results (`OrderBy`, `OrderByDescending`)

### 3. AI Workout Planner

- Collects user data (height, weight, fitness goal)
- Sends request to OpenAI API
- Returns personalized workout plan
- Includes fallback plan if API is unavailable

## 🧪 Testing the Application

### Test Appointment Booking
1. Login as member (member@gym.com)
2. Navigate to "Book Appointment"
3. Select trainer, service, date, and time
4. System validates availability and prevents overlaps

### Test API Endpoints
1. Use browser or Postman
2. Navigate to `/api/trainersapi`
3. Test filtering with `/api/trainersapi/available?date=2025-12-20`

### Test AI Feature
1. Login as any user
2. Navigate to "AI Workout Planner"
3. Enter height, weight, and fitness goal
4. Generate personalized plan

## 📝 Database Schema

### Main Entities
- **Gym**: Fitness center locations
- **Trainer**: Trainers with specializations
- **Service**: Services offered (duration, price)
- **TrainerService**: Many-to-many relationship
- **TrainerAvailability**: Trainer schedules
- **ApplicationUser**: Extended Identity user
- **Appointment**: Booking records

### Relationships
- Gym → Trainers (One-to-Many)
- Gym → Services (One-to-Many)
- Trainer ↔ Service (Many-to-Many via TrainerService)
- Trainer → TrainerAvailability (One-to-Many)
- Trainer → Appointments (One-to-Many)
- Member → Appointments (One-to-Many)

## 🎓 Academic Notes

This project demonstrates:
- **Clean Architecture**: Separation of concerns (Models, Views, Controllers, Services)
- **LINQ Queries**: Extensive use throughout the application
- **Entity Framework Core**: Code-First approach with migrations
- **ASP.NET Identity**: Role-based authorization
- **REST API**: RESTful design principles
- **Validation**: Both client-side and server-side
- **AI Integration**: Modern AI API usage

## 🐛 Troubleshooting

### Database Connection Issues
- Ensure SQL Server is running
- Check connection string in `appsettings.json`
- Run `dotnet ef database update`

### Migration Errors
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### OpenAI API Errors
- Verify API key is correct
- Check internet connection
- Application will use fallback plan if API fails

## 📄 License

This project is created for academic purposes.

## 👨‍💻 Author

Created as a comprehensive gym management system demonstration.

---

**Note**: Remember to keep your OpenAI API key secure and never commit it to version control!
