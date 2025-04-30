using SmartBookApp.Models;
using SmartBookApp.Services;
using System;
using System.Text.Json;
using System.IO;

namespace SmartBookApp.UI;

public class LibraryApp
{
    private readonly Library library = new();
    private const string FilePath = "library.json";
    public void Run()
    {
        try
        {
            library.LoadFromFile(FilePath);
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
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Remove Book");
            Console.WriteLine("3. Find Book");
            Console.WriteLine("4. Sort Books");
            Console.WriteLine("5. Loan Book");
            Console.WriteLine("6. Return Book");
            Console.WriteLine("7. List All Books");
            Console.WriteLine("8. Exit");
            Console.Write("Please select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
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
                    library.SaveToFile(FilePath);
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

    private string PromptForValidatedInput(string prompt, Func<string, bool> isValid, string errorMessage)
    {
        string? input;
        do
        {
            Console.Write($"{prompt}: ");
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || !isValid(input))
            {
                Console.WriteLine(errorMessage);
                input = null;
            }

        } while (input == null);

        return input;
    }
    private void AddBook()
    {
        Console.Clear();
        Console.WriteLine("Add a new book to the library:");

        string title = PromptForValidatedInput("Title", s => true, "Title is required.");
        string author = PromptForValidatedInput("Author", s => true, "Author is required.");
        string isbn = PromptForValidatedInput("ISBN", IsValidIsbn, "ISBN must be exactly 10 digits.");
        string genre = PromptForValidatedInput("Genre", s => true, "Genre is required.");

        int? yearPublished = PromptForOptionalYear("Year Published");

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
    private bool IsValidIsbn(string isbn)
    {
        return isbn.All(char.IsDigit) && isbn.Length == 10;
    }
    private int? PromptForOptionalYear(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (optional): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return null; // User skipped year

            if (int.TryParse(input, out int parsedYear))
            {
                if (parsedYear <= 2025 && parsedYear >= 0)
                {
                    return parsedYear;
                }
                else
                {
                    Console.WriteLine("Year must be a number less than or equal to 2025.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric year.");
            }
        }
    }


    private void RemoveBook()
    {
        Console.Clear();
        Console.WriteLine("Remove a book from the library:");
        Console.Write("Enter the title or ISBN of the book to remove: ");
        string identifier = Console.ReadLine() ?? string.Empty;

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
        string searchTerm = Console.ReadLine() ?? throw new ArgumentNullException(nameof(searchTerm));
        var results = library.FindBook(searchTerm);

        if (!results.Any())
        {
            Console.WriteLine("No matches found.");
            return;
        }

        Console.WriteLine("\n--- Search results ---");
        foreach (var book in results)
            Console.WriteLine(book);
    }

    private void SortBooks()
    {
        Console.Clear();
        Console.WriteLine("Sort books in the library:");
        Console.Write("Enter the sorting criteria (Title, Author, YearPublished): ");
        string sortBy = Console.ReadLine() ?? throw new ArgumentNullException(nameof(sortBy));
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
            Console.WriteLine(book);
    }

    private void LoanBook()
    {
        Console.Clear();
        Console.WriteLine("Loan a book from the library:");
        Console.Write("Enter the ISBN, title or author of the book to loan: ");
        string isbn = Console.ReadLine() ?? throw new ArgumentNullException(nameof(isbn));

        try
        {
            library.LoanBook(isbn);
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
        Console.Write("Enter the ISBN, title or author of the book to return: ");
        string isbn = Console.ReadLine() ?? throw new ArgumentNullException(nameof(isbn));

        try
        {
            library.MarkAsAvailable(isbn);
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
            Console.WriteLine(book);
    }

}