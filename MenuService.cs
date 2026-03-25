using Project.Services;

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
            Console.WriteLine("Run your demo scenario here...");
            // tu możesz zostawić swój wcześniejszy kod demo
        }
    }
}