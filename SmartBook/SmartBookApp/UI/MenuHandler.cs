using SmartBookApp.Models;
using SmartBookApp.Services;
using System;
using System.Text.Json;
using System.IO;

namespace SmartBookApp.UI;

public class MenuHandler
{
    private readonly Library library = new();
    private readonly InputHelper input = new();
    private readonly InputHelper userInput = new();
    private const string FilePath = "library.json";
    private const string UsersFilePath = "users.json";


    public void MainMenu()
    {
        try
        {
            library.LoadFromFile(FilePath);
            library.LoadUsersFromFile(UsersFilePath);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the SmartBook Library📚!");
            Console.WriteLine("0. Register User");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Remove Book");
            Console.WriteLine("3. Find Book");
            Console.WriteLine("4. Sort Books");
            Console.WriteLine("5. Loan Book");
            Console.WriteLine("6. Return Book");
            Console.WriteLine("7. List All Books");
            Console.WriteLine("8. View User's Borrowed Books");
            Console.WriteLine("9. Export Loaned Books");
            Console.WriteLine("10. Save and exit");
            Console.Write("Please select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    RegisterUser();
                    break;
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    FindBook();
                    break;
                case "4":
                    SortBooks();
                    break;
                case "5":
                    LoanBook();
                    break;
                case "6":
                    ReturnBook();
                    break;
                case "7":
                    ListBooks();
                    break;
                case "8":
                    ViewUserLoans();
                    break;
                case "9":
                    ExportLoanedBooks();
                    break;
                case "10":
                    library.SaveToFile(FilePath);
                    library.SaveUsersToFile(UsersFilePath);
                    Console.WriteLine("Library saved. Goodbye!");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue......");
                Console.ReadKey();
            }
        }
    }

    private void AddBook()
    {
        Console.Clear();
        Console.WriteLine("Add a new book to the library:");

        string title = input.PromptForValidatedInput("Title", s => true, "Title is required.").Trim();
        string author = input.PromptForValidatedInput("Author", s => true, "Author is required.").Trim();
        string isbn = input.PromptForValidatedInput("ISBN", input.IsValidIsbn, "ISBN must be exactly 10 digits.").Trim();
        string genre = input.PromptForValidatedInput("Genre", s => true, "Genre is required.").Trim();

        int? yearPublished = input.PromptForOptionalYear("Year Published");

        try
        {
            var book = new Book(title, author, isbn, genre, yearPublished);
            library.AddBook(book);
            Console.WriteLine("Book added successfully.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }



    private void RemoveBook()
    {
        Console.Clear();
        Console.WriteLine("Remove a book from the library:");

        string identifier = "";
        while (string.IsNullOrWhiteSpace(identifier))
        {
            Console.Write("Enter the title or ISBN of the book to remove: ");
            identifier = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                Console.WriteLine("Input cannot be empty. Please try again.\n");
            }
        }

        try
        {
            library.RemoveBook(identifier);
            Console.WriteLine("Book removed.");

        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void FindBook()
    {
        Console.Clear();
        Console.WriteLine("Find a book in the library:");
        Console.Write("Enter a search term (title or author): ");
        string searchTerm = (Console.ReadLine() ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(searchTerm))
            throw new ArgumentNullException(nameof(searchTerm));
        var results = library.FindBook(searchTerm);

        if (!results.Any())
        {
            Console.WriteLine("No matches found.");
            return;
        }

        Console.WriteLine("\n--- Search results ---");
        foreach (var book in results)
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine(book);
        }

    }

    private void SortBooks()
    {
        Console.Clear();
        Console.WriteLine("Sort books in the library:");
        Console.Write("Enter the sorting criteria (Title, Author, YearPublished): ");
        string sortBy = (Console.ReadLine() ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(sortBy))
            throw new ArgumentNullException(nameof(sortBy));
        if (!Enum.TryParse<Library.SortOption>(sortBy, true, out var sortOption))
        {
            Console.WriteLine("Invalid sorting criteria. Please try again.");
            return;
        }
        var sortedBooks = library.ListBooksSorted(sortOption);

        if (!sortedBooks.Any())
        {
            Console.WriteLine("No books found.");
            return;
        }

        Console.WriteLine("\n--- Sorted Books ---");
        foreach (var book in sortedBooks)
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine(book);
        }
    }

    private void LoanBook()
    {
        Console.Clear();
        Console.WriteLine("Loan a book from the library:");

        User? user = null;
        while (user == null)
        {
            string cardId = userInput.PromptForRequiredInput("Enter the Card ID of the user");
            user = library.FindUserByCardId(cardId);
            if (user == null)
            {
                Console.WriteLine("No user found with that Card ID. Please try again.\n");
            }
        }

        string input = userInput.PromptForRequiredInput("Enter the ISBN, title or author of the book to loan");

        try
        {
            library.LoanBook(input, user.CardId);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ReturnBook()
    {
        Console.Clear();
        Console.WriteLine("Return a book to the library:");

        string input = "";

        while (string.IsNullOrWhiteSpace(input))
        {
            Console.Write("Enter the ISBN, title or author of the book to return: ");
            input = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again.\n");
            }
        }

        try
        {
            library.MarkAsAvailable(input);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void ListBooks()
    {
        Console.Clear();
        Console.WriteLine("List all books in the library:");
        var allBooks = library.ListBooksSorted(Library.SortOption.Title);

        if (!allBooks.Any())
        {
            Console.WriteLine("No books available.");
            return;
        }

        Console.WriteLine("\n--- Books in Library ---");
        foreach (var book in allBooks)
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine(book);
        }

    }

    public void RegisterUser()
    {
        Console.Clear();
        Console.WriteLine("Register a new user:");
        string cardId = input.PromptForValidatedInput("Card ID", s => !string.IsNullOrWhiteSpace(s), "Card ID is required.");
        string name = input.PromptForValidatedInput("Name", s => !string.IsNullOrWhiteSpace(s), "Name is required.");

        try
        {
            library.RegisterUser(new User(cardId.Trim(), name.Trim()));
            Console.WriteLine("User registered successfully.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ViewUserLoans()
    {
        Console.Clear();
        Console.WriteLine("View a user's borrowed books:");


        string cardId = userInput.PromptForRequiredInput("Enter the Card ID of the user");

        var user = library.FindUserByCardId(cardId);

        if (user == null)
        {
            Console.WriteLine("No user found with that Card ID.");
            return;
        }

        Console.WriteLine($"\nUser: {user.Name} (Card ID: {user.CardId})");

        if (!user.LoanedBooks.Any())
        {
            Console.WriteLine($"\n{user.Name} has no borrowed books.");
            return;
        }

        Console.WriteLine("\n Borrowed Books:");
        foreach (var book in user.LoanedBooks)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(book);
        }
    }

    private void ExportLoanedBooks()
    {
        const string reportPath = "loaned_books.txt";

        try
        {
            library.ExportLoanedBooks(reportPath);
            Console.WriteLine($"The report has been saved to '{reportPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during export: {ex.Message}");
        }
    }

}
