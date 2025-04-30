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

    private void AddBook()
    {
        Console.Clear();
        Console.WriteLine("Add a new book to the library:");
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? throw new ArgumentNullException(nameof(title));
        Console.Write("Author: ");
        string author = Console.ReadLine() ?? throw new ArgumentNullException(nameof(author));
        Console.Write("ISBN: ");
        string isbn = Console.ReadLine() ?? throw new ArgumentNullException(nameof(isbn));
        Console.Write("Genre: ");
        string genre = Console.ReadLine() ?? throw new ArgumentNullException(nameof(genre));
        Console.Write("Year Published (optional): ");
        int? yearPublished = int.TryParse(Console.ReadLine(), out var year) ? year : null;

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
        Console.Write("Enter the ISBN of the book to loan: ");
        string isbn = Console.ReadLine() ?? throw new ArgumentNullException(nameof(isbn));

        try
        {
            library.LoanBook(isbn);
            Console.WriteLine("Book loaned successfully.");
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
        Console.Write("Enter the ISBN of the book to return: ");
        string isbn = Console.ReadLine() ?? throw new ArgumentNullException(nameof(isbn));

        try
        {
            library.MarkAsAvailable(isbn);
            Console.WriteLine("Book returned successfully.");
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