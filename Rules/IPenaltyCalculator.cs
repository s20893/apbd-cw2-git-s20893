namespace CW2.Rules;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(DateTime dueDate, DateTime returnDate);
}