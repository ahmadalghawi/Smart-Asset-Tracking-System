using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ConsoleEfLex1;
using Smart_Asset_Tracking_System.Models;

namespace Smart_Asset_Tracking_System
{
    class Program
    {
        private static string currentUserRole = "Employee";

        static void Main(string[] args)
        {
            // Set console output encoding to support global currency symbols (kr, €, ₺)
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("==========================================================");
            Console.WriteLine("        SMART ASSET TRACKING SYSTEM (LEARNING PORTAL)     ");
            Console.WriteLine("==========================================================");
            Console.WriteLine("NOTE: Database is managed via Visual Studio Package Manager:");
            Console.WriteLine("      Run 'Add-Migration InitialCreate' and 'Update-Database' first.");
            Console.WriteLine("==========================================================\n");

            // Simulation of Login System
            SimulateLogin();

            // Simple Menu Loop
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine($"\n--- MAIN CONTROL PANEL [Logged in as: {currentUserRole}] ---");
                Console.WriteLine("0. Populate Demo Data (Offices, Employees, Assets) [Admin]");
                Console.WriteLine("1. Add Office Location [Admin/Manager]");
                Console.WriteLine("2. Add Employee Roster Card [Admin/Manager]");
                Console.WriteLine("3. Add New Asset [Admin/Manager]");
                Console.WriteLine("4. Show All Assets [All Roles]");
                Console.WriteLine("5. Update Asset Records [Admin/Manager]");
                Console.WriteLine("6. Delete Asset from Database [Admin]");
                Console.WriteLine("7. Search & Filter Assets [All Roles]");
                Console.WriteLine("8. Export Asset Ledger to CSV / TXT / JSON [All Roles]");
                Console.WriteLine("9. Generate Reporting Dashboard [All Roles]");
                Console.WriteLine("10. View Enterprise Dashboard Statistics [All Roles]");
                Console.WriteLine("11. View Employee Assets & Assignment Cards [All Roles]");
                Console.WriteLine("12. Manage Asset Maintenance Logs [All Roles/Write: Admin/Manager]");
                Console.WriteLine("13. Switch User Role / Relog");
                Console.WriteLine("14. Exit");
                Console.WriteLine("--------------------------");
                Console.Write("Select an option (0-14): ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "0":
                        if (CheckRole("Admin"))
                        {
                            using (var context = new MyDbContext())
                            {
                                DbSeeder.Seed(context);
                            }
                        }
                        break;
                    case "1":
                        if (CheckRole("Admin", "Manager"))
                        {
                            AddOffice();
                        }
                        break;
                    case "2":
                        if (CheckRole("Admin", "Manager"))
                        {
                            AddEmployee();
                        }
                        break;
                    case "3":
                        if (CheckRole("Admin", "Manager"))
                        {
                            AddAsset();
                        }
                        break;
                    case "4":
                        ShowAllAssets();
                        break;
                    case "5":
                        if (CheckRole("Admin", "Manager"))
                        {
                            UpdateAsset();
                        }
                        break;
                    case "6":
                        if (CheckRole("Admin"))
                        {
                            DeleteAsset();
                        }
                        break;
                    case "7":
                        SearchAndFilterAssets();
                        break;
                    case "8":
                        ExportAssets();
                        break;
                    case "9":
                        GenerateReportsDashboard();
                        break;
                    case "10":
                        ShowEnterpriseDashboard();
                        break;
                    case "11":
                        ShowEmployeeAssignments();
                        break;
                    case "12":
                        ManageMaintenanceLogs();
                        break;
                    case "13":
                        SimulateLogin();
                        break;
                    case "14":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        #region 1. Add Office (Level 3)
        static void AddOffice()
        {
            Console.WriteLine("\n--- REGISTER COMPANY OFFICE ---");
            Console.Write("Enter Office Name (e.g. Sweden Office): ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter Country Name: ");
            string country = Console.ReadLine() ?? "";

            Console.Write("Enter Currency Code (e.g. SEK, EUR, TRY, USD): ");
            string currency = Console.ReadLine() ?? "";

            Console.Write("Enter Exchange Rate (1 USD = X Local Currency): ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
            {
                rate = 1.0m; // Default to 1.0 if input is invalid
            }

            // Create context, add to DB, and save
            using (var context = new MyDbContext())
            {
                var office = new Office
                {
                    Name = name,
                    Country = country,
                    Currency = currency,
                    ExchangeRateToUsd = rate
                };

                context.Offices.Add(office); // Prepare INSERT command
                context.SaveChanges();      // Execute command in SQL Server

                Console.WriteLine($"✔ Office '{name}' successfully saved to database!");
            }
        }
        #endregion

        #region 2. Add Employee (Level 5)
        static void AddEmployee()
        {
            Console.WriteLine("\n--- REGISTER EMPLOYEE ROSTER CARD ---");
            Console.Write("Enter Employee Full Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter Department: ");
            string dept = Console.ReadLine() ?? "";

            Console.Write("Enter Corporate Email: ");
            string email = Console.ReadLine() ?? "";

            using (var context = new MyDbContext())
            {
                var emp = new Employee
                {
                    FullName = name,
                    Department = dept,
                    Email = email
                };

                context.Employees.Add(emp);
                context.SaveChanges();

                Console.WriteLine($"✔ Employee '{name}' registered successfully!");
            }
        }
        #endregion

        #region 3. Add Asset (CRUD - Create)
        static void AddAsset()
        {
            using (var context = new MyDbContext())
            {
                // We need at least one office created first to link the asset
                var offices = context.Offices.ToList();
                if (offices.Count == 0)
                {
                    Console.WriteLine("❌ Error: No offices exist. Please register an office first (Option 1).");
                    return;
                }

                Console.WriteLine("\n--- REGISTER NEW PHYSICAL ASSET ---");
                Console.WriteLine("Select Asset Category:");
                Console.WriteLine("1. Computer Asset (Laptop / Desktop)");
                Console.WriteLine("2. Mobile Asset (Phone / Tablet)");
                Console.Write("Choice (1-2): ");
                string categoryChoice = Console.ReadLine() ?? "";

                if (categoryChoice != "1" && categoryChoice != "2")
                {
                    Console.WriteLine("Invalid selection. Aborting registration.");
                    return;
                }

                // Gather general parameters
                Console.Write("Enter Specific Type (e.g. Laptop, Phone, Tablet): ");
                string type = Console.ReadLine() ?? "";

                Console.Write("Enter Brand (e.g. Lenovo, Apple, Samsung): ");
                string brand = Console.ReadLine() ?? "";

                Console.Write("Enter Model Name (e.g. ThinkPad X1, iPhone 15): ");
                string model = Console.ReadLine() ?? "";

                Console.Write("Enter Purchase Date (YYYY-MM-DD): ");
                DateTime purchaseDate;
                if (!DateTime.TryParse(Console.ReadLine(), out purchaseDate))
                {
                    purchaseDate = DateTime.Today; // default to today if failed
                }

                Console.Write("Enter Purchase Price (in USD): ");
                decimal priceUsd;
                decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out priceUsd);

                Console.Write("Enter Serial Number / Asset Tag: ");
                string serial = Console.ReadLine() ?? "";

                // Select Office Hub via numerical input
                Console.WriteLine("\nSelect Corporate Office Location:");
                for (int i = 0; i < offices.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {offices[i].Name} ({offices[i].Country})");
                }
                Console.Write("Select Office index: ");
                int officeIndex;
                int.TryParse(Console.ReadLine(), out officeIndex);
                if (officeIndex < 1 || officeIndex > offices.Count)
                {
                    Console.WriteLine("Invalid office. Registration aborted.");
                    return;
                }
                var selectedOffice = offices[officeIndex - 1];

                // Select Employee Assignment (Optional)
                var employees = context.Employees.ToList();
                int? employeeId = null;
                if (employees.Count > 0)
                {
                    Console.Write("\nAssign this hardware asset to an employee? (y/n): ");
                    string assignInput = Console.ReadLine()?.Trim().ToLower() ?? "";
                    if (assignInput == "y" || assignInput == "yes")
                    {
                        Console.WriteLine("Select Employee:");
                        Console.WriteLine("0. Remain Unassigned");
                        for (int i = 0; i < employees.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {employees[i].FullName} ({employees[i].Department})");
                        }
                        Console.Write("Select Employee index: ");
                        int empIndex;
                        int.TryParse(Console.ReadLine(), out empIndex);
                        if (empIndex > 0 && empIndex <= employees.Count)
                        {
                            employeeId = employees[empIndex - 1].Id;
                        }
                    }
                }

                Asset newAsset;

                // 1. Instantiating concrete derived subclasses (TPH Inheritance)
                if (categoryChoice == "1") // ComputerAsset
                {
                    Console.Write("Enter CPU/Processor (e.g. Intel i7, Ryzen 5): ");
                    string cpu = Console.ReadLine() ?? "";
                    Console.Write("Enter RAM Size in GB: ");
                    int ram;
                    int.TryParse(Console.ReadLine(), out ram);

                    newAsset = new ComputerAsset
                    {
                        AssetType = type,
                        Brand = brand,
                        ModelName = model,
                        PurchaseDate = purchaseDate,
                        PurchasePriceUsd = priceUsd,
                        SerialNumber = serial,
                        OfficeId = selectedOffice.Id,
                        EmployeeId = employeeId,
                        ProcessorType = cpu,
                        RamSizeGb = ram
                    };
                }
                else // MobileAsset
                {
                    Console.Write("Enter SIM Card Number (or E-SIM): ");
                    string sim = Console.ReadLine() ?? "";
                    Console.Write("Does this support 5G? (y/n): ");
                    string input5g = Console.ReadLine()?.Trim().ToLower() ?? "";
                    bool has5g = input5g == "y" || input5g == "yes";

                    newAsset = new MobileAsset
                    {
                        AssetType = type,
                        Brand = brand,
                        ModelName = model,
                        PurchaseDate = purchaseDate,
                        PurchasePriceUsd = priceUsd,
                        SerialNumber = serial,
                        OfficeId = selectedOffice.Id,
                        EmployeeId = employeeId,
                        SimCardNumber = sim,
                        Is5gEnabled = has5g
                    };
                }

                // Add to Context and call EF SaveChanges to commit transaction
                context.Assets.Add(newAsset);
                context.SaveChanges();

                Console.WriteLine($"✔ Asset '{brand} {model}' successfully registered!");
            }
        }
        #endregion

        #region 4. Show All Assets (CRUD - Read, Sorting & Converted Prices)
        static void ShowAllAssets()
        {
            Console.WriteLine("\n--- REGISTERED HARDWARE INVENTORY ---");

            using (var context = new MyDbContext())
            {
                // Fetch all assets, including related Office and Employee tables
                var assets = context.Assets
                    .Include(a => a.Office)
                    .Include(a => a.Employee)
                    .ToList();

                if (assets.Count == 0)
                {
                    Console.WriteLine("No assets registered in the database yet.");
                    return;
                }

                // Call our beautiful premium table formatting helper
                PrintAssetsTable(assets);
            }
        }
        #endregion

        #region 5. Update Asset (CRUD - Update)
        static void UpdateAsset()
        {
            Console.WriteLine("\n--- UPDATE REGISTERED ASSET ---");
            Console.Write("Enter the ID of the Asset to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            using (var context = new MyDbContext())
            {
                // Fetch the asset by ID
                var asset = context.Assets.FirstOrDefault(a => a.Id == id);
                if (asset == null)
                {
                    Console.WriteLine("Asset not found in database.");
                    return;
                }

                Console.WriteLine($"Found: {asset.Brand} {asset.ModelName} (Serial: {asset.SerialNumber})");
                Console.WriteLine("Press Enter to skip modifying any field.\n");

                // Update core properties if text is provided
                Console.Write($"Update Brand [{asset.Brand}]: ");
                string brand = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(brand)) asset.Brand = brand;

                Console.Write($"Update Model [{asset.ModelName}]: ");
                string model = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(model)) asset.ModelName = model;

                Console.Write($"Update Purchase Date (YYYY-MM-DD) [{asset.PurchaseDate:yyyy-MM-dd}]: ");
                string dateStr = Console.ReadLine() ?? "";
                if (DateTime.TryParse(dateStr, out DateTime newDate)) asset.PurchaseDate = newDate;

                Console.Write($"Update Price in USD [{asset.PurchasePriceUsd}]: ");
                string priceStr = Console.ReadLine() ?? "";
                if (decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newPrice)) asset.PurchasePriceUsd = newPrice;

                Console.Write($"Update Serial Number [{asset.SerialNumber}]: ");
                string serial = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(serial)) asset.SerialNumber = serial;

                // Save modifications
                context.SaveChanges();
                Console.WriteLine("✔ Asset records successfully updated in your database!");
            }
        }
        #endregion

