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
}
