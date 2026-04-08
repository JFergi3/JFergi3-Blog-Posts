using NLog;

public class BlogService
{
   private readonly DataContext _db;
   private readonly Logger _logger;

   public BlogService(DataContext db, Logger logger)
   {
      _db = db;
      _logger = logger;
   }

   public void DisplayBlogs()
    {
        var blogs = _db.Blogs.OrderBy(b => b.Name).ToList();

        if (!blogs.Any())
        {
            Console.WriteLine("No blogs found.");
        }
        else
        {
            Console.WriteLine("All blogs in the database:");
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.Name);
            }
        }

        _logger.Info("Displayed all blogs");

    }

    public void AddBlog()
    {
        Console.WriteLine("Enter a name for a new Blog: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Blog name cannot be empty.");
            return;
        }

        var blog = new Blog { Name = name };
        _db.AddBlog(blog);

        Console.WriteLine($"Blog '{name}' added successfully.");
        _logger.Info("Blog added - {name}", name);
    }

    public void CreatePost()
    {
        var blogs = GetBlogsOrWarn();
        if (blogs == null) return;

        var selectedBlog = SelectBlog(blogs, "Select a blog to add a post to:");
        if (selectedBlog == null)
        {
            Console.WriteLine("Invalid selection. Returning to menu.");
            return;
        }

        Console.WriteLine("Enter the title of the post: ");
        string? title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))        {
            Console.WriteLine("Post title cannot be empty.");
            return;
        }

        Console.WriteLine("Enter the content of the post: ");
        string? content = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("Post content cannot be empty.");
            return;
        }

        var post = new Post
        {
            Title = title,
            Content = content,
            BlogId = selectedBlog.BlogId
        };

        _db.Posts.Add(post);
        _db.SaveChanges();

        Console.WriteLine($"Post '{title}' added to blog '{selectedBlog.Name}' successfully.");
        _logger.Info("Post added - {title} to blog {blogName}", title, selectedBlog.Name);
    }

    public void DisplayPosts()
    {
        var blogs = GetBlogsOrWarn();
        if (blogs == null) return;

        var selectedBlog = SelectBlog(blogs, "Select a blog to view its posts:");
        if (selectedBlog == null)
        {
            Console.WriteLine("Invalid selection. Returning to menu.");
            return;
        }

        var posts = _db.Posts.Where(p => p.BlogId == selectedBlog.BlogId).ToList();

        if (!posts.Any())
        {
            Console.WriteLine($"No posts found for blog '{selectedBlog.Name}'.");
        }
        else
        {
            Console.WriteLine($"Posts for blog '{selectedBlog.Name}':");
            foreach (var p in posts)
            {
                Console.WriteLine($"\n Blog: {selectedBlog.Name}");
                Console.WriteLine($"    Title: {p.Title}");
                Console.WriteLine($"    Content: {p.Content}");
            }
        }

        _logger.Info("Displayed posts for blog {blogName}", selectedBlog.Name);
    }

    private Blog? SelectBlog(List<Blog> blogs, string prompt)
    {
        Console.WriteLine(prompt);
        for (int i = 0; i < blogs.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {blogs[i].Name}");
        }
        Console.Write("Your choice: ");

        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int choice) || choice < 1 || choice > blogs.Count)
        {
            return null; // Invalid choice
        }
        return blogs[choice - 1]; // Return zero-based index
    }

    private List<Blog>? GetBlogsOrWarn()
    {
        var blogs = _db.Blogs.OrderBy(b => b.Name).ToList();
        if (!blogs.Any())
        {
            Console.WriteLine("No blogs found. Please add a blog first.");
            return null;
        }
        return blogs;
    }

}