using System;
using System.Collections.Generic;

class Program
{
    // Dictionary to store grammar rules
    static Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("Recursive Descent Parser");
            Console.WriteLine("1 - Define Grammar");
            Console.WriteLine("2 - Test String");
            Console.WriteLine("3 - Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DefineGrammar();
                    break;
                case 2:
                    if (grammar.Count == 0)
                    {
                        Console.WriteLine("Please define the grammar first!");
                    }
                    else
                    {
                        TestString();
                    }
                    break;
                case 3:
                    Console.WriteLine("Exiting program.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    // Method to define grammar rules
    static void DefineGrammar()
    {
        grammar.Clear();
        Console.WriteLine("Enter grammar rules:");
        Console.WriteLine("Example: For non-terminal 'S' with rules 'aSb' and 'b', enter 'S:aSb|b'");

        while (true)
        {
            Console.Write("Enter rule (or type 'done' to finish): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "done")
                break;

            string[] parts = input.Split(':');
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid format. Try again.");
                continue;
            }

            string nonTerminal = parts[0].Trim();
            string[] rules = parts[1].Split('|');

            if (!grammar.ContainsKey(nonTerminal))
            {
                grammar[nonTerminal] = new List<string>();
            }

            foreach (string rule in rules)
            {
                grammar[nonTerminal].Add(rule.Trim());
            }
        }

        if (IsSimpleGrammar())
        {
            Console.WriteLine("The grammar is simple.");
        }
        else
        {
            Console.WriteLine("The grammar is not simple. Please redefine it.");
            grammar.Clear();
        }
    }

    // Method to check if the grammar is simple
    static bool IsSimpleGrammar()
    {
        foreach (var entry in grammar)
        {
            foreach (string rule in entry.Value)
            {
                foreach (char symbol in rule)
                {
                    if (char.IsUpper(symbol) && symbol != entry.Key[0])
                    {
                        return false; // Found a non-terminal other than the current key
                    }
                }
            }
        }
        return true;
    }

    // Method to test if a string is accepted by the grammar
    static void TestString()
    {
        Console.Write("Enter the string to be checked: ");
        string input = Console.ReadLine();
        int index = 0;

        if (Parse("S", input, ref index) && index == input.Length)
        {
            Console.WriteLine("The input string is Accepted.");
        }
        else
        {
            Console.WriteLine("The input string is Rejected.");
        }
    }

    // Recursive method to parse the input string
    static bool Parse(string nonTerminal, string input, ref int index)
    {
        if (!grammar.ContainsKey(nonTerminal))
        {
            return false; // Non-terminal not found in grammar
        }

        foreach (string rule in grammar[nonTerminal])
        {
            int currentIndex = index;
            bool match = true;

            foreach (char symbol in rule)
            {
                if (char.IsUpper(symbol))
                {
                    // Recursively parse non-terminal
                    if (!Parse(symbol.ToString(), input, ref currentIndex))
                    {
                        match = false;
                        break;
                    }
                }
                else
                {
                    // Match terminal
                    if (currentIndex >= input.Length || input[currentIndex] != symbol)
                    {
                        match = false;
                        break;
                    }
                    currentIndex++;
                }
            }

            if (match)
            {
                index = currentIndex;
                return true;
            }
        }

        return false;
    }
}