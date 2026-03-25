using CW2.Equipment;
using CW2.User;
using CW2.Rules;

namespace CW2.Loan;

public class LoanService
{
    private readonly List<Loan> _loans = new();
    private readonly IPenaltyCalculator _penaltyCalculator;

    public LoanService(IPenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator;
    }

    public Loan BorrowEquipment(int loanId, User user, Equipment equipment, int days)
    {
        // 1. sprawdź dostępność
        if (equipment.Status != EquipmentStatus.Available)
        {
            throw new InvalidOperationException("Equipment not available.");
        }

        // 2. sprawdź limit użytkownika
        int activeLoans = _loans.Count(l => l.User.Id == user.Id && l.IsActive);

        if (activeLoans >= user.MaxActiveLoans)
        {
            throw new InvalidOperationException("User exceeded loan limit.");
        }

        // 3. utwórz wypożyczenie
        DateTime now = DateTime.Now;
        DateTime due = now.AddDays(days);

        Loan loan = new Loan(loanId, user, equipment, now, due);

        // 4. zmień status sprzętu
        equipment.MarkAsBorrowed();

        // 5. zapisz
        _loans.Add(loan);

        return loan;
    }

    public decimal ReturnEquipment(int loanId, DateTime returnDate)
    {
        Loan loan = _loans.FirstOrDefault(l => l.Id == loanId)
            ?? throw new InvalidOperationException("Loan not found.");

        if (!loan.IsActive)
        {
            throw new InvalidOperationException("Loan already closed.");
        }

        // kara
        decimal penalty = _penaltyCalculator.CalculatePenalty(loan.DueDate, returnDate);

        // zamknięcie
        loan.Close(returnDate, penalty);

        // sprzęt wraca
        loan.Equipment.MarkAsAvailable();

        return penalty;
    }

    public List<Loan> GetActiveLoansByUser(int userId)
    {
        return _loans
            .Where(l => l.User.Id == userId && l.IsActive)
            .ToList();
    }

    public List<Loan> GetOverdueLoans()
    {
        return _loans
            .Where(l => l.IsOverdue)
            .ToList();
    }

    public List<Loan> GetAllLoans()
    {
        return _loans;
    }
}