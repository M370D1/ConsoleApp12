using System;
using Task1;
using Task2;
using Task3;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Choose a task to execute:");
            Console.WriteLine("1. Product Inventory Management");
            Console.WriteLine("2. Dynamic JSON Configuration Manager");
            Console.WriteLine("3. Merge Two JSON Files");
            Console.WriteLine("4. Exit");

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
                    JsonConfigurationManager.RunTask2(); // Calls Task 2
                    break;

                case 3:
                    ProductMerger.RunTask3(); // Calls Task 3
                    break;

                case 4:
                    Console.WriteLine("Exiting the application. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid task.");
                    break;
            }
        }
    }
}
