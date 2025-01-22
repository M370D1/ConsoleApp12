using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Task1
{
    public class ProductInventoryManager
    {
        private static string FilePath = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\inventory.json";
        public static void AddProduct(int productId, string productName, string category, decimal price, int stock, int[] sales)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            string jsonData = File.ReadAllText(FilePath);

            try
            {
                var products = JsonConvert.DeserializeObject<JArray>(jsonData) ?? new JArray();

                if (products.Any(p => (int)p["productId"] == productId))
                {
                    Console.WriteLine("Product with this productId already exists.");
                    return;
                }

                var newProduct = new JObject
        {
            { "productId", productId },
            { "productName", productName },
            { "category", category },
            { "price", price },
            { "stock", stock },
            { "sales", new JArray(sales) }
        };

                products.Add(newProduct);

                File.WriteAllText(FilePath, JsonConvert.SerializeObject(products, Formatting.Indented));
                Console.WriteLine("Product added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
        }


        public static void CalculateTotalStockValuePerCategory()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            string jsonData = File.ReadAllText(FilePath);

            try
            {
                var products = JsonConvert.DeserializeObject<JArray>(jsonData) ?? new JArray();

                var stockValues = products.GroupBy(p => (string)p["category"])
                                           .Select(g => new
                                           {
                                               Category = g.Key,
                                               TotalStockValue = g.Sum(p => (decimal)p["price"] * (int)p["stock"])
                                           });

                foreach (var category in stockValues)
                {
                    Console.WriteLine($"Category: {category.Category}, Total Stock Value: {category.TotalStockValue:C}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
        }


        public static void PrintBestSellingProduct()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            string jsonData = File.ReadAllText(FilePath);

            try
            {
                var products = JsonConvert.DeserializeObject<JArray>(jsonData) ?? new JArray();

                var bestSellingProduct = products.OrderByDescending(p => ((JArray)p["sales"]).Sum(s => (int)s))
                                                  .FirstOrDefault();

                if (bestSellingProduct != null)
                {
                    Console.WriteLine("Best Selling Product:");
                    Console.WriteLine($"Product ID: {bestSellingProduct["productId"]}");
                    Console.WriteLine($"Product Name: {bestSellingProduct["productName"]}");
                    Console.WriteLine($"Total Sales: {((JArray)bestSellingProduct["sales"]).Sum(s => (int)s)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
        }


        public static void UpdateStock(int productId, int newStock)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            string jsonData = File.ReadAllText(FilePath);

            try
            {
                var products = JsonConvert.DeserializeObject<JArray>(jsonData) ?? new JArray();
                var productToUpdate = products.FirstOrDefault(p => (int)p["productId"] == productId);

                if (productToUpdate == null)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                productToUpdate["stock"] = newStock;
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(products, Formatting.Indented));
                Console.WriteLine("Stock updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
        }


        public static void RunTask1()
        {
            // Example Usage
            AddProduct(101, "Laptop", "Electronics", 1200.50m, 10, new int[] { 5, 7, 8 });
            CalculateTotalStockValuePerCategory();
            PrintBestSellingProduct();
            UpdateStock(101, 15);
        }
    }
}
