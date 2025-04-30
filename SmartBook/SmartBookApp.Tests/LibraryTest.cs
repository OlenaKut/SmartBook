using System;
using Xunit;

using SmartBookApp.Models;
using SmartBookApp.Services;

namespace SmartBookApp.Tests;

public class LibraryTest
{
    [Fact]
    public void AddBook_ShouldAddBook_ToList()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");

        // Act
        library.AddBook(book);

        // Assert
        Assert.Contains(book, library.Books);

    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("Test Title")]
    public void RemoveBook_ShouldRemoveBook_FromListByTitleOrISBN(string identifier)
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");

        // Act
        library.AddBook(book);
        library.RemoveBook(identifier);

        // Assert
        Assert.DoesNotContain(book, library.Books);
    }


    [Fact(Skip = "This test is temporarily disabled.")]
    public void FindBook_ShouldFindBook_InListByTitleOrAuthor()
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

    [Theory]
    [InlineData("Test Title")]
    [InlineData("Test Author")]
    [InlineData("1234567890")]
    public void LoanBook_ShouldMarkBookAsLoaned_ByTitleOrAuthorOrISBN(string identifier)
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");
        library.AddBook(book);

        // Act
        library.LoanBook(identifier);

        // Assert
        Assert.True(book.IsLoaned);
    }

    [Fact]
    public void MarkAsAvailable_ShouldThrowIfNotLoaned_ByTitleOrAuthorOrISBN()
    {
        // Arrange
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", "Fiction");

        // Act
        library.AddBook(book);

        //Assert
        var ex = Assert.Throws<ArgumentException>(() => library.MarkAsAvailable("1234567890"));
        Assert.Equal("Book is not currently loaned.", ex.Message);
    }


}
