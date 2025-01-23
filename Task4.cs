using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Task4
{
    public class TransactionSplitter
    {
        private static string TransactionsPath = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\transactions.json";
        private static string CreditsPath = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\credits.json";
        private static string DebitsPath = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\debits.json";

        public static void EnsureJsonFilesExist()
        {
            if (!File.Exists(CreditsPath))
            {
                Console.WriteLine("Credits file not found. Creating a new file...");
                try
                {
                    File.WriteAllText(CreditsPath, new JArray().ToString(Formatting.Indented));
                    Console.WriteLine("Credits file created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating credits file: " + ex.Message);
                }
            }

            if (!File.Exists(DebitsPath))
            {
                Console.WriteLine("Debits file not found. Creating a new file...");
                try
                {
                    File.WriteAllText(DebitsPath, new JArray().ToString(Formatting.Indented));
                    Console.WriteLine("Debits file created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating debits file: " + ex.Message);
                }
            }
        }

        public static void SplitTransactionsAndCalculateTotals()
        {
            if (!File.Exists(TransactionsPath))
            {
                Console.WriteLine("Transactions file not found.");
                return;
            }

            try
            {
                string transactionsData = File.ReadAllText(TransactionsPath);
                var transactions = JsonConvert.DeserializeObject<List<JObject>>(transactionsData);

                var credits = transactions.Where(t => (string)t["type"] == "credit").ToList();
                var debits = transactions.Where(t => (string)t["type"] == "debit").ToList();

                var totalCredits = credits.Sum(t => (decimal)t["amount"]);
                var totalDebits = debits.Sum(t => (decimal)t["amount"]);

                File.WriteAllText(CreditsPath, JsonConvert.SerializeObject(credits, Formatting.Indented));
                File.WriteAllText(DebitsPath, JsonConvert.SerializeObject(debits, Formatting.Indented));

                Console.WriteLine("Credits saved to credits.json");
                Console.WriteLine("Debits saved to debits.json");

                Console.WriteLine($"Total Credits: {totalCredits}");
                Console.WriteLine($"Total Debits: {totalDebits}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing transactions: " + ex.Message);
            }
        }

        public static void RunTask4()
        {
            EnsureJsonFilesExist();
            SplitTransactionsAndCalculateTotals();
        }
    }
}
