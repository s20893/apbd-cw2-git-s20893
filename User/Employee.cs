namespace CW2.User;

public class Employee : User
{
    public Employee(int id, string firstName, string lastName)
        : base(id, firstName, lastName)
    {
    }

    public override int MaxActiveLoans => 5;
}