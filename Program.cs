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
        Console.WriteLine("Choose mode:");
        Console.WriteLine("1. Demo (automatic)");
        Console.WriteLine("2. Interactive menu");
        Console.Write("Choice: ");

        string? choice = Console.ReadLine();

        if (choice == "2")
        {
            MenuService menu = new();
            menu.Run();
        }
        else
        {
            RunDemo();
        }
    }

    private static void RunDemo()
    {
        EquipmentService equipmentService = new();
        LoanService loanService = new(new SimplePenaltyCalculator());
        ReportService reportService = new();

        List<User> users = new();

        int equipmentId = 1;
        int userId = 1;
        int loanId = 1;

        Console.WriteLine("\n=== DEMO MODE ===");

        Laptop laptop1 = new(equipmentId++, "Dell XPS", 16, "i7");
        Laptop laptop2 = new(equipmentId++, "Lenovo ThinkPad", 8, "Ryzen 5");
        Projector projector = new(equipmentId++, "Epson", "1080p", 3000);
        Camera camera = new(equipmentId++, "Canon", "APS-C", "EF");

        equipmentService.AddEquipment(laptop1);
        equipmentService.AddEquipment(laptop2);
        equipmentService.AddEquipment(projector);
        equipmentService.AddEquipment(camera);

        Student student = new(userId++, "Jan", "Kowalski");
        Employee employee = new(userId++, "Anna", "Nowak");

        users.Add(student);
        users.Add(employee);

        Console.WriteLine("\n=== ALL EQUIPMENT ===");
        foreach (var e in equipmentService.GetAllEquipment())
        {
            Console.WriteLine($"{e.Id} - {e.Name} ({e.Status})");
        }

        Console.WriteLine("\n=== BORROW ===");
        var loan1 = loanService.BorrowEquipment(loanId++, student, laptop1, 5);
        Console.WriteLine($"Loan created: {loan1.Id}");

        Console.WriteLine("\n=== ERROR TEST ===");
        try
        {
            loanService.BorrowEquipment(loanId++, employee, laptop1, 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\n=== LIMIT TEST ===");
        try
        {
            loanService.BorrowEquipment(loanId++, student, laptop2, 3);
            loanService.BorrowEquipment(loanId++, student, projector, 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\n=== RETURN ON TIME ===");
        decimal penalty1 = loanService.ReturnEquipment(loan1.Id, loan1.DueDate);
        Console.WriteLine($"Penalty: {penalty1}");

        Console.WriteLine("\n=== LATE RETURN ===");
        var loan2 = loanService.BorrowEquipment(loanId++, employee, camera, 2);

        decimal penalty2 = loanService.ReturnEquipment(
            loan2.Id,
            loan2.DueDate.AddDays(3)
        );

        Console.WriteLine($"Penalty: {penalty2}");

        Console.WriteLine("\n=== OVERDUE ===");
        foreach (var l in loanService.GetOverdueLoans())
        {
            Console.WriteLine($"Loan {l.Id} overdue");
        }

        Console.WriteLine("\n=== REPORT ===");
        Console.WriteLine(reportService.GenerateSummary(
            equipmentService.GetAllEquipment(),
            users,
            loanService.GetAllLoans()
        ));
    }
}