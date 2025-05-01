using SmartBookApp.Models;
using SmartBookApp.Services;
using System;
using System.Text.Json;
using System.IO;

namespace SmartBookApp.UI;

public class InputHelper
{
    public string PromptForValidatedInput(string prompt, Func<string, bool> isValid, string errorMessage)
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


    public bool IsValidIsbn(string isbn)
    {
        return isbn.All(char.IsDigit) && isbn.Length == 10;
    }

    public int? PromptForOptionalYear(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (optional): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return null;

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

    public string PromptForRequiredInput(string prompt)
    {
        string userInput = "";

        while (string.IsNullOrWhiteSpace(userInput))
        {
            Console.Write($"{prompt}: ");
            userInput = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine($"{prompt} is required. Please try again.\n");
            }
        }

        return userInput;
    }
}