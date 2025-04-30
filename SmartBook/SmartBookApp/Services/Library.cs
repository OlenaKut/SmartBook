using System;
using System.Text.Json;
using System.IO;
using SmartBookApp.Models;

namespace SmartBookApp.Services;


public class Library
{

    public List<User> Users { get; set; } = new();

    public void RegisterUser(User user)
    {
        if (Users.Any(u => u.CardId == user.CardId))
            throw new ArgumentException("A user with this card ID already exists.");
        Users.Add(user);
    }

    public User? FindUserByCardId(string cardId)
    {
        return Users.FirstOrDefault(u => u.CardId == cardId);
    }
    public List<Book> Books { get; set; } = new();

    public void SaveUsersToFile(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(Users, options);
        File.WriteAllText(filePath, json);
    }

    public void LoadUsersFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            Users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        else
        {
            Users = new List<User>();
        }
    }


    public void AddBook(Book book)
    {
        if (Books.Any(b => b.ISBN == book.ISBN))
        {
            throw new ArgumentException("A book with the same ISBN already exists in the library.");
        }
        Books.Add(book);
    }

    public void RemoveBook(string identifier)
    {
        var bookToRemove = Books.FirstOrDefault(b =>
            b.ISBN.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
            b.Title.Equals(identifier, StringComparison.OrdinalIgnoreCase));

        if (bookToRemove != null)
        {
            Books.Remove(bookToRemove);
            foreach (var user in Users)
            {
                if (user.LoanedBooks.Contains(bookToRemove))
                {
                    user.LoanedBooks.Remove(bookToRemove);
                    break;
                }
            }
        }
        else
        {
            throw new ArgumentException("Book not found by ISBN or title.");
        }
    }


    public List<Book> FindBook(string searchTerm)
    {
        return Books
            .Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }


    public enum SortOption { Title, Author, YearPublished }

    public List<Book> ListBooksSorted(SortOption sortBy)
    {
        return sortBy switch
        {
            SortOption.Title => Books.OrderBy(b => b.Title).ToList(),
            SortOption.Author => Books.OrderBy(b => b.Author).ToList(),
            SortOption.YearPublished => Books.OrderBy(b => b.YearPublished).ToList(),
            _ => Books
        };
    }

    public void LoanBook(string identifier, string cardId)
    {

        var user = FindUserByCardId(cardId);
        if (user == null)
            throw new ArgumentException("User not found.");

        var matches = Books
               .Where(b =>
                   b.ISBN.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
                   b.Title.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
                   b.Author.Equals(identifier, StringComparison.OrdinalIgnoreCase))
               .ToList();

        if (matches.Count == 0)
        {
            throw new ArgumentException("No matching book found.");
        }

        while (matches.Count > 1)
        {
            Console.WriteLine("Multiple books match the search criteria:");
            foreach (var b in matches)
            {
                Console.WriteLine($"- {b.Title} by {b.Author} (ISBN: {b.ISBN})");
            }

            Console.Write("Please enter the ISBN of the book you want to loan: ");
            string? isbnInput = Console.ReadLine();

            matches = Books
                .Where(b => b.ISBN.Equals(isbnInput, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("No book found with that ISBN.");
                return;
            }
        }
        var book = matches.First();
        if (book.IsLoaned)
        {
            throw new ArgumentException("Book is already loaned.");
        }

        book.IsLoaned = true;
        user.LoanedBooks.Add(book);
        Console.WriteLine($"Book loaned successfully to {user.Name} (Card ID: {user.CardId}).");

    }




    public void MarkAsAvailable(string identifier)
    {
        var matches = Books
      .Where(b =>
          b.ISBN.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
          b.Title.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
          b.Author.Equals(identifier, StringComparison.OrdinalIgnoreCase))
      .ToList();


        if (matches.Count == 0)
        {
            throw new ArgumentException("No matching book found.");
        }

        while (matches.Count > 1)
        {
            Console.WriteLine("Multiple books match the search criteria:");
            foreach (var b in matches)
            {
                Console.WriteLine($"- {b.Title} by {b.Author} (ISBN: {b.ISBN})");
            }

            Console.Write("Please enter the ISBN of the book you want to return: ");
            string? isbnInput = Console.ReadLine();

            matches = Books
                .Where(b => b.ISBN.Equals(isbnInput, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("No book found with that ISBN.");
                return;
            }
        }

        var book = matches.First();
        if (!book.IsLoaned)
        {
            throw new ArgumentException("Book is not currently loaned.");
        }

        book.IsLoaned = false;

        foreach (var user in Users)
        {
            if (user.LoanedBooks.Contains(book))
            {
                user.LoanedBooks.Remove(book);
                break;
            }
        }


        Console.WriteLine("Book returned successfully.");
    }

    public void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(Books, options);
        File.WriteAllText(filePath, json);
    }
    public void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            Books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }
        else
        {
            throw new FileNotFoundException("The specified file was not found.");
        }
    }

}