using Project.Equipments;
using Project.Users;
using Project.Loans;
using Project.Rules;

namespace Project.Services;

public class MenuService
{
    private readonly EquipmentService _equipmentService;
    private readonly LoanService _loanService;
    private readonly ReportService _reportService;

    private readonly List<User> _users = new();

    private int _equipmentId = 1;
    private int _userId = 1;
    private int _loanId = 1;

    public MenuService()
    {
        _equipmentService = new EquipmentService();
        _loanService = new LoanService(new SimplePenaltyCalculator());
        _reportService = new ReportService();
    }

    public void Run()
    {
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
                        AddUser();
                        break;
                    case "2":
                        AddEquipment();
                        break;
                    case "3":
                        ShowAllEquipment();
                        break;
                    case "4":
                        ShowAvailableEquipment();
                        break;
                    case "5":
                        BorrowEquipment();
                        break;
                    case "6":
                        ReturnEquipment();
                        break;
                    case "7":
                        MarkUnavailable();
                        break;
                    case "8":
                        ShowUserLoans();
                        break;
                    case "9":
                        ShowOverdue();
                        break;
                    case "10":
                        ShowReport();
                        break;
                    case "11":
                        ShowAllUsers();
                        break;
                    case "12":
                        ShowStudents();
                        break;
                    case "13":
                        ShowEmployees();
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

    private void ShowMenu()
    {
        Console.WriteLine("===== MENU =====");
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
        Console.WriteLine("11. Show all users");
        Console.WriteLine("12. Show students");
        Console.WriteLine("13. Show employees");
        Console.WriteLine("0. Exit");
        Console.Write("Choice: ");
    }

    private void AddUser()
    {
        Console.Write("1-Student, 2-Employee: ");
        string? type = Console.ReadLine();

        Console.Write("First name: ");
        string first = Console.ReadLine() ?? "";

        Console.Write("Last name: ");
        string last = Console.ReadLine() ?? "";

        User user;

        if (type == "1")
        {
            user = new Student(_userId++, first, last);
        }
        else if (type == "2")
        {
            user = new Employee(_userId++, first, last);
        }
        else
        {
            Console.WriteLine("Invalid user type.");
            return;
        }

        _users.Add(user);
        Console.WriteLine($"Added user: {user.Id} - {user.FullName} ({user.GetType().Name})");
    }

    private void AddEquipment()
    {
        Console.Write("1-Laptop, 2-Projector, 3-Camera: ");
        string? type = Console.ReadLine();

        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "";

        Equipment equipment;

        switch (type)
        {
            case "1":
                Console.Write("RAM (GB): ");
                int ram = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("CPU model: ");
                string cpu = Console.ReadLine() ?? "";

                equipment = new Laptop(_equipmentId++, name, ram, cpu);
                break;

            case "2":
                Console.Write("Resolution: ");
                string resolution = Console.ReadLine() ?? "";

                Console.Write("Brightness (lumens): ");
                int brightness = int.Parse(Console.ReadLine() ?? "0");

                equipment = new Projector(_equipmentId++, name, resolution, brightness);
                break;

            case "3":
                Console.Write("Sensor type: ");
                string sensor = Console.ReadLine() ?? "";

                Console.Write("Lens mount: ");
                string lensMount = Console.ReadLine() ?? "";

                equipment = new Camera(_equipmentId++, name, sensor, lensMount);
                break;

            default:
                Console.WriteLine("Invalid equipment type.");
                return;
        }

        _equipmentService.AddEquipment(equipment);
        Console.WriteLine($"Added equipment: {equipment.Id} - {equipment.Name} ({equipment.GetType().Name})");
    }

    private void ShowAllEquipment()
    {
        List<Equipment> equipment = _equipmentService.GetAllEquipment();

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

    private void ShowAvailableEquipment()
    {
        List<Equipment> equipment = _equipmentService.GetAvailableEquipment();

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

    private void BorrowEquipment()
    {
        if (_users.Count == 0)
        {
            Console.WriteLine("No users in system.");
            return;
        }

        if (_equipmentService.GetAllEquipment().Count == 0)
        {
            Console.WriteLine("No equipment in system.");
            return;
        }

        Console.WriteLine("Users:");
        foreach (User user in _users)
        {
            Console.WriteLine($"{user.Id} - {user.FullName} ({user.GetType().Name})");
        }

        Console.Write("User id: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        User userToBorrow = _users.FirstOrDefault(u => u.Id == userId)
            ?? throw new InvalidOperationException("User not found.");

        Console.WriteLine("Equipment:");
        foreach (Equipment item in _equipmentService.GetAllEquipment())
        {
            Console.WriteLine($"{item.Id} - {item.Name} ({item.Status})");
        }

        Console.Write("Equipment id: ");
        int equipmentId = int.Parse(Console.ReadLine() ?? "0");

        Equipment equipmentToBorrow = _equipmentService.GetById(equipmentId);

        Console.Write("Days: ");
        int days = int.Parse(Console.ReadLine() ?? "0");

        Loan loan = _loanService.BorrowEquipment(_loanId++, userToBorrow, equipmentToBorrow, days);
        Console.WriteLine($"Loan created: {loan.Id}");
    }

    private void ReturnEquipment()
    {
        List<Loan> loans = _loanService.GetAllLoans();

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

        decimal penalty = _loanService.ReturnEquipment(loanId, returnDate);
        Console.WriteLine($"Equipment returned. Penalty: {penalty}");
    }

    private void MarkUnavailable()
    {
        List<Equipment> equipment = _equipmentService.GetAllEquipment();

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

        _equipmentService.MarkAsUnavailable(id);
        Console.WriteLine("Equipment marked as unavailable.");
    }

    private void ShowUserLoans()
    {
        if (_users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (User user in _users)
        {
            Console.WriteLine($"{user.Id} - {user.FullName}");
        }

        Console.Write("User id: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        List<Loan> loans = _loanService.GetActiveLoansByUser(id);

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

    private void ShowOverdue()
    {
        List<Loan> loans = _loanService.GetOverdueLoans();

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

    private void ShowReport()
    {
        Console.WriteLine(_reportService.GenerateSummary(
            _equipmentService.GetAllEquipment(),
            _users,
            _loanService.GetAllLoans()
        ));
    }

    private void ShowAllUsers()
    {
        if (_users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (User user in _users)
        {
            Console.WriteLine($"{user.Id} - {user.FullName} ({user.GetType().Name})");
        }
    }

    private void ShowStudents()
    {
        List<Student> students = _users.OfType<Student>().ToList();

        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        foreach (Student student in students)
        {
            Console.WriteLine($"{student.Id} - {student.FullName} (limit: {student.MaxActiveLoans})");
        }
    }

    private void ShowEmployees()
    {
        List<Employee> employees = _users.OfType<Employee>().ToList();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (Employee employee in employees)
        {
            Console.WriteLine($"{employee.Id} - {employee.FullName} (limit: {employee.MaxActiveLoans})");
        }
    }
}