## Project Structure

The solution contains two main projects: 
`SmartBookApp`
Contains the core application logic and console interface. It's organized into the following folders:
 * `Models` – Contains the Book class representing book data.
 * `Services` – Contains the Library class, which handles business logic like adding, removing, searching, and loaning books.
 * `UI` – Contains the MenuHandler and InputHelper classes, which handle user interaction and validated input.

`SmartBookApp.Tests`
A separate project for unit testing. It contains:  
* `LibraryTests.cs` – xUnit tests that verify the behavior of the Library class.
-------------------------------------------------------------------------------

`Program.cs` – The entry point of the application, which starts the console interface.

-------------------------------------------------------------------------------

`Models` folder contains `Book.cs` class that  a single book in the library. It includes the following properties:
- `Title` (string) – The title of the book.
- `Author` (string) – The name of the author.
- `ISBN` (string) – A unique 10-digit identifier for the book.
- `Category` (string) – The genre or category of the book.
- `YearPublished` (int?, optional) – The year the book was published.

**Behavior**:
- Two books are considered equal if they have the same `ISBN`.
- The `ToString()` method outputs a readable summary of the book, including its loan status.
- The `Equals()` method checks if two Book objects have the same ISBN, meaning they represent the same book.
- The `GetHashCode()` method ensures this also works correctly in collections like HashSet<Book> or dictionaries.

-------------------------------------------------------------------------------

`Services` folder contains the core application logic, handled by the Library class.
`Library.cs` handles the main logic of the application. It works with a list of books and supports the following operations:
- `AddBook` – Adds a new book (checks for duplicate ISBN).
- `RemoveBook` – Removes a book by title or ISBN.
- `FindBook` – Searches for books by title or author.
- `ListBooksSorted` – Returns the list of books sorted by title, author, or year.
- `LoanBook` – Marks a book as loaned. Can be found by title, author, or ISBN.
- `MarkAsAvailable` – Marks a loaned book as returned.
- `SaveToFile` / `LoadFromFile` – Saves and loads the book list from a JSON file.

-------------------------------------------------------------------------------

The `UI` folder contains the console interface of the application.
`LibraryApp` class is the starting point of the application. It simply runs the main menu by calling `menuHandler.MainMenu()`.
`MenuHandler` – Shows the menu, handles user choices, and connects actions like adding, removing, or loaning books.
`InputHelper` – Handles user input and validation (e.g., making sure ISBN is 10 digits or title is not empty).
Together, these classes make it easy for users to interact with the library from the console.

-------------------------------------------------------------------------------

## Techniques and Features Used
This project uses several important C# features and best practices:
**List<Book>** – Used to store and manage all books in memory.
**try/catch blocks** – Used to handle errors gracefully, such as when trying to remove a book that doesn’t exist.
**LINQ** – Used for searching and sorting books by title, author, or year (OrderBy, Where, FirstOrDefault).
**JSON** – Used to save and load the book list to/from a .json file.
**Input validation** – Ensures that required fields (like title and ISBN) are not empty and that ISBN is exactly 10 digits.
**ArgumentException** – Thrown when invalid operations are attempted, such as adding a duplicate book or loaning an already loaned book.

-------------------------------------------------------------------------------

## How to Run the Application (If you use a Mac like me)

1. Open your terminal and navigate to the project folder `cd SmartBook/SmartBookApp`
2. Run the application using the .NET CLI `dotnet run`
3. Shoose an option from the Menu and enter a number: 

Welcome to the SmartBook Library📚!
1. Add Book  --- You can add a new book to collection. Make sure you are entering a valid data. The app saves all changes to a file named `library.json`.
2. Remove Book --- You can remove a book using title or ISBN.
3. Find Book --- SYou can searche books by title or author and get results like a list.
4. Sort Books --- You can sort books by title, author, or year.
5. Loan Book --- You can loan a book by searching by ISBN, title or author. If there is more than one book with the same title or author, you need to additionally enter an ISBN.
6. Return Book --- You can return a book by searching by ISBN, title or author. If there is more than one book with the same title or author, you need to additionally enter an ISBN.
7. List All Books --- You can see a list of all books in the collection.
8. Exit

-------------------------------------------------------------------------------

## Unit Testing

The `SmartBookApp.Tests` project contains automated unit tests for the Library class using **xUnit**.
These tests check that the main features of the library system work correctly, such as adding, removing, loaning, and returning books.

****What’s Tested:**
`AddBook_ShouldAddBook_ToList` 
Verifies that a new book is added to the library list.

`RemoveBook_ShouldRemoveBook_FromListByTitleOrISBN`
Confirms that a book can be removed using either its title or ISBN.

`FindBook_ShouldFindBook_InListByTitleOrAuthor`
(Temporarily skipped) Tests searching books by title or author.

`LoanBook_ShouldMarkBookAsLoaned_ByTitleOrAuthorOrISBN`
Checks that a book is correctly marked as loaned using any valid identifier.

`MarkAsAvailable_ShouldThrowIfNotLoaned_ByTitleOrAuthorOrISBN`
Ensures an exception is thrown if trying to return a book that wasn’t loaned.


## How to Run the Tests  (If you use a Mac like me)

1. Open your terminal and navigate to the project folder `cd SmartBook/SmartBookApp.Tests`
2. Run the test  project using the .NET CLI `dotnet test`
3. You’ll see a summary like: Test summary: total: 8, failed: 0, succeeded: 7, skipped: 1, duration: 0.5s

## Summary 
This project demonstrates a simple but complete C# console application for managing a library system. It includes:
 - A user-friendly menu for adding, finding, sorting, and managing books.
 - Proper input validation and exception handling.
 - Data saving using JSON file storage.
 - A clear code structure with separation of logic (Models, Services, UI).
 - Unit tests using xUnit to ensure the core logic works as expected.