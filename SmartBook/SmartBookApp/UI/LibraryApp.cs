using SmartBookApp.Models;
using SmartBookApp.Services;
using System;
using System.Text.Json;
using System.IO;

namespace SmartBookApp.UI;

public class LibraryApp
{
    private readonly MenuHandler menuHandler = new();
    public void Run()
    {
        menuHandler.MainMenu();
    }
}



