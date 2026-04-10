﻿using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

var db = new DataContext();
var blogService = new BlogService(db, logger);
var menu = new Menu(blogService);

bool running = true;

while (running)
{
    int choice = menu.GetUserChoice();

    switch (choice)
    {
        case 1:
            blogService.DisplayBlogs();
            break;
        case 2:
            blogService.AddBlog();
            break;
        case 3:
            blogService.CreatePost();
            break;
        case 4:
            blogService.DisplayPosts();
            break;
        case 5:
            running = false;
            Console.WriteLine("Exiting the program. Goodbye!");
            logger.Info("Program exited");
            break;
        default:
            // This case will never be hit due to validation in GetUserChoice, but it's here for completeness.
            Console.WriteLine("Invalid choice. Please select a number between 1 and 5.");
            break;
    }
}

logger.Info("Program ended");
