using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

class ProductInventoryManager
{
    private const string InventoryFilePath = "inventory.json";

    public static void RunTask1()
    {
        while (true)
        {
            try
            {
                if (!File.Exists(InventoryFilePath))
                {
                    Console.WriteLine("Inventory file not found. Creating a new one...");
                    File.WriteAllText(InventoryFilePath, "[]");
                }

                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Add a new product");
                Console.WriteLine("2. Calculate total stock value by category");
                Console.WriteLine("3. Display the best-selling product");
                Console.WriteLine("4. Update stock quantity by productId");
                Console.WriteLine("5. Exit");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        CalculateTotalStockValue();
                        break;
                    case 3:
                        PrintBestSellingProduct();
                        break;
                    case 4:
                        UpdateStockQuantity();
                        break;
                    case 5:
                        Console.WriteLine("Exiting the application.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    private static JArray LoadInventory()
    {
        string json = File.ReadAllText(InventoryFilePath);
        return JArray.Parse(json);
    }

    private static void SaveInventory(JArray inventory)
    {
        File.WriteAllText(InventoryFilePath, inventory.ToString(Formatting.Indented));
    }

    private static void AddProduct()
    {
        Console.WriteLine("Enter product details (productId, productName, category, price, stock, sales as comma-separated values):");
        string[] input = Console.ReadLine().Split(',');

        int productId = int.Parse(input[0]);
        string productName = input[1];
        string category = input[2];
        decimal price = decimal.Parse(input[3]);
        int stock = int.Parse(input[4]);
        int[] sales = input[5].Split('-').Select(int.Parse).ToArray();

        JArray inventory = LoadInventory();

        if (inventory.Any(p => (int)p["productId"] == productId))
        {
            Console.WriteLine("Product with the given productId already exists.");
            return;
        }

        JObject newProduct = new JObject
        {
            ["productId"] = productId,
            ["productName"] = productName,
            ["category"] = category,
            ["price"] = price,
            ["stock"] = stock,
            ["sales"] = new JArray(sales)
        };

        inventory.Add(newProduct);
        SaveInventory(inventory);
        Console.WriteLine("Product added successfully.");
    }

    private static void CalculateTotalStockValue()
    {
        JArray inventory = LoadInventory();

        var stockValueByCategory = inventory.GroupBy(p => p["category"].ToString())
            .Select(g => new { Category = g.Key, TotalValue = g.Sum(p => (decimal)p["price"] * (int)p["stock"]) });

        Console.WriteLine("Total Stock Value by Category:");
        foreach (var category in stockValueByCategory)
        {
            Console.WriteLine($"{category.Category}: {category.TotalValue:C}");
        }
    }

    private static void PrintBestSellingProduct()
    {
        JArray inventory = LoadInventory();

        var bestSeller = inventory.Select(p => new
        {
            ProductName = p["productName"].ToString(),
            TotalSales = ((JArray)p["sales"]).Sum(s => (int)s)
        }).OrderByDescending(p => p.TotalSales).FirstOrDefault();

        Console.WriteLine(bestSeller != null
            ? $"Best-Selling Product: {bestSeller.ProductName} (Total Sales: {bestSeller.TotalSales})"
            : "No products available to evaluate best-seller.");
    }

    private static void UpdateStockQuantity()
    {
        Console.WriteLine("Enter the productId of the product to update:");
        int productId = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the new stock quantity:");
        int newStock = int.Parse(Console.ReadLine());

        JArray inventory = LoadInventory();

        JObject product = inventory.FirstOrDefault(p => (int)p["productId"] == productId) as JObject;
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        product["stock"] = newStock;
        SaveInventory(inventory);
        Console.WriteLine("Stock quantity updated successfully.");
    }
}
