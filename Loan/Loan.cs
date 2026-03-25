using CW2.User;
using CW2.Equipment;

namespace CW2.Loan;

public class Loan
{
    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }

    public DateTime BorrowDate { get; }
    public DateTime DueDate { get; }

    public DateTime? ReturnDate { get; private set; }
    public decimal Penalty { get; private set; }

    public Loan(int id, User user, Equipment equipment, DateTime borrowDate, DateTime dueDate)
    {
        Id = id;
        User = user;
        Equipment = equipment;
        BorrowDate = borrowDate;
        DueDate = dueDate;
    }

    public bool IsActive => ReturnDate == null;

    public bool IsOverdue =>
        IsActive && DateTime.Now.Date > DueDate.Date;

    public void Close(DateTime returnDate, decimal penalty)
    {
        if (ReturnDate != null)
        {
            throw new InvalidOperationException("Loan already closed.");
        }

        ReturnDate = returnDate;
        Penalty = penalty;
    }
}