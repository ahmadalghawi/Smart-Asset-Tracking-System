using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleEfLex1;
using Smart_Asset_Tracking_System.Models;

namespace Smart_Asset_Tracking_System
{
    public static class DbSeeder
    {
        public static void Seed(MyDbContext context)
        {
            Console.WriteLine("Initializing demo database seeding...");

            // 1. Safe Office Retrieval or Creation
            var swedenOffice = context.Offices.FirstOrDefault(o => o.Country == "Sweden");
            if (swedenOffice == null)
            {
                swedenOffice = new Office { Name = "Sweden Office", Country = "Sweden", Currency = "SEK", ExchangeRateToUsd = 8.3333m };
                context.Offices.Add(swedenOffice);
                Console.WriteLine("✔ Seeded Sweden Office.");
            }

            var usaOffice = context.Offices.FirstOrDefault(o => o.Country == "USA");
            if (usaOffice == null)
            {
                usaOffice = new Office { Name = "USA Office", Country = "USA", Currency = "USD", ExchangeRateToUsd = 1.0000m };
                context.Offices.Add(usaOffice);
                Console.WriteLine("✔ Seeded USA Office.");
            }

            var germanyOffice = context.Offices.FirstOrDefault(o => o.Country == "Germany");
            if (germanyOffice == null)
            {
                germanyOffice = new Office { Name = "Germany Office", Country = "Germany", Currency = "EUR", ExchangeRateToUsd = 0.8264m };
                context.Offices.Add(germanyOffice);
                Console.WriteLine("✔ Seeded Germany Office.");
            }

            var turkeyOffice = context.Offices.FirstOrDefault(o => o.Country == "Turkey");
            if (turkeyOffice == null)
            {
                turkeyOffice = new Office { Name = "Turkey Office", Country = "Turkey", Currency = "TRY", ExchangeRateToUsd = 32.2500m };
                context.Offices.Add(turkeyOffice);
                Console.WriteLine("✔ Seeded Turkey Office.");
            }

            context.SaveChanges(); // Persist all office additions to database to generate IDs

            // 2. Safe Employee Retrieval or Creation
            var sarah = context.Employees.FirstOrDefault(e => e.FullName.Contains("Sarah"));
            if (sarah == null)
            {
                sarah = new Employee { FullName = "Sarah Johnson", Department = "IT Support", Email = "sarah.j@company.com" };
                context.Employees.Add(sarah);
                Console.WriteLine("✔ Registered employee Sarah Johnson.");
            }

            var ahmad = context.Employees.FirstOrDefault(e => e.FullName.Contains("Ahmad"));
            if (ahmad == null)
            {
                ahmad = new Employee { FullName = "Ahmad Al-Ghawi", Department = "Software Engineering", Email = "ahmad.a@company.com" };
                context.Employees.Add(ahmad);
                Console.WriteLine("✔ Registered employee Ahmad Al-Ghawi.");
            }

            var emily = context.Employees.FirstOrDefault(e => e.FullName.Contains("Emily"));
            if (emily == null)
            {
                emily = new Employee { FullName = "Emily Davis", Department = "Marketing", Email = "emily.d@company.com" };
                context.Employees.Add(emily);
                Console.WriteLine("✔ Registered employee Emily Davis.");
            }

            var sofia = context.Employees.FirstOrDefault(e => e.FullName.Contains("Sofia"));
            if (sofia == null)
            {
                sofia = new Employee { FullName = "Sofia Martinez", Department = "Human Resources", Email = "sofia.m@company.com" };
                context.Employees.Add(sofia);
                Console.WriteLine("✔ Registered employee Sofia Martinez.");
            }

            var michael = context.Employees.FirstOrDefault(e => e.FullName.Contains("Michael"));
            if (michael == null)
            {
                michael = new Employee { FullName = "Michael Chang", Department = "Sales & Business Dev", Email = "michael.c@company.com" };
                context.Employees.Add(michael);
                Console.WriteLine("✔ Registered employee Michael Chang.");
            }

            var john = context.Employees.FirstOrDefault(e => e.FullName.Contains("John"));
            if (john == null)
            {
                john = new Employee { FullName = "John Doe", Department = "Finance", Email = "john.doe@company.com" };
                context.Employees.Add(john);
                Console.WriteLine("✔ Registered employee John Doe.");
            }

            var david = context.Employees.FirstOrDefault(e => e.FullName.Contains("David"));
            if (david == null)
            {
                david = new Employee { FullName = "David Larsson", Department = "Engineering", Email = "david.l@company.com" };
                context.Employees.Add(david);
                Console.WriteLine("✔ Registered employee David Larsson.");
            }

            var elena = context.Employees.FirstOrDefault(e => e.FullName.Contains("Elena"));
            if (elena == null)
            {
                elena = new Employee { FullName = "Elena Fischer", Department = "Operations", Email = "elena.f@company.com" };
                context.Employees.Add(elena);
                Console.WriteLine("✔ Registered employee Elena Fischer.");
            }

            var hassan = context.Employees.FirstOrDefault(e => e.FullName.Contains("Hassan"));
            if (hassan == null)
            {
                hassan = new Employee { FullName = "Hassan Yilmaz", Department = "Logistics", Email = "hassan.y@company.com" };
                context.Employees.Add(hassan);
                Console.WriteLine("✔ Registered employee Hassan Yilmaz.");
            }

            var robert = context.Employees.FirstOrDefault(e => e.FullName.Contains("Robert"));
            if (robert == null)
            {
                robert = new Employee { FullName = "Robert Smith", Department = "Security", Email = "robert.s@company.com" };
                context.Employees.Add(robert);
                Console.WriteLine("✔ Registered employee Robert Smith.");
            }

            var anna = context.Employees.FirstOrDefault(e => e.FullName.Contains("Anna"));
            if (anna == null)
            {
                anna = new Employee { FullName = "Anna Lindstrom", Department = "Quality Assurance", Email = "anna.l@company.com" };
                context.Employees.Add(anna);
                Console.WriteLine("✔ Registered employee Anna Lindstrom.");
            }

            var clara = context.Employees.FirstOrDefault(e => e.FullName.Contains("Clara"));
            if (clara == null)
            {
                clara = new Employee { FullName = "Clara Dubois", Department = "Product Management", Email = "clara.d@company.com" };
                context.Employees.Add(clara);
                Console.WriteLine("✔ Registered employee Clara Dubois.");
            }

            context.SaveChanges(); // Persist all employee additions to generate IDs

            // 3. Seed Assets individually if they don't exist by SerialNumber
            var newAssets = new List<Asset>();

            void AddComputerIfMissing(string type, string brand, string model, DateTime purchaseDate, decimal priceUsd, string serial, int officeId, int? employeeId, string cpu, int ram)
            {
                if (!context.Assets.Any(a => a.SerialNumber == serial))
                {
                    newAssets.Add(new ComputerAsset
                    {
                        AssetType = type,
                        Brand = brand,
                        ModelName = model,
                        PurchaseDate = purchaseDate,
                        PurchasePriceUsd = priceUsd,
                        SerialNumber = serial,
                        OfficeId = officeId,
                        EmployeeId = employeeId,
                        ProcessorType = cpu,
                        RamSizeGb = ram
                    });
                }
            }

            void AddMobileIfMissing(string type, string brand, string model, DateTime purchaseDate, decimal priceUsd, string serial, int officeId, int? employeeId, string sim, bool has5g)
            {
                if (!context.Assets.Any(a => a.SerialNumber == serial))
                {
                    newAssets.Add(new MobileAsset
                    {
                        AssetType = type,
                        Brand = brand,
                        ModelName = model,
                        PurchaseDate = purchaseDate,
                        PurchasePriceUsd = priceUsd,
                        SerialNumber = serial,
                        OfficeId = officeId,
                        EmployeeId = employeeId,
                        SimCardNumber = sim,
                        Is5gEnabled = has5g
                    });
                }
            }

            // === COMPUTERS ===
            AddComputerIfMissing("Laptop", "Lenovo", "ThinkPad X1 Carbon", DateTime.Today.AddYears(-3).AddMonths(2).AddDays(10), 1500.00m, "SN-LENOVO-X1", swedenOffice.Id, sarah.Id, "Intel Core i7", 16);
            AddComputerIfMissing("Laptop", "Asus", "ZenBook 14", DateTime.Today.AddYears(-3).AddMonths(5).AddDays(25), 1250.00m, "SN-ASUS-ZB14", germanyOffice.Id, ahmad.Id, "AMD Ryzen 7", 16);
            AddComputerIfMissing("Laptop", "HP", "EliteBook 840 G10", DateTime.Today.AddYears(-1).AddMonths(-4), 1399.00m, "SN-HP-EB840", swedenOffice.Id, sofia.Id, "Intel Core i5", 16);
            AddComputerIfMissing("Laptop", "Apple", "MacBook Pro 16", DateTime.Today.AddYears(-1).AddMonths(-2), 2499.00m, "SN-MACBOOK-16", usaOffice.Id, michael.Id, "Apple M3 Pro", 18);
            AddComputerIfMissing("Laptop", "Dell", "Latitude 7440", DateTime.Today.AddYears(-3).AddMonths(1).AddDays(15), 1150.00m, "SN-DELL-LAT74", turkeyOffice.Id, null, "Intel Core i5", 8);
            AddComputerIfMissing("Desktop", "Dell", "OptiPlex 7010", DateTime.Today.AddYears(-3).AddMonths(4).AddDays(10), 850.00m, "SN-DELL-OPT70", usaOffice.Id, sofia.Id, "Intel Core i5", 16);
            AddComputerIfMissing("Desktop Computer", "Asus", "ROG Tower STrix", DateTime.Today.AddMonths(-6), 1899.00m, "SN-ASUS-ROG", germanyOffice.Id, null, "Intel Core i9", 32);
            AddComputerIfMissing("Laptop", "Lenovo", "ThinkPad T14 Gen4", DateTime.Today.AddYears(-1).AddMonths(-3), 1299.00m, "SN-LENOVO-T14", swedenOffice.Id, david.Id, "AMD Ryzen 5", 16);
            AddComputerIfMissing("Laptop", "Apple", "MacBook Air M2", DateTime.Today.AddYears(-2).AddMonths(-9), 1099.00m, "SN-APPLE-MBA13", usaOffice.Id, anna.Id, "Apple M2", 8);
            AddComputerIfMissing("Laptop", "Dell", "XPS 15 9530", DateTime.Today.AddYears(-2).AddMonths(-1), 2199.00m, "SN-DELL-XPS15", germanyOffice.Id, elena.Id, "Intel Core i9", 32);
            AddComputerIfMissing("Desktop", "HP", "Envy Desktop TE02", DateTime.Today.AddYears(-3).AddMonths(1).AddDays(5), 950.00m, "SN-HP-ENVY", turkeyOffice.Id, hassan.Id, "Intel Core i7", 16);
            AddComputerIfMissing("Laptop", "MSI", "Stealth 16 Studio", DateTime.Today.AddMonths(-7), 2399.00m, "SN-MSI-STEALTH", germanyOffice.Id, clara.Id, "Intel Core i9", 32);
            AddComputerIfMissing("Desktop", "Lenovo", "ThinkCentre M70q", DateTime.Today.AddYears(-3).AddMonths(2).AddDays(-10), 650.00m, "SN-LENOVO-M70", usaOffice.Id, robert.Id, "Intel Core i5", 8);
            AddComputerIfMissing("Laptop", "Fujitsu", "Lifebook U7411", DateTime.Today.AddYears(-3).AddMonths(4).AddDays(5), 1200.00m, "SN-FUJITSU-U7", swedenOffice.Id, david.Id, "Intel Core i7", 16);
            AddComputerIfMissing("Laptop", "HP", "Pavilion Plus 14", DateTime.Today.AddMonths(-11), 899.00m, "SN-HP-PAV14", turkeyOffice.Id, null, "Intel Core i7", 16);
            AddComputerIfMissing("Laptop", "Acer", "Swift Go 14", DateTime.Today.AddYears(-3).AddMonths(5).AddDays(1), 799.00m, "SN-ACER-SWIFT", germanyOffice.Id, clara.Id, "Intel Core i5", 16);
            AddComputerIfMissing("Desktop Computer", "Apple", "iMac 24 M3", DateTime.Today.AddMonths(-4), 1499.00m, "SN-IMAC-24", usaOffice.Id, robert.Id, "Apple M3", 8);

            // === MOBILE DEVICES ===
            AddMobileIfMissing("Mobile Phone", "Apple", "iPhone 15 Pro", DateTime.Today.AddMonths(-8), 999.00m, "SN-IPHONE-15", usaOffice.Id, emily.Id, "+1-555-0199", true);
            AddMobileIfMissing("Mobile Phone", "Samsung", "Galaxy S22 Ultra", DateTime.Today.AddYears(-3).AddMonths(-2), 1099.00m, "SN-SAMSUNG-S22", turkeyOffice.Id, sarah.Id, "+90-555-2244", true);
            AddMobileIfMissing("Tablet", "Apple", "iPad Pro 12.9", DateTime.Today.AddYears(-3).AddMonths(3).AddDays(20), 1099.00m, "SN-IPAD-PRO", swedenOffice.Id, john.Id, "E-SIM-SWE-41", true);
            AddMobileIfMissing("Tablet", "Samsung", "Galaxy Tab S9", DateTime.Today.AddYears(-1).AddMonths(-1), 799.00m, "SN-SAM-TAB-S9", germanyOffice.Id, ahmad.Id, "E-SIM-GER-99", false);
            AddMobileIfMissing("Mobile Phone", "Google", "Pixel 8 Pro", DateTime.Today.AddMonths(-3), 999.00m, "SN-PIXEL-8", turkeyOffice.Id, michael.Id, "+90-555-9876", true);
            AddMobileIfMissing("Mobile Phone", "Apple", "iPhone 13", DateTime.Today.AddYears(-3).AddMonths(1).AddDays(5), 699.00m, "SN-IPHONE-13", usaOffice.Id, null, "+1-555-0123", true);
            AddMobileIfMissing("Mobile Phone", "Xiaomi", "Redmi Note 12", DateTime.Today.AddYears(-1).AddMonths(-6), 299.00m, "SN-XIAOMI-12", germanyOffice.Id, emily.Id, "+49-155-4321", false);
            AddMobileIfMissing("Mobile Phone", "Apple", "iPhone 14 Plus", DateTime.Today.AddYears(-2).AddMonths(-2), 899.00m, "SN-IPHONE-14", usaOffice.Id, anna.Id, "+1-555-0211", true);
            AddMobileIfMissing("Mobile Phone", "Samsung", "Galaxy S23+", DateTime.Today.AddYears(-1).AddMonths(-2), 999.00m, "SN-SAMSUNG-S23", germanyOffice.Id, elena.Id, "+49-155-9822", true);
            AddMobileIfMissing("Mobile Phone", "Samsung", "Galaxy A54 5G", DateTime.Today.AddYears(-3).AddMonths(4).AddDays(15), 449.00m, "SN-SAMSUNG-A54", turkeyOffice.Id, hassan.Id, "+90-555-4455", true);
            AddMobileIfMissing("Tablet", "Apple", "iPad Air 5", DateTime.Today.AddYears(-1).AddMonths(-8), 599.00m, "SN-IPAD-AIR", swedenOffice.Id, david.Id, "E-SIM-SWE-88", false);
            AddMobileIfMissing("Tablet", "Samsung", "Galaxy Tab A8", DateTime.Today.AddYears(-3).AddMonths(1).AddDays(2), 229.00m, "SN-SAM-TAB-A8", germanyOffice.Id, null, "E-SIM-GER-11", false);
            AddMobileIfMissing("Mobile Phone", "Google", "Pixel 7a", DateTime.Today.AddYears(-2).AddMonths(-3), 499.00m, "SN-PIXEL-7", swedenOffice.Id, anna.Id, "+46-70-12345", true);
            AddMobileIfMissing("Mobile Phone", "OnePlus", "OnePlus 11 5G", DateTime.Today.AddYears(-1).AddMonths(-4), 699.00m, "SN-ONEPLUS-11", turkeyOffice.Id, hassan.Id, "+90-555-1111", true);
            AddMobileIfMissing("Mobile Phone", "Apple", "iPhone 15 Pro Max", DateTime.Today.AddMonths(-5), 1199.00m, "SN-IPHONE-15PM", usaOffice.Id, robert.Id, "+1-555-9900", true);
            AddMobileIfMissing("Mobile Phone", "Motorola", "Edge 40 Neo", DateTime.Today.AddYears(-3).AddMonths(2).AddDays(22), 349.00m, "SN-MOTO-EDGE", germanyOffice.Id, null, "+49-155-6677", true);
            AddMobileIfMissing("Tablet", "Apple", "iPad Mini 6", DateTime.Today.AddYears(-2).AddMonths(-5), 499.00m, "SN-IPAD-MINI", swedenOffice.Id, david.Id, "E-SIM-SWE-12", false);

            if (newAssets.Count > 0)
            {
                context.Assets.AddRange(newAssets);
                context.SaveChanges();
                Console.WriteLine($"✔ Seeded {newAssets.Count} new company assets (Computers and Mobile devices).");
            }
            else
            {
                Console.WriteLine("✔ All demo assets are already present in the database.");
            }

            Console.WriteLine("Database seeding completed successfully!");
        }
    }
}
