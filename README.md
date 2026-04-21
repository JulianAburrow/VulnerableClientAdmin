# Vulnerable Client Admin (VCA)

A Blazor Server application designed to support FCA‑aligned Vulnerability Assessments with explicit workflows, predictable behaviour, and a clean, maintainable architecture.

---

## Overview

VCA provides a structured environment for advisers and administrators to record, review, and manage client vulnerability indicators. The system emphasises:

- **Clear, auditable assessment flows**
- **Explicit state and predictable UI behaviour**
- **Strong authentication and role‑based authorization**
- **A modern, consistent UI built with MudBlazor**
- **Maintainable, intention‑revealing code throughout**

Although compact in structure (**UI + DataAccess**), the application demonstrates modern Blazor Server engineering practices suitable for production use and portfolio review.

## Setting Up the Application

VCA uses a SQL Server database created entirely by a provided script.  
There are **no Entity Framework migrations** — the database and all required tables are created in one step.

---

### 1. Create the Database (Using the Script)

The repository includes a script named **CreateVCATables.sql** which:

- Creates the database  
- Creates all required tables  
- Applies relationships and constraints  
- Inserts any required seed data  

To run it:

1. Open SQL Server Management Studio (SSMS) or Azure Data Studio.
2. Connect to your SQL Server instance.
3. Open the `CreateVCATables.sql` script.
4. Execute the script.

After execution, the database will exist and be fully ready for use.

---

### 2. Configure the Connection String

In the **VulnerableClientAdminUI** project, update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=VulnerableClientAdmin;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### 3. Seeded Administrator Account

When the application is run for the first time, it automatically creates a default administrator account.  
This account is **not** created by the SQL script — it is created by the application startup logic.

**Seeded Admin Credentials:**

- **Username / Email:** `systemadmin@vca.local`
- **Password:** `Admin123!`

This account is intended for initial access to the system.  
It is recommended that you change the password after first login or replace this account entirely in production environments.

---

### 4. User Registration and Default Role Assignment

When a new user registers through the application, they are automatically assigned the basic `User` role.  
This allows newly registered users to log in and access standard functionality immediately.

This behaviour is designed for development and testing convenience and can be easily modified to require:

- Administrator approval  
- Manual role assignment  
- Restricted access until verified  

The role assignment logic is contained within the application startup and Identity configuration.

---

### 5. Running the Application

Once the database has been created and the connection string configured:

1. Restore NuGet packages.  
2. Set **VulnerableClientAdminUI** as the startup project.  
3. Run the application.  
4. Log in using the seeded admin account or register a new user.

The application will then be ready for use, providing access to vulnerability assessments, client management, and role‑based features.

---

## Summary

Vulnerable Client Admin (VCA) is a Blazor Server application designed to support FCA‑aligned vulnerability assessments with a clear, predictable, and maintainable architecture. The solution is intentionally compact, consisting of:

- **VulnerableClientAdminUI** — the Blazor Server front‑end using MudBlazor, ASP.NET Core Identity, and explicit lifecycle/layout patterns.
- **VulnerableClientAdminDataAccess** — the EF Core data layer containing the DbContext and entity models.

The database is created entirely by the included SQL script, and the application seeds an administrator account automatically on first run. New users who register are assigned a basic user role by default, though this behaviour can be easily adapted to require administrator approval or manual role assignment.

VCA demonstrates modern Blazor Server engineering practices, explicit workflows, strong authentication, and a consistent UI. It is suitable both as a functional internal tool and as a portfolio‑ready example of clean, intention‑revealing architecture.

