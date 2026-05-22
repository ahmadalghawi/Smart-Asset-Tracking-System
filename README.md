# 💻 Smart Asset Tracking System

Welcome to the **Smart Asset Tracking System**, an educational C# console-based enterprise inventory application designed to demonstrate the power of **Entity Framework Core**, **LINQ queries**, **Object-Oriented Programming (OOP)**, and global currency management.

---

## 🌟 Key Features

### 👤 Role-Based Authorization Simulation (Level 5)
* **Simulated Login**: Supports different login profiles to emulate enterprise networks:
  * **Admin** (`admin` / `1234`): Full write, delete, db-seeding, and maintenance logging rights.
  * **Manager** (`manager` / `1234`): Add and edit records, log maintenance, but blocked from deleting assets or seeding the database.
  * **Employee**: Read-only access to all dashboards and reports.
* **Role Enforcement**: Operations check active roles dynamically through a custom `CheckRole` security gateway.

### 📊 Real-Time Enterprise Analytics Dashboard
* Group and analyze inventory statistics:
  * Total global assets and employee counts.
  * Total company valuation dynamically calculated in **Swedish Krona (SEK)**.
  * Near-expiration counts (warning alerts).
  * Automatically calculates the top-utilized asset type and the most expensive corporate office location.

### 🌐 Global Currency & Multi-Office Support (Level 3)
* Context-aware currency formatting showing correct local currency symbols based on office:
  * 🇸🇪 **Sweden Office**: Swedish Krona (`kr` - SEK)
  * 🇩🇪 **Germany Office**: Euro (`€` - EUR)
  * 🇹🇷 **Turkey Office**: Turkish Lira (`₺` - TRY)
  * 🇺🇸 **USA Office**: US Dollar (`$` - USD)
* Automatically converts USD base purchase prices dynamically at run-time.

### 🧑‍💼 Employee Roster & Asset Assignments
* Connects physical assets directly to employees.
* Generates clear, structured staff assignment card blocks detailing employee metadata and their assigned hardware assets.

### 🔧 Maintenance & Lifecycle Logs
* Tracks **Last Maintenance Date**, **Next Maintenance Date**, and custom engineering notes for every device in the organization.

---

## 📂 Project Architecture

```text
smart-asset-tracking/
│
├── Smart Asset Tracking System/
│   ├── Models/
│   │   ├── Office.cs          - Manages global locations & exchange rates
│   │   ├── Employee.cs        - Represents company staff members
│   │   ├── Asset.cs           - Abstract base class with warranty & status warnings
│   │   ├── ComputerAsset.cs   - Derived class mapping Processor & RAM
│   │   └── MobileAsset.cs     - Derived class mapping SIM Cards & 5G support
│   │
│   ├── MyDbContext.cs         - Mapped EF Core database context (ConsoleEfLex1)
│   ├── DbSeeder.cs            - Populates idempotently a rich 46-record test dataset
│   ├── Program.cs             - The interactive CLI control panel & main loop
│   └── Smart Asset Tracking System.csproj
│
└── doc/
    └── Asset_Tracking_Project2.md - Original educational design brief
```

---

## 🚀 Getting Started

### Prerequisites
* **.NET 8.0 SDK / .NET 10.0 SDK** (or higher)
* **SQL Server / LocalDB** installed and running on your system
* **Visual Studio** or **VS Code**

### Database Setup & Migrations
1. Open the solution in **Visual Studio**.
2. Open the **Package Manager Console (PMC)** (`Tools` -> `NuGet Package Manager` -> `Package Manager Console`).
3. Run the following standard commands to set up the relational database and create tables via Entity Framework Core Table-Per-Hierarchy (TPH):
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

### Running the System
1. Compile and run the project:
   ```bash
   dotnet run --project "Smart Asset Tracking System"
   ```
2. Log in with the profile matching your preferred access rights.
3. Select **Option 0** in the Main Control Panel to automatically populate the SQL Server database with global offices, employees, and 34 pre-configured assets.
