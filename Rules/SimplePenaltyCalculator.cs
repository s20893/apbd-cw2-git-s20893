namespace CW2.Rules;

public class SimplePenaltyCalculator : IPenaltyCalculator
{
    private const decimal DailyRate = 10m;

    public decimal CalculatePenalty(DateTime dueDate, DateTime returnDate)
    {
        int days = (returnDate.Date - dueDate.Date).Days;

        if (days <= 0)
            return 0;

        return days * DailyRate;
    }
}