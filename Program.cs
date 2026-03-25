using Project.Equipments;
using Project.Users;
using Project.Loans;
using Project.Services;
using Project.Rules;

namespace Project;

public class Program
{
    public static void Main()
    {
        EquipmentService equipmentService = new();
        LoanService loanService = new(new SimplePenaltyCalculator());
        ReportService reportService = new();

        List<User> users = new();

        int equipmentId = 1;
        int userId = 1;
        int loanId = 1;

        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddUser(users, ref userId);
                        break;

                    case "2":
                        AddEquipment(equipmentService, ref equipmentId);
                        break;

                    case "3":
                        ShowAllEquipment(equipmentService);
                        break;

                    case "4":
                        ShowAvailableEquipment(equipmentService);
                        break;

                    case "5":
                        BorrowEquipment(users, equipmentService, loanService, ref loanId);
                        break;

                    case "6":
                        ReturnEquipment(loanService);
                        break;

                    case "7":
                        MarkEquipmentUnavailable(equipmentService);
                        break;

                    case "8":
                        ShowActiveLoansByUser(users, loanService);
                        break;

                    case "9":
                        ShowOverdueLoans(loanService);
                        break;

                    case "10":
                        ShowReport(equipmentService, users, loanService, reportService);
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine();
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("===== UNIVERSITY RENTAL MENU =====");
        Console.WriteLine("1. Add user");
        Console.WriteLine("2. Add equipment");
        Console.WriteLine("3. Show all equipment");
        Console.WriteLine("4. Show available equipment");
        Console.WriteLine("5. Borrow equipment");
        Console.WriteLine("6. Return equipment");
        Console.WriteLine("7. Mark equipment as unavailable");
        Console.WriteLine("8. Show active loans by user");
        Console.WriteLine("9. Show overdue loans");
        Console.WriteLine("10. Show summary report");
        Console.WriteLine("0. Exit");
        Console.Write("Choose option: ");
    }

    private static void AddUser(List<User> users, ref int userId)
    {
        Console.WriteLine("Choose user type:");
        Console.WriteLine("1. Student");
        Console.WriteLine("2. Employee");
        Console.Write("Type: ");
        string? type = Console.ReadLine();

        Console.Write("First name: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Last name: ");
        string lastName = Console.ReadLine() ?? "";

        User user;

        if (type == "1")
        {
            user = new Student(userId++, firstName, lastName);
        }
        else if (type == "2")
        {
            user = new Employee(userId++, firstName, lastName);
        }
        else
        {
            Console.WriteLine("Invalid user type.");
            return;
        }

        users.Add(user);
        Console.WriteLine($"User added. Id: {user.Id}, Name: {user.FullName}");
    }

    private static void AddEquipment(EquipmentService equipmentService, ref int equipmentId)
    {
        Console.WriteLine("Choose equipment type:");
        Console.WriteLine("1. Laptop");
        Console.WriteLine("2. Projector");
        Console.WriteLine("3. Camera");
        Console.Write("Type: ");
        string? type = Console.ReadLine();

        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "";

        Equipment equipment;

        if (type == "1")
        {
            Console.Write("RAM (GB): ");
            int ramGb = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("CPU model: ");
            string cpuModel = Console.ReadLine() ?? "";

            equipment = new Laptop(equipmentId++, name, ramGb, cpuModel);
        }
        else if (type == "2")
        {
            Console.Write("Resolution: ");
            string resolution = Console.ReadLine() ?? "";

            Console.Write("Brightness (lumens): ");
            int brightnessLumens = int.Parse(Console.ReadLine() ?? "0");

            equipment = new Projector(equipmentId++, name, resolution, brightnessLumens);
        }
        else if (type == "3")
        {
            Console.Write("Sensor type: ");
            string sensorType = Console.ReadLine() ?? "";

            Console.Write("Lens mount: ");
            string lensMount = Console.ReadLine() ?? "";

            equipment = new Camera(equipmentId++, name, sensorType, lensMount);
        }
        else
        {
            Console.WriteLine("Invalid equipment type.");
            return;
        }

        equipmentService.AddEquipment(equipment);
        Console.WriteLine($"Equipment added. Id: {equipment.Id}, Name: {equipment.Name}");
    }

    private static void ShowAllEquipment(EquipmentService equipmentService)
    {
        List<Equipment> equipment = equipmentService.GetAllEquipment();

        if (equipment.Count == 0)
        {
            Console.WriteLine("No equipment found.");
            return;
        }

        foreach (Equipment item in equipment)
        {
            Console.WriteLine($"{item.Id} - {item.Name} ({item.Status})");
        }
    }

