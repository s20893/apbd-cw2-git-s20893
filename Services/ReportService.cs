using System.Text;
using Project.Equipments;
using Project.Loans;
using Project.Users;

namespace Project.Services;

public class ReportService
{
    public string GenerateSummary(
        List<Equipment> equipment,
        List<User> users,
        List<Loan> loans)
    {
        int totalEquipment = equipment.Count;
        int availableEquipment = equipment.Count(e => e.Status == EquipmentStatus.Available);
        int borrowedEquipment = equipment.Count(e => e.Status == EquipmentStatus.Borrowed);
        int unavailableEquipment = equipment.Count(e => e.Status == EquipmentStatus.Unavailable);

        int totalUsers = users.Count;
        int activeLoans = loans.Count(l => l.IsActive);
        int overdueLoans = loans.Count(l => l.IsOverdue);
        decimal totalPenalties = loans.Sum(l => l.Penalty);

        StringBuilder sb = new();

        sb.AppendLine("=== RENTAL REPORT ===");
        sb.AppendLine($"Total equipment: {totalEquipment}");
        sb.AppendLine($"Available: {availableEquipment}");
        sb.AppendLine($"Borrowed: {borrowedEquipment}");
        sb.AppendLine($"Unavailable: {unavailableEquipment}");
        sb.AppendLine();
        sb.AppendLine($"Total users: {totalUsers}");
        sb.AppendLine($"Active loans: {activeLoans}");
        sb.AppendLine($"Overdue loans: {overdueLoans}");
        sb.AppendLine($"Total penalties: {totalPenalties}");
        sb.AppendLine("=====================");

        return sb.ToString();
    }
}