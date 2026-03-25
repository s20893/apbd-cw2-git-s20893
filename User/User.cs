namespace CW2.User;

public abstract class User
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public abstract int MaxActiveLoans { get; }

    protected User(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public string FullName => $"{FirstName} {LastName}";
}