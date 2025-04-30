namespace SmartBookApp.Models;
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Genre { get; set; }
    public bool IsLoaned { get; set; } = false;
    public int? YearPublished { get; set; }


    public Book(string title, string author, string isbn, string genre, int? yearPublished = null)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        Genre = genre;
        YearPublished = yearPublished;
    }

    public override string ToString()
    {
        string status = IsLoaned ? "Lend" : "Available";
        return $"{Title} by {Author} (ISBN: {ISBN}) - Genre: {Genre}, Year Published: {YearPublished}. Status: {status}";
    }
    public override bool Equals(object? obj)
    {
        return obj is Book book && ISBN == book.ISBN;
    }

    public override int GetHashCode()
    {
        return ISBN.GetHashCode();
    }
}