        #region 6. Delete Asset (CRUD - Delete)
        static void DeleteAsset()
        {
            Console.WriteLine("\n--- DELETE ASSET RECORD ---");
            Console.Write("Enter the ID of the Asset to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            using (var context = new MyDbContext())
            {
                var asset = context.Assets.FirstOrDefault(a => a.Id == id);
                if (asset == null)
                {
                    Console.WriteLine("Asset not found in database.");
                    return;
                }

                Console.WriteLine($"WARNING: You are about to delete {asset.Brand} {asset.ModelName} (Serial: {asset.SerialNumber}).");
                Console.Write("Are you sure? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";
                if (confirm == "y" || confirm == "yes")
                {
                    // EF Delete command
                    context.Assets.Remove(asset);
                    context.SaveChanges();
                    Console.WriteLine("✔ Asset successfully deleted from database.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled safely.");
                }
            }
        }
        #endregion

        #region 7. Search & Filtering (Level 4)
        static void SearchAndFilterAssets()
        {
            Console.WriteLine("\n--- ASSET SEARCH & FILTERS ---");
            Console.WriteLine("1. Search by Brand name");
            Console.WriteLine("2. Show only Expired / Expiring soon assets (Status warning flags)");
            Console.WriteLine("3. Filter assets by Corporate Office Location");
            Console.Write("Choice (1-3): ");
            string choice = Console.ReadLine() ?? "";

            using (var context = new MyDbContext())
            {
                // Fetch query including related Office and Employee tables for rendering
                var query = context.Assets.Include(a => a.Office).Include(a => a.Employee).AsQueryable();
                List<Asset> results = new List<Asset>();

                if (choice == "1")
                {
                    Console.Write("Enter Brand name query: ");
                    string brand = Console.ReadLine()?.ToLower() ?? "";

                    // LINQ Filter: string search
                    results = query.Where(a => a.Brand.ToLower().Contains(brand)).ToList();
                }
                else if (choice == "2")
                {
                    // Filter warning assets
                    var all = query.ToList();
                    results = all.Where(a => a.Status == "RED" || a.Status == "YELLOW").ToList();
                }
                else if (choice == "3")
                {
                    var offices = context.Offices.ToList();
                    if (offices.Count == 0)
                    {
                        Console.WriteLine("No offices exist.");
                        return;
                    }
                    Console.WriteLine("Select office to filter:");
                    for (int i = 0; i < offices.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {offices[i].Name}");
                    }
                    Console.Write("Index: ");
                    int idx;
                    int.TryParse(Console.ReadLine(), out idx);
                    if (idx > 0 && idx <= offices.Count)
                    {
                        int officeId = offices[idx - 1].Id;

                        // LINQ Filter: foreign key match
                        results = query.Where(a => a.OfficeId == officeId).ToList();
                    }
                }

                Console.WriteLine($"\n--- Match results: {results.Count} assets found ---");
                if (results.Count > 0)
                {
                    // Call our beautiful premium table formatting helper to output results nicely
                    PrintAssetsTable(results);
                }
            }
        }
        #endregion

        #region 8. Export Reports (Level 4)
        static void ExportAssets()
        {
            Console.WriteLine("\n--- EXPORT ASSET INVENTORY REPORT ---");
            Console.WriteLine("1. Export to plain TXT file");
            Console.WriteLine("2. Export to spreadsheet CSV format");
            Console.WriteLine("3. Export to JSON file");
            Console.Write("Choice (1-3): ");
            string choice = Console.ReadLine() ?? "";

            using (var context = new MyDbContext())
            {
                var assets = context.Assets.Include(a => a.Office).ToList();
                if (assets.Count == 0)
                {
                    Console.WriteLine("No assets in database to export.");
                    return;
                }

                if (choice == "1")
                {
                    // Plain text formatting
                    string txtContent = "SMART ASSET TRACKING LEDGER REPORT\n";
                    txtContent += $"Generated On: {DateTime.Now:yyyy-MM-dd}\n";
                    txtContent += new string('=', 60) + "\n";
                    foreach (var a in assets)
                    {
                        txtContent += $"[ID: {a.Id}] Type: {a.AssetType} | {a.Brand} {a.ModelName} | Serial: {a.SerialNumber} | Price: {a.PurchasePriceUsd:C} USD\n";
                    }

                    File.WriteAllText("assets_report.txt", txtContent);
                    Console.WriteLine("✔ Exported successfully to 'assets_report.txt' in application directory!");
                }
                else if (choice == "2")
                {
                    // Comma Separated Values
                    string csvContent = "Id,Type,Brand,ModelName,SerialNumber,PurchaseDate,PriceUSD,OfficeName\n";
                    foreach (var a in assets)
                    {
                        csvContent += $"{a.Id},{a.AssetType},{a.Brand},{a.ModelName},{a.SerialNumber},{a.PurchaseDate:yyyy-MM-dd},{a.PurchasePriceUsd},{a.Office?.Name ?? "N/A"}\n";
                    }

                    File.WriteAllText("assets_report.csv", csvContent);
                    Console.WriteLine("✔ Exported successfully to 'assets_report.csv' in application directory!");
                }
                else if (choice == "3")
                {
                    // Simple JSON format building using anonymous flat projection to avoid loop references
                    var flatList = assets.Select(a => new {
                        a.Id,
                        a.AssetType,
                        a.Brand,
                        a.ModelName,
                        a.SerialNumber,
                        PurchaseDate = a.PurchaseDate.ToString("yyyy-MM-dd"),
                        a.PurchasePriceUsd,
                        OfficeName = a.Office?.Name ?? "N/A"
                    }).ToList();

                    // Using simple System.Text.Json
                    string json = System.Text.Json.JsonSerializer.Serialize(flatList, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

                    File.WriteAllText("assets_report.json", json);
                    Console.WriteLine("✔ Exported successfully to 'assets_report.json' in application directory!");
                }
                else
                {
                    Console.WriteLine("Invalid export selection.");
                }
            }
        }
        #endregion

        #region Premium Inventory Formatting Helpers (Table View Design)
        public static void PrintAssetsTable(List<Asset> assets)
        {
            if (assets == null || assets.Count == 0)
            {
                Console.WriteLine("No assets to display.");
                return;
            }

            // Separate and sort computers
            var computers = assets.OfType<ComputerAsset>()
                .OrderBy(a => a.PurchaseDate)
                .ToList();

            // Separate and sort mobile devices
            var mobiles = assets.OfType<MobileAsset>()
                .OrderBy(a => a.PurchaseDate)
                .ToList();

            const int tableWidth = 153;

            if (computers.Count > 0)
            {
                Console.WriteLine("\n┌" + new string('─', tableWidth) + "┐");
                Console.WriteLine("│" + PadCenter("★ CORPORATE COMPUTER HARDWARE INVENTORY ★", tableWidth) + "│");
                Console.WriteLine("├────┬────────────┬──────────────────┬──────────────────┬─────────────┬───────────┬────────────────┬─────────────────┬────────┬────────────────────┬────────┤");
                Console.WriteLine("│ ID │ Brand      │ Model            │ Office Location  │ Purchase D. │ Price USD │ Local Price    │ CPU / Processor │ RAM GB │ Assigned To        │ Status │");
                Console.WriteLine("├────┼────────────┼──────────────────┼──────────────────┼─────────────┼───────────┼────────────────┼─────────────────┼────────┼────────────────────┼────────┤");

                foreach (var asset in computers)
                {
                    var oldColor = Console.ForegroundColor;
                    if (asset.Status == "YELLOW") Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (asset.Status == "RED") Console.ForegroundColor = ConsoleColor.Red;
                    else Console.ForegroundColor = ConsoleColor.Green;

                    string symbol = asset.Office?.Currency switch
                    {
                        "SEK" => "kr",
                        "EUR" => "€",
                        "TRY" => "₺",
                        _ => "$"
                    };
                    string localPriceStr = $"{symbol}{asset.LocalPrice:N2} {asset.Office?.Currency ?? "USD"}";
                    string usdPriceStr = $"${asset.PurchasePriceUsd:N2}";
                    string empName = asset.Employee?.FullName ?? "Unassigned";

                    Console.WriteLine(string.Format("│ {0,-2} │ {1,-10} │ {2,-16} │ {3,-16} │ {4,-11:yyyy-MM-dd} │ {5,-9} │ {6,-14} │ {7,-15} │ {8,-6} │ {9,-18} │ {10,-6} │",
                        asset.Id,
                        Truncate(asset.Brand, 10),
                        Truncate(asset.ModelName, 16),
                        Truncate(asset.Office?.Name ?? "N/A", 16),
                        asset.PurchaseDate,
                        usdPriceStr,
                        Truncate(localPriceStr, 14),
                        Truncate(asset.ProcessorType ?? "N/A", 15),
                        asset.RamSizeGb + " GB",
                        Truncate(empName, 18),
                        asset.Status));

                    Console.ForegroundColor = oldColor;
                }
                Console.WriteLine("└────┴────────────┴──────────────────┴──────────────────┴─────────────┴───────────┴────────────────┴─────────────────┴────────┴────────────────────┴────────┘");
            }

            if (mobiles.Count > 0)
            {
                Console.WriteLine("\n┌" + new string('─', tableWidth) + "┐");
                Console.WriteLine("│" + PadCenter("★ CORPORATE MOBILE DEVICES INVENTORY ★", tableWidth) + "│");
                Console.WriteLine("├────┬────────────┬──────────────────┬──────────────────┬─────────────┬───────────┬────────────────┬───────────────────┬──────┬────────────────────┬────────┤");
                Console.WriteLine("│ ID │ Brand      │ Model            │ Office Location  │ Purchase D. │ Price USD │ Local Price    │ SIM Card Number   │ 5G?  │ Assigned To        │ Status │");
                Console.WriteLine("├────┼────────────┼──────────────────┼──────────────────┼─────────────┼───────────┼────────────────┼───────────────────┼──────┼────────────────────┼────────┤");

                foreach (var asset in mobiles)
                {
                    var oldColor = Console.ForegroundColor;
                    if (asset.Status == "YELLOW") Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (asset.Status == "RED") Console.ForegroundColor = ConsoleColor.Red;
                    else Console.ForegroundColor = ConsoleColor.Green;

                    string symbol = asset.Office?.Currency switch
                    {
                        "SEK" => "kr",
                        "EUR" => "€",
                        "TRY" => "₺",
                        _ => "$"
                    };
                    string localPriceStr = $"{symbol}{asset.LocalPrice:N2} {asset.Office?.Currency ?? "USD"}";
                    string usdPriceStr = $"${asset.PurchasePriceUsd:N2}";
                    string empName = asset.Employee?.FullName ?? "Unassigned";
                    string has5g = asset.Is5gEnabled ? "Yes" : "No";

                    Console.WriteLine(string.Format("│ {0,-2} │ {1,-10} │ {2,-16} │ {3,-16} │ {4,-11:yyyy-MM-dd} │ {5,-9} │ {6,-14} │ {7,-17} │ {8,-4} │ {9,-18} │ {10,-6} │",
                        asset.Id,
                        Truncate(asset.Brand, 10),
                        Truncate(asset.ModelName, 16),
                        Truncate(asset.Office?.Name ?? "N/A", 16),
                        asset.PurchaseDate,
                        usdPriceStr,
                        Truncate(localPriceStr, 14),
                        Truncate(asset.SimCardNumber ?? "N/A", 17),
                        has5g,
                        Truncate(empName, 18),
                        asset.Status));

                    Console.ForegroundColor = oldColor;
                }
                Console.WriteLine("└────┴────────────┴──────────────────┴──────────────────┴─────────────┴───────────┴────────────────┴───────────────────┴──────┴────────────────────┴────────┘");
            }
        }

        private static string Truncate(string val, int maxLen)
        {
            if (string.IsNullOrEmpty(val)) return "";
            return val.Length <= maxLen ? val : val.Substring(0, maxLen - 3) + "...";
        }

        private static string PadCenter(string text, int width)
        {
            if (string.IsNullOrEmpty(text)) return new string(' ', width);
            int spaces = width - text.Length;
            if (spaces <= 0) return text;
            int padLeft = spaces / 2 + text.Length;
            return text.PadLeft(padLeft).PadRight(width);
        }
        #endregion

        #region 9. Reporting Dashboard (Level 3 / Level 5)
        static void GenerateReportsDashboard()
        {
            using (var context = new MyDbContext())
            {
                // Verify we have assets to run reports
                if (!context.Assets.Any())
                {
                    Console.WriteLine("\n❌ Error: No assets registered in the database yet to generate reports.");
                    return;
                }

                bool back = false;
                while (!back)
                {
                    Console.WriteLine("\n========================================================");
                    Console.WriteLine("                HARDWARE REPORTING DASHBOARD            ");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("1. Office Asset Valuations (Detailed Office-by-Office)");
                    Console.WriteLine("2. Global Executive Report (Asset Counts & Expirations)");
                    Console.WriteLine("3. Top 5 Most Expensive Assets (Global USD Rankings)");
                    Console.WriteLine("4. Return to Main Control Panel");
                    Console.WriteLine("========================================================");
                    Console.Write("Select report option (1-4): ");

                    string choice = Console.ReadLine() ?? "";
                    switch (choice)
                    {
                        case "1":
                            PrintOfficeValuationsReport(context);
                            break;
                        case "2":
                            PrintGlobalExecutiveReport(context);
                            break;
                        case "3":
                            PrintMostExpensiveAssetsReport(context);
                            break;
                        case "4":
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }

        static void PrintOfficeValuationsReport(MyDbContext context)
        {
            var offices = context.Offices.ToList();
            Console.WriteLine("\n================ OFFICE ASSET VALUATIONS ================");

            foreach (var office in offices)
            {
                string headerName = office.Name.ToUpper();

                // Center the office header text exactly in 50 characters formatted as ============ NAME ============
                string headerText = $" {headerName} ";
                int padLength = (50 - headerText.Length) / 2;
                string leftPad = new string('=', padLength);
                string rightPad = new string('=', 50 - headerText.Length - padLength);
                Console.WriteLine($"\n{leftPad}{headerText}{rightPad}");

                Console.WriteLine(string.Format("{0,-5}{1,-10}{2,-8}{3,-13}{4}", "ID", "Asset", "Brand", "Price", "Status"));

                decimal totalValue = 0;
                var officeAssets = context.Assets
                    .Where(a => a.OfficeId == office.Id)
                    .OrderBy(a => a.PurchaseDate)
                    .ToList();

                foreach (var asset in officeAssets)
                {
                    string assetDisplayType = "Asset";
                    if (asset is ComputerAsset)
                    {
                        assetDisplayType = asset.AssetType.Contains("Laptop", StringComparison.OrdinalIgnoreCase) ? "Laptop" : "Desktop";
                    }
                    else if (asset is MobileAsset)
                    {
                        assetDisplayType = asset.AssetType.Contains("Phone", StringComparison.OrdinalIgnoreCase) ? "Mobile" : "Tablet";
                    }

                    decimal localPrice = asset.PurchasePriceUsd * office.ExchangeRateToUsd;
                    totalValue += localPrice;

                    string priceStr = $"{localPrice:N0} {office.Currency}";

                    var oldColor = Console.ForegroundColor;
                    if (asset.Status == "YELLOW") Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (asset.Status == "RED") Console.ForegroundColor = ConsoleColor.Red;
                    else Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine(string.Format("{0,-5}{1,-10}{2,-8}{3,-13}{4}",
                        asset.Id,
                        Truncate(assetDisplayType, 9),
                        Truncate(asset.Brand, 7),
                        priceStr,
                        asset.Status));

                    Console.ForegroundColor = oldColor;
                }

                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"Total Office Value: {totalValue:N0} {office.Currency}");
                Console.WriteLine(new string('=', 50));
            }
        }

        static void PrintGlobalExecutiveReport(MyDbContext context)
        {
            Console.WriteLine("\n================ REPORT ================");
            Console.WriteLine("Office Asset Counts");

            var offices = context.Offices.Include(o => o.Assets).ToList();
            foreach (var office in offices)
            {
                Console.WriteLine($"{office.Name,-14} : {office.Assets.Count}");
            }

            Console.WriteLine("\nAssets Near Expiration");

            // Assets close to expiration (Status = YELLOW or RED)
            var nearExpirationAssets = context.Assets
                .ToList() // fetch to run in-memory Status check
                .Where(a => a.Status == "YELLOW" || a.Status == "RED")
                .OrderBy(a => a.RemainingLifetimeMonths)
                .ToList();

            if (nearExpirationAssets.Count == 0)
            {
                Console.WriteLine("- No assets are near expiration (all have > 6 months remaining)");
            }
            else
            {
                foreach (var asset in nearExpirationAssets)
                {
                    string remainingStr = asset.RemainingLifetimeMonths < 0
                        ? "EXPIRED"
                        : $"{asset.RemainingLifetimeMonths:F1} mo left";

                    var oldColor = Console.ForegroundColor;
                    if (asset.Status == "YELLOW") Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (asset.Status == "RED") Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"- {asset.Brand} {asset.ModelName} ({asset.Status} - {remainingStr})");
                    Console.ForegroundColor = oldColor;
                }
            }

            Console.WriteLine(new string('=', 40));
        }

        static void PrintMostExpensiveAssetsReport(MyDbContext context)
        {
            Console.WriteLine("\n================ TOP 5 MOST EXPENSIVE ASSETS ================");
            var expensiveAssets = context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .OrderByDescending(a => a.PurchasePriceUsd)
                .Take(5)
                .ToList();

            int rank = 1;
            foreach (var asset in expensiveAssets)
            {
                string assetType = asset is ComputerAsset ? "Computer" : "Mobile";
                string empName = asset.Employee?.FullName ?? "Unassigned";
                string officeName = asset.Office?.Name ?? "N/A";

                Console.WriteLine($"{rank}. {asset.Brand} {asset.ModelName} ({assetType}) - ${asset.PurchasePriceUsd:N2} USD");
                Console.WriteLine($"   Office: {officeName} | Assigned To: {empName}");
                rank++;
            }
            Console.WriteLine(new string('=', 61));
        }

        #endregion

        #region LEVEL 5 - Enterprise Asset Management System
        static void SimulateLogin()
        {
            Console.WriteLine("==========================================================");
            Console.WriteLine("                  ENTERPRISE LOG-IN GATEWAY               ");
            Console.WriteLine("==========================================================");
            Console.WriteLine("Select login profile:");
            Console.WriteLine("1. Admin (Full access to all features, crud, logs, seeder)");
            Console.WriteLine("2. Manager (Read/Write access, except deletion and seeding)");
            Console.WriteLine("3. Employee (Read-only access to records and reports)");
            Console.Write("Select role (1-3): ");
            string roleChoice = Console.ReadLine() ?? "";

            if (roleChoice == "1")
            {
                Console.Write("Enter Admin Username: ");
                string username = Console.ReadLine() ?? "";
                Console.Write("Enter Password: ");
                string password = Console.ReadLine() ?? "";

                if (username == "admin" && password == "1234")
                {
                    currentUserRole = "Admin";
                    Console.WriteLine("\n🔓 Login Successful! You have logged in as Admin.");
                }
                else
                {
                    Console.WriteLine("\n❌ Invalid credentials! Access denied. Logging in as default 'Employee' role.");
                    currentUserRole = "Employee";
                }
            }
            else if (roleChoice == "2")
            {
                Console.Write("Enter Manager Username: ");
                string username = Console.ReadLine() ?? "";
                Console.Write("Enter Password: ");
                string password = Console.ReadLine() ?? "";

                if (username == "manager" && password == "1234")
                {
                    currentUserRole = "Manager";
                    Console.WriteLine("\n🔓 Login Successful! You have logged in as Manager.");
                }
                else
                {
                    Console.WriteLine("\n❌ Invalid credentials! Access denied. Logging in as default 'Employee' role.");
                    currentUserRole = "Employee";
                }
            }
            else
            {
                currentUserRole = "Employee";
                Console.WriteLine("\n🔓 Login Successful! You have logged in as Employee (Read-Only).");
            }
            Console.WriteLine("==========================================================\n");
            Console.WriteLine("Press Enter to enter the corporate network...");
            Console.ReadLine();
        }

        static bool CheckRole(params string[] allowedRoles)
        {
            if (allowedRoles.Contains(currentUserRole))
            {
                return true;
            }
            Console.WriteLine($"\n❌ Access Denied: Logged in as '{currentUserRole}'. This operation requires: {string.Join(" or ", allowedRoles)} permissions.");
            return false;
        }

        static void ShowEnterpriseDashboard()
        {
            using (var context = new MyDbContext())
            {
                if (!context.Assets.Any())
                {
                    Console.WriteLine("\n❌ Error: No assets registered in the database yet to load dashboard stats.");
                    return;
                }

                int totalAssets = context.Assets.Count();
                int totalEmployees = context.Employees.Count();

                int expiringSoon = context.Assets.ToList().Count(a => a.Status == "YELLOW" || a.Status == "RED");

                var swedenOffice = context.Offices.FirstOrDefault(o => o.Country == "Sweden");
                decimal exchangeRate = swedenOffice?.ExchangeRateToUsd ?? 8.3333m;
                decimal totalValueSek = context.Assets.ToList().Sum(a => a.PurchasePriceUsd * exchangeRate);

                var topType = context.Assets
                    .GroupBy(a => a.AssetType)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault() ?? "N/A";

                var mostExpensiveOfficeName = context.Offices
                    .Include(o => o.Assets)
                    .ToList()
                    .OrderByDescending(o => o.Assets.Sum(a => a.PurchasePriceUsd))
                    .Select(o => o.Name)
                    .FirstOrDefault() ?? "N/A";

                Console.WriteLine("\n================ DASHBOARD ================");
                Console.WriteLine($"Logged in as: {currentUserRole}");
                Console.WriteLine();
                Console.WriteLine($"Total Assets          : {totalAssets}");
                Console.WriteLine($"Total Employees       : {totalEmployees}");
                Console.WriteLine($"Expiring Soon         : {expiringSoon}");
                Console.WriteLine($"Total Company Value   : {totalValueSek:N0} SEK");
                Console.WriteLine($"Top Asset Type        : {topType}");
                Console.WriteLine($"Most Expensive Office : {mostExpensiveOfficeName}");
                Console.WriteLine("===========================================");
            }
        }

        static void ShowEmployeeAssignments()
        {
            using (var context = new MyDbContext())
            {
                var employees = context.Employees.Include(e => e.AssignedAssets).ToList();
                if (employees.Count == 0)
                {
                    Console.WriteLine("\n❌ Error: No employee roster cards registered in the database yet.");
                    return;
                }

                Console.WriteLine("\n--- SELECT EMPLOYEE FOR ASSIGNMENT CARD ---");
                for (int i = 0; i < employees.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {employees[i].FullName} ({employees[i].Department}) - {employees[i].AssignedAssets.Count} Assets");
                }
                Console.Write("Select employee index (or press Enter to exit): ");
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int index) && index > 0 && index <= employees.Count)
                {
                    var emp = employees[index - 1];
                    Console.WriteLine("\n================ EMPLOYEE ASSETS ================");
                    Console.WriteLine($"Employee    : {emp.FullName}");
                    Console.WriteLine($"Department  : {emp.Department}");
                    Console.WriteLine();
                    Console.WriteLine("Assigned Assets:");

                    var assets = emp.AssignedAssets.ToList();
                    if (assets.Count == 0)
                    {
                        Console.WriteLine("- No assets assigned to this employee.");
                    }
                    else
                    {
                        int rank = 1;
                        foreach (var asset in assets)
                        {
                            Console.WriteLine($"{rank}. {asset.Brand} {asset.ModelName}");
                            rank++;
                        }
                    }
                    Console.WriteLine("=================================================");
                }
            }
        }

        static void ManageMaintenanceLogs()
        {
            using (var context = new MyDbContext())
            {
                var assets = context.Assets.Include(a => a.Office).Include(a => a.Employee).ToList();
                if (assets.Count == 0)
                {
                    Console.WriteLine("\n❌ Error: No assets registered in the database yet to manage maintenance.");
                    return;
                }

                Console.WriteLine("\n--- HARDWARE MAINTENANCE LOGS ---");
                Console.WriteLine("1. View Active Maintenance Roster (All Assets)");
                Console.WriteLine("2. Register New Maintenance Entry (Admin / Manager Only)");
                Console.WriteLine("3. Return to Main Control Panel");
                Console.Write("Select option (1-3): ");

                string choice = Console.ReadLine() ?? "";
                if (choice == "1")
                {
                    Console.WriteLine("\n================ ASSET MAINTENANCE ROSTER ================");
                    Console.WriteLine(string.Format("{0,-4} {1,-15} {2,-15} {3,-15} {4,-20}", "ID", "Asset Brand/Model", "Last Maint.", "Next Maint.", "Maintenance Notes"));
                    Console.WriteLine(new string('-', 75));

                    foreach (var asset in assets)
                    {
                        string name = $"{asset.Brand} {asset.ModelName}";
                        string lastM = asset.LastMaintenanceDate?.ToString("yyyy-MM-dd") ?? "None";
                        string nextM = asset.NextMaintenanceDate?.ToString("yyyy-MM-dd") ?? "None";
                        string notes = asset.MaintenanceNotes ?? "No active logs";

                        Console.WriteLine(string.Format("{0,-4} {1,-15} {2,-15} {3,-15} {4,-20}",
                            asset.Id,
                            Truncate(name, 15),
                            lastM,
                            nextM,
                            Truncate(notes, 20)));
                    }
                    Console.WriteLine("==========================================================");
                }
                else if (choice == "2")
                {
                    if (!CheckRole("Admin", "Manager")) return;

                    Console.Write("\nEnter the ID of the Asset to log maintenance for: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Invalid ID.");
                        return;
                    }

                    var asset = context.Assets.FirstOrDefault(a => a.Id == id);
                    if (asset == null)
                    {
                        Console.WriteLine("Asset not found in database.");
                        return;
                    }

                    Console.WriteLine($"Found Asset: {asset.Brand} {asset.ModelName}");

                    Console.Write("Enter Last Maintenance Date (YYYY-MM-DD) [Leave empty for today]: ");
                    string lastStr = Console.ReadLine() ?? "";
                    DateTime lastDate = string.IsNullOrEmpty(lastStr) ? DateTime.Today : DateTime.Parse(lastStr);

                    Console.Write("Enter Next Maintenance Date (YYYY-MM-DD) [Leave empty for 6 months from last]: ");
                    string nextStr = Console.ReadLine() ?? "";
                    DateTime nextDate = string.IsNullOrEmpty(nextStr) ? lastDate.AddMonths(6) : DateTime.Parse(nextStr);

                    Console.Write("Enter Maintenance Details / Notes: ");
                    string notes = Console.ReadLine() ?? "";

                    asset.LastMaintenanceDate = lastDate;
                    asset.NextMaintenanceDate = nextDate;
                    asset.MaintenanceNotes = notes;

                    context.SaveChanges();
                    Console.WriteLine($"\n✔ Maintenance logged successfully for {asset.Brand} {asset.ModelName}!");
                }
            }
        }
        #endregion
    }
}
