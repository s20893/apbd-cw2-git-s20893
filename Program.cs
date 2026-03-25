using Project.Equipment;
using Project.User;
using Project.Loan;
using Project.Services;
using Project.Rules;

namespace Project;

public class Program
{
    public static void Main()
    {
        // SERVICES
        EquipmentService equipmentService = new();
        LoanService loanService = new(new SimplePenaltyCalculator());
        ReportService reportService = new();

        // LISTA USERÓW (prosto)
        List<User.User> users = new();

        int equipmentId = 1;
        int userId = 1;
        int loanId = 1;

        // =========================
        // 1. DODANIE SPRZĘTU
        // =========================

        Laptop laptop1 = new(equipmentId++, "Dell XPS", 16, "i7");
        Laptop laptop2 = new(equipmentId++, "Lenovo ThinkPad", 8, "Ryzen 5");
        Projector projector = new(equipmentId++, "Epson", "1080p", 3000);
        Camera camera = new(equipmentId++, "Canon", "APS-C", "EF");

        equipmentService.AddEquipment(laptop1);
        equipmentService.AddEquipment(laptop2);
        equipmentService.AddEquipment(projector);
        equipmentService.AddEquipment(camera);

        // =========================
        // 2. DODANIE USERÓW
        // =========================

        Student student = new(userId++, "Jan", "Kowalski");
        Employee employee = new(userId++, "Anna", "Nowak");

        users.Add(student);
        users.Add(employee);

        // =========================
        // 3. LISTA SPRZĘTU
        // =========================

        Console.WriteLine("=== ALL EQUIPMENT ===");
        foreach (var e in equipmentService.GetAllEquipment())
        {
            Console.WriteLine($"{e.Id} - {e.Name} ({e.Status})");
        }

        // =========================
        // 4. POPRAWNE WYPOŻYCZENIE
        // =========================

        Console.WriteLine("\n=== BORROW ===");
        var loan1 = loanService.BorrowEquipment(loanId++, student, laptop1, 5);
        Console.WriteLine($"Loan created: {loan1.Id}");

        // =========================
        // 5. BŁĄD (sprzęt zajęty)
        // =========================

        Console.WriteLine("\n=== ERROR TEST ===");
        try
        {
            loanService.BorrowEquipment(loanId++, employee, laptop1, 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // =========================
        // 6. LIMIT STUDENTA
        // =========================

        Console.WriteLine("\n=== LIMIT TEST ===");
        try
        {
            loanService.BorrowEquipment(loanId++, student, laptop2, 3);
            loanService.BorrowEquipment(loanId++, student, projector, 3); // powinien wywalić błąd
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // =========================
        // 7. ZWROT W TERMINIE
        // =========================

        Console.WriteLine("\n=== RETURN ON TIME ===");
        decimal penalty1 = loanService.ReturnEquipment(loan1.Id, loan1.DueDate);
        Console.WriteLine($"Penalty: {penalty1}");

        // =========================
        // 8. ZWROT PO TERMINIE
        // =========================

        Console.WriteLine("\n=== LATE RETURN ===");
        var loan2 = loanService.BorrowEquipment(loanId++, employee, camera, 2);

        decimal penalty2 = loanService.ReturnEquipment(
            loan2.Id,
            loan2.DueDate.AddDays(3)
        );

        Console.WriteLine($"Penalty: {penalty2}");

        // =========================
        // 9. PRZETERMINOWANE
        // =========================

        Console.WriteLine("\n=== OVERDUE ===");
        foreach (var l in loanService.GetOverdueLoans())
        {
            Console.WriteLine($"Loan {l.Id} overdue");
        }

        // =========================
        // 10. RAPORT
        // =========================

        Console.WriteLine("\n=== REPORT ===");
        Console.WriteLine(reportService.GenerateSummary(
            equipmentService.GetAllEquipment(),
            users,
            loanService.GetAllLoans()
        ));
    }
}