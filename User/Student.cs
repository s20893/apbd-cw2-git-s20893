namespace Project.Users;

public class Student : User
{
    public Student(int id, string firstName, string lastName)
        : base(id, firstName, lastName)
    {
    }

    public override int MaxActiveLoans => 2;
}