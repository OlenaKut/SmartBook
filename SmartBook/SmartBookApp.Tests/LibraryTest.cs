using System;
using Xunit;

using SmartBookApp.Models;
using SmartBookApp.Services;

namespace SmartBookApp.Tests;

public class LibraryTest
{
    [Fact]
    public void AddBook_ShouldAddBookToList()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");

        // Act
        library.AddBook(book);

        // Assert
        Assert.Contains(book, library.Books);

    }

    [Fact]
    public void RemoveBook_ShouldRemoveBookFromList()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");
        library.AddBook(book);

        // Act
        library.RemoveBook("1234567890");

        // Assert
        Assert.DoesNotContain(book, library.Books);

    }

    [Fact]
    public void FindBook_ShouldFindBookInList()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");
        library.AddBook(book);

        // Act
        var result = library.FindBook("Olena");

        // Assert
        Assert.DoesNotContain(book, result);

    }

    [Fact]
    public void LoanBook_ShouldMarkBookAsLoaned()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");
        library.AddBook(book);

        // Act
        library.LoanBook("1234567890");

        // Assert
        Assert.True(book.IsLoaned);
    }

    [Fact]
    public void MarkAsAvailable_ShouldThrowIfNotLoaned()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");
        library.AddBook(book);


        var ex = Assert.Throws<ArgumentException>(() => library.MarkAsAvailable("1234567890"));
        Assert.Equal("Book is not currently loaned.", ex.Message);
    }
}
