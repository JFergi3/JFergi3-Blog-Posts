using NLog;

public class App
{
    private readonly BlogService _blogService;
    private readonly Menu _menu;
    private readonly Logger _logger;

    public App()
    {
        string path = Directory.GetCurrentDirectory() + "//nlog.config";
        _logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
        
        var db = new DataContext();
        _blogService = new BlogService(db, _logger);
        _menu = new Menu(_blogService);
    }

    public void Run()
    {
        _logger.Info("Program started");
        bool running = true;

        while (running)
        {
            int choice = _menu.GetUserChoice();

            switch (choice)
            {
                case 1:
                    _blogService.DisplayBlogs();
                    break;
                case 2:
                    _blogService.AddBlog();
                    break;
                case 3:
                    _blogService.CreatePost();
                    break;
                case 4:
                    _blogService.DisplayPosts();
                    break;
                case 5:
                    running = false;
                    Console.WriteLine("Exiting the program. Goodbye!");
                    _logger.Info("Program exited");
                    break;
                default:
                    // This case will never be hit due to validation in GetUserChoice, but it's here for completeness.
                    Console.WriteLine("Invalid choice. Please select a number between 1 and 5.");
                    break;
            }
        }

        _logger.Info("Program ended");
    }
}