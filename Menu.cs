public class Menu
{
  private readonly BlogService_ _blogService;

    public Menu(BlogService_ blogService)
    {
        _blogService = blogService;
    }

    public int GetUserChoice()
    {
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Display all blogs");
        Console.WriteLine("2. Add a new blog");
        Console.WriteLine("3. Create Post");
        Console.WriteLine("4. Display Posts");
        Console.WriteLine("5. Exit");
        Console.Write("Your choice: ");

        string? input = Console.ReadLine();

        if (input?.ToLower() == "exit" || input == "5")
        {
            return 5; // Exit option
        }

        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
            return 0; // Invalid choice
        }
        return choice;
    }
}