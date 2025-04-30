namespace SmartBookApp.Models;

public class User
{
    public string CardId { get; set; }
    public string Name { get; set; }
    public List<Book> LoanedBooks { get; set; } = new();

    public User(string cardId, string name)
    {
        CardId = cardId;
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name} (Card ID: {CardId}) - {LoanedBooks.Count} books loaned";
    }
}