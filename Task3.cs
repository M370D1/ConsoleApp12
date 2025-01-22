using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Task3
{
    public class ProductMerger
    {
        private static string Products1Path = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\products1.json";
        private static string Products2Path = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\products2.json";
        private static string MergedProductsPath = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\mergedProducts.json";

        public static void MergeProductFiles()
        {
            if (!File.Exists(Products1Path) || !File.Exists(Products2Path))
            {
                Console.WriteLine("One or both source files not found.");
                return;
            }

            try
            {
                string products1Data = File.ReadAllText(Products1Path);
                string products2Data = File.ReadAllText(Products2Path);

                var products1 = JsonConvert.DeserializeObject<List<JObject>>(products1Data);
                var products2 = JsonConvert.DeserializeObject<List<JObject>>(products2Data);

                var mergedProducts = products1.ToDictionary(p => (int)p["id"], p => p); // Use products1 as base
                foreach (var product in products2)
                {
                    mergedProducts[(int)product["id"]] = product; // Overwrite with products2 if ID matches
                }

                // Convert merged dictionary back to a list
                var mergedProductsArray = new JArray(mergedProducts.Values);

                // Save to the new JSON file
                using (var stream = new FileStream(MergedProductsPath, FileMode.Create, FileAccess.Write))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(mergedProductsArray, Formatting.Indented));
                }

                Console.WriteLine("Merged products saved to mergedProducts.json successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error merging products: " + ex.Message);
            }
        }

        public static void EnsureMergedProductsFileExists()
        {
            if (!File.Exists(MergedProductsPath))
            {
                Console.WriteLine("Merged products file not found. Creating a new file...");
                try
                {
                    var emptyArray = new JArray();
                    File.WriteAllText(MergedProductsPath, emptyArray.ToString(Formatting.Indented));
                    Console.WriteLine("Merged products file created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating merged products file: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Merged products file already exists.");
            }
        }

        public static void RunTask3()
        {
            EnsureMergedProductsFileExists();
            MergeProductFiles();
        }
    }
}