    private static void ShowAvailableEquipment(EquipmentService equipmentService)
    {
        List<Equipment> equipment = equipmentService.GetAvailableEquipment();

        if (equipment.Count == 0)
        {
            Console.WriteLine("No available equipment.");
            return;
        }

        foreach (Equipment item in equipment)
        {
            Console.WriteLine($"{item.Id} - {item.Name} ({item.Status})");
        }
    }

    private static void BorrowEquipment(
        List<User> users,
        EquipmentService equipmentService,
        LoanService loanService,
        ref int loanId)
    {
        if (users.Count == 0)
        {
            Console.WriteLine("No users in system.");
            return;
        }

        if (equipmentService.GetAllEquipment().Count == 0)
        {
            Console.WriteLine("No equipment in system.");
            return;
        }

        Console.WriteLine("Users:");
        foreach (User user in users)
        {
            Console.WriteLine($"{user.Id} - {user.FullName} ({user.GetType().Name})");
        }

        Console.Write("User id: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        User userToBorrow = users.FirstOrDefault(u => u.Id == userId)
            ?? throw new InvalidOperationException("User not found.");

        Console.WriteLine("Equipment:");
        foreach (Equipment item in equipmentService.GetAllEquipment())
        {
            Console.WriteLine($"{item.Id} - {item.Name} ({item.Status})");
        }

        Console.Write("Equipment id: ");
        int equipmentId = int.Parse(Console.ReadLine() ?? "0");

        Equipment equipmentToBorrow = equipmentService.GetById(equipmentId);

        Console.Write("Days: ");
        int days = int.Parse(Console.ReadLine() ?? "0");

        Loan loan = loanService.BorrowEquipment(loanId++, userToBorrow, equipmentToBorrow, days);
        Console.WriteLine($"Loan created. Id: {loan.Id}");
    }

    private static void ReturnEquipment(LoanService loanService)
    {
        List<Loan> loans = loanService.GetAllLoans();

        if (loans.Count == 0)
        {
            Console.WriteLine("No loans found.");
            return;
        }

        Console.WriteLine("Loans:");
        foreach (Loan loan in loans)
        {
            Console.WriteLine($"{loan.Id} - {loan.User.FullName} - {loan.Equipment.Name} - Active: {loan.IsActive}");
        }

        Console.Write("Loan id: ");
        int loanId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Return today? (y/n): ");
        string? answer = Console.ReadLine();

        DateTime returnDate;

        if (answer?.ToLower() == "y")
        {
            returnDate = DateTime.Now;
        }
        else
        {
            Console.Write("Return date (yyyy-MM-dd): ");
            returnDate = DateTime.Parse(Console.ReadLine() ?? "");
        }

        decimal penalty = loanService.ReturnEquipment(loanId, returnDate);
        Console.WriteLine($"Equipment returned. Penalty: {penalty}");
    }

    private static void MarkEquipmentUnavailable(EquipmentService equipmentService)
    {
        List<Equipment> equipment = equipmentService.GetAllEquipment();

        if (equipment.Count == 0)
        {
            Console.WriteLine("No equipment found.");
            return;
        }

        foreach (Equipment item in equipment)
        {
            Console.WriteLine($"{item.Id} - {item.Name} ({item.Status})");
        }

        Console.Write("Equipment id: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        equipmentService.MarkAsUnavailable(id);
        Console.WriteLine("Equipment marked as unavailable.");
    }

    private static void ShowActiveLoansByUser(List<User> users, LoanService loanService)
    {
        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (User user in users)
        {
            Console.WriteLine($"{user.Id} - {user.FullName}");
        }

        Console.Write("User id: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        List<Loan> loans = loanService.GetActiveLoansByUser(userId);

        if (loans.Count == 0)
        {
            Console.WriteLine("No active loans for this user.");
            return;
        }

        foreach (Loan loan in loans)
        {
            Console.WriteLine($"{loan.Id} - {loan.Equipment.Name} - Due: {loan.DueDate:yyyy-MM-dd}");
        }
    }

    private static void ShowOverdueLoans(LoanService loanService)
    {
        List<Loan> loans = loanService.GetOverdueLoans();

        if (loans.Count == 0)
        {
            Console.WriteLine("No overdue loans.");
            return;
        }

        foreach (Loan loan in loans)
        {
            Console.WriteLine($"{loan.Id} - {loan.User.FullName} - {loan.Equipment.Name} - Due: {loan.DueDate:yyyy-MM-dd}");
        }
    }

    private static void ShowReport(
        EquipmentService equipmentService,
        List<User> users,
        LoanService loanService,
        ReportService reportService)
    {
        string report = reportService.GenerateSummary(
            equipmentService.GetAllEquipment(),
            users,
            loanService.GetAllLoans()
        );

        Console.WriteLine(report);
    }
}