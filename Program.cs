using System;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Choose a task to execute:");
            Console.WriteLine("1. Product Inventory Management");
            Console.WriteLine("2. [Future Task Placeholder]");
            Console.WriteLine("3. Exit");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ProductInventoryManager.RunTask1(); // Calls Task 1
                    break;

                case 2:
                    Console.WriteLine("[Future Task Placeholder: Add your task logic here]");
                    break;

                case 3:
                    Console.WriteLine("Exiting the application. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid task.");
                    break;
            }
        }
    }
}
