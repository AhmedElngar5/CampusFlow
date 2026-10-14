# 🎓 CampusFlow — School & Student Management System

A production-ready, portfolio-grade Student Management System architected with **ASP.NET Core MVC 8**, **Entity Framework Core**, **ASP.NET Core Identity**, and **Microsoft SQL Server**. Engineered to demonstrate full-stack .NET architectural principles, enterprise security patterns, and clean UX design for technical recruiters, engineering teams, and freelance clients.

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/)
[![EF Core](https://img.shields.io/badge/EF_Core-8.0.11-512BD4?style=for-the-badge&logo=nuget&logoColor=white)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
[![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5.3.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

---

## 🌟 Executive Summary

**CampusFlow** delivers an administrative portal for academic institutions. Unlike basic tutorial apps, CampusFlow implements:
- **Role-Based Access Control (RBAC)** powered by ASP.NET Core Identity with strict server-side action filters.
- **Anti-Overposting ViewModel Pattern** eliminating mass-assignment vulnerabilities.
- **Dynamic KPI Dashboard** computing statistics directly from SQL queries without in-memory bloat.
- **Multi-Field Sorting, Filtering & Pagination** executed at the database query level via `IQueryable`.
- **Zero-Secret First-Run Provisioning** with an auto-locking setup wizard that prevents subsequent takeover.

---

## ✨ Features Breakdown

### 🔐 1. Authentication & Role-Based Authorization
- **ASP.NET Core Identity Engine**: Secure PBKDF2 password hashing with cryptographically random salt, authentication cookie expiration, and sliding session timeout.
- **Dual-Tier Role Architecture**:
  - **`Admin`**: Unrestricted authority — create, edit, and delete student records, view analytics, export CSV, and provision user accounts.
  - **`Viewer`**: Auditor authority — read-only access to directory, student profile details, KPI dashboard, and CSV export. Mutation endpoints are blocked.
- **Server-Side Route Security**: Enforcement via `[Authorize]` and `[Authorize(Roles = "Admin")]`. Direct URL access (e.g., `/Students/Create`) is intercepted, returning a customized **HTTP 403 Access Denied** page.
- **Adaptive Interface**: Navigation items, action toolbars, and contextual buttons automatically reflect the active user's permissions.
- **First-Run Administrator Setup Wizard (`/Account/Setup`)**: Self-locking initialization flow that only executes when zero administrators exist in the database, eliminating hardcoded passwords.
- **Admin User Management (`/Account/Users`)**: Authorized administrators can provision accounts and assign roles directly from the interface.

### 📊 2. Executive Analytics Dashboard
- **Real-Time KPIs**: Total students, active enrollment count, graduated alumni, and academic suspensions.
- **Department Distribution**: Live breakdown of student density across academic departments with proportional progress bars.
- **Monthly Influx Metric**: Dynamic tracking of newly enrolled students registered within the active calendar month.
- **Recent Registrations**: Instant feed of the 5 most recent student admissions with one-click profile inspection.

### 📋 3. Student Record Lifecycle (CRUD)
- **Comprehensive Profiles**: Full Name, Email, Phone Number, Academic Department, Study Year (1–8), Status (`Active`, `Graduated`, `Suspended`), and Enrollment Date.
- **Server & Client-Side Validation**: jQuery Validation and Data Annotations validate constraints (regex phone verification, email formatting, string lengths).
- **Duplicate Protection**: Unique SQL index on `Email` combined with server-side pre-flight checks prevents duplicate registration.
- **Safe Destructive Actions**: Delete operations require an explicit confirmation page and execute exclusively via HTTP POST.

### 🔍 4. Search, Multi-Column Sorting & Pagination
- **Multi-Field Search**: Filters dynamically across Full Name, Email Address, and Department.
- **Compound Filters**: Filter simultaneously by Academic Department and Enrollment Status.
- **Bidirectional Column Sorting**: Sort by Name, Department, Year, Status, or Enrollment Date with clear visual indicators (`↑` / `↓`).
- **Server-Side Pagination**: Constant 8 records per page handled via SQL `OFFSET`/`FETCH` queries (`Skip()` / `Take()`).

### 📤 5. Data Export & Utilities
- **Excel-Compatible CSV Export**: Generates timestamped `.csv` files encoded with UTF-8 BOM, ensuring proper rendering of international characters and symbols in Microsoft Excel.
- **Centralized Status Code Pages**: Custom views for **404 Not Found**, **403 Forbidden**, and **500 Internal Error**.

---

## 📸 Interface Preview

| Executive Dashboard | Student Directory & Filters |
|:---:|:---:|
| ![Dashboard Placeholder](https://via.placeholder.com/600x340/17243a/ffffff?text=CampusFlow+Executive+Dashboard) | ![Students Directory Placeholder](https://via.placeholder.com/600x340/17243a/ffffff?text=Searchable+Students+Directory) |
| *Real-time KPI metrics, department distribution, and recent students* | *Search, multi-column sorting, filter dropdowns, and pagination* |

| Initial Setup Wizard | Role-Based Access Denied (403) |
|:---:|:---:|
| ![Setup Wizard Placeholder](https://via.placeholder.com/600x340/17243a/ffffff?text=Auto-Locking+Setup+Wizard) | ![Access Denied Placeholder](https://via.placeholder.com/600x340/17243a/ffffff?text=HTTP+403+Access+Denied) |
| *Zero-hardcoded-secret first-run administrator provisioning* | *Server-side route guard intercepting unauthorized actions* |

---

## 👥 Role Permissions Matrix

| Platform Capability | Unauthenticated | Viewer Role | Admin Role | Enforcement Mechanism |
|---|:---:|:---:|:---:|---|
| Access Dashboard | ❌ *Redirect to Login* | ✅ View | ✅ View | `[Authorize]` Controller Attribute |
| View Student Directory | ❌ *Redirect to Login* | ✅ View | ✅ View | `[Authorize]` Action Attribute |
| Search & Filter Records | ❌ *Redirect to Login* | ✅ Allowed | ✅ Allowed | Server-side query composition |
| Export Data to CSV | ❌ *Redirect to Login* | ✅ Allowed | ✅ Allowed | `[Authorize]` Action Attribute |
| View Student Profile | ❌ *Redirect to Login* | ✅ View | ✅ View | `[Authorize]` Action Attribute |
| Add New Student | ❌ *Redirect to Login* | ❌ *403 Forbidden* | ✅ Full Access | `[Authorize(Roles = "Admin")]` |
| Edit Student Records | ❌ *Redirect to Login* | ❌ *403 Forbidden* | ✅ Full Access | `[Authorize(Roles = "Admin")]` |
| Delete Student Record | ❌ *Redirect to Login* | ❌ *403 Forbidden* | ✅ Full Access | `[Authorize(Roles = "Admin")]` + POST |
| Manage User Accounts | ❌ *Redirect to Login* | ❌ *403 Forbidden* | ✅ Full Access | `[Authorize(Roles = "Admin")]` |
| Provision New Accounts | ❌ *Redirect to Login* | ❌ *403 Forbidden* | ✅ Full Access | `[Authorize(Roles = "Admin")]` + POST |

---

## 🛠 Technology Stack

- **Runtime & Framework**: [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) / ASP.NET Core MVC
- **Programming Language**: C# 12 (Primary constructors, nullable reference types, pattern matching)
- **Data Persistence**: Entity Framework Core 8.0.11 (Code-First Migrations)
- **Relational Database**: Microsoft SQL Server LocalDB / SQL Server Express
- **Security & Identity**: ASP.NET Core Identity 8.0.11 with cookie authentication
- **Client Presentation**: Razor Views, Bootstrap 5.3.3, Bootstrap Icons 1.11.3
- **Client Validation**: jQuery 3.7.1, jQuery Validate 1.19.5, Unobtrusive Validation 4.0.0

---

## 📁 Repository Structure

```
CampusFlow/
├── StudentManagementPortfolio/
│   ├── Controllers/
│   │   ├── AccountController.cs       # Authentication, Setup, AccessDenied, User Admin
│   │   ├── DashboardController.cs     # Real-time metrics and department aggregations
│   │   ├── HomeController.cs          # Centralized status code & error handler
│   │   └── StudentsController.cs      # Full CRUD, search, filter, sort, and CSV export
│   ├── Data/
│   │   ├── DbSeeder.cs                # Startup role seeder & config provisioning
│   │   └── SchoolDbContext.cs         # IdentityDbContext with Student DbSet & seeds
│   ├── Migrations/
│   │   ├── 20261014210503_InitialCreate.cs # Students schema & baseline data
│   │   └── 20261014214919_AddIdentity.cs   # ASP.NET Core Identity tables
│   ├── Models/
│   │   ├── AppRoles.cs                # Admin and Viewer role constants
│   │   ├── CreateUserViewModel.cs     # User provisioning model with role selection
│   │   ├── DashboardViewModel.cs      # Aggregation model for executive metrics
│   │   ├── LoginViewModel.cs          # Authentication input credentials model
│   │   ├── SetupAdminViewModel.cs     # Self-locking initial admin setup model
│   │   ├── Student.cs                 # Student entity with Data Annotations
│   │   ├── StudentListViewModel.cs    # Directory model carrying filters & sort state
│   │   ├── StudentViewModel.cs        # Anti-overposting input model for forms
│   │   └── UserItemViewModel.cs       # Account summary model for user management
│   ├── Views/
│   │   ├── Account/                   # Login, Setup, AccessDenied, Users, CreateUser
│   │   ├── Dashboard/                 # Responsive analytics overview
│   │   ├── Home/                      # Custom 404, 403, and 500 error pages
│   │   ├── Shared/                    # _Layout, validation scripts partial
│   │   └── Students/                  # Index, Details, Create, Edit, Delete, _StudentForm
│   ├── wwwroot/
│   │   ├── css/site.css               # Modern school workspace stylesheet
│   │   └── js/site.js                 # Mobile sidebar, auto-dismiss alerts, form states
│   ├── appsettings.json               # Local development configuration
│   ├── appsettings.example.json       # Safe configuration template
│   ├── Program.cs                     # DI service registrations & middleware pipeline
│   └── StudentManagementPortfolio.csproj
├── .gitignore                         # Comprehensive .NET / Visual Studio ignores
├── LICENSE                            # MIT License
└── README.md                          # Project documentation
```

---

## 🚀 Getting Started

### 1. Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (Version `8.0.x` or later)
- SQL Server LocalDB (included with Visual Studio) **or** SQL Server Express

Check your installed SDK:
```bash
dotnet --list-sdks
```

### 2. Clone the Repository
```bash
git clone https://github.com/YOUR_USERNAME/CampusFlow-StudentManagement.git
cd CampusFlow-StudentManagement/StudentManagementPortfolio
```

### 3. Restore Packages & Verify Build
```bash
dotnet restore
dotnet build
```

### 4. Database Setup & Migration
Apply existing Entity Framework Core migrations to your SQL Server instance:
```bash
dotnet ef database update
```
> **Note**: This automatically creates the database, provisions the Identity tables, and seeds 4 sample student records without manual SQL scripts.

### 5. Run the Application
```bash
dotnet run
```
Open your browser and navigate to:
```
http://localhost:5000
```

---

## 🔑 Administrator Provisioning

CampusFlow provides two safe methods for creating the root administrator account:

### Method A: Interactive Setup Wizard (Recommended)
1. On initial startup, navigate to `http://localhost:5000`.
2. The sign-in page displays an alert banner indicating that setup is required.
3. Click the banner link or navigate directly to `http://localhost:5000/Account/Setup`.
4. Enter your administrator email and password (minimum 6 characters).
5. Submit the form. The system securely hashes your credentials, assigns the `Admin` role, and automatically signs you in.
6. **Automatic Lockout**: Once created, `/Account/Setup` is permanently disabled and rejects all subsequent attempts.

### Method B: Configuration / User Secrets (CI/CD & Headless)
You can configure administrative credentials before launching without editing source code:
```bash
dotnet user-secrets init
dotnet user-secrets set "InitialAdmin:Email" "admin@school.edu"
dotnet user-secrets set "InitialAdmin:Password" "Admin@123456"

# Optional: seed a testing viewer account
dotnet user-secrets set "InitialViewer:Email" "viewer@school.edu"
dotnet user-secrets set "InitialViewer:Password" "Viewer@123456"

dotnet run
```
On startup, `DbSeeder.cs` inspects configuration, securely hashes the passwords, and creates the accounts if they do not already exist.

---

## ⚙️ Configuration & Connection Strings

The default configuration in [`appsettings.json`](file:///c:/Users/AHMED/Downloads/StudentManagementPortfolio/StudentManagementPortfolio/appsettings.json) targets SQL Server LocalDB:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StudentManagementPortfolioDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Connecting to SQL Server Express
Update `DefaultConnection` to your named instance:
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=StudentManagementPortfolioDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

### Safe Production Configuration
A safe template [`appsettings.example.json`](file:///c:/Users/AHMED/Downloads/StudentManagementPortfolio/StudentManagementPortfolio/appsettings.example.json) is provided. Never commit real production connection strings or credentials to version control.

---

## 🛡 Security Architecture Highlights

1. **Anti-Overposting (Mass-Assignment)**: Form actions bind strictly to dedicated input view models (`StudentViewModel`, `LoginViewModel`, `CreateUserViewModel`), preventing malicious property injection.
2. **Anti-Forgery Validation**: All state-modifying requests (`POST`) enforce `[ValidateAntiForgeryToken]`.
3. **Safe HTTP Verbs**: Mutations (`Delete`, `Logout`, `Create`, `Edit`) strictly require HTTP `POST` — destructive actions via `GET` are rejected.
4. **Custom Error Handling**: Handled via `app.UseStatusCodePagesWithReExecute("/Home/Error/{0}")` to prevent stack trace leakage while providing clear feedback.
5. **SQL Injection Resistance**: All database operations execute through Entity Framework Core parameterized LINQ queries.

---

## 🗺 Roadmap & Future Enhancements

- [ ] Two-Factor Authentication (2FA) via Time-Based One-Time Passwords (TOTP / Authenticator Apps)
- [ ] Student Profile Image Upload with Azure Blob Storage or local storage provider
- [ ] Academic Course & Semester Enrollment tracking with relational junction tables
- [ ] Comprehensive Audit Trail logging entity mutations with user and timestamp stamps
- [ ] Automated Unit and Integration Test suite via xUnit and Testcontainers

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for complete details.

---

## 👨‍💻 Author & Contact

Developed by **Ahmed** as a showcase project demonstrating enterprise-grade ASP.NET Core architecture, secure role-based authorization, and modern UI engineering.

- **GitHub**: [https://github.com/AhmedElngar5/CampusFlow)

- **LinkedIn**: [www.linkedin.com/in/ahmed-elngar-3251b5375)

