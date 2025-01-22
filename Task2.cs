using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Task2
{
    public class JsonConfigurationManager
    {
        private static string ConfigFilePath = "D:\\Automation_QA\\ConsoleApp12\\JsonFiles\\config.json";

        public static void LoadAndDisplayConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                Console.WriteLine("Config file not found.");
                return;
            }

            using (var stream = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                string jsonData = reader.ReadToEnd();

                try
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                    Console.WriteLine(jsonObject.ToString(Formatting.Indented));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }
        }

        public static void ToggleFeatureFlag(string feature, bool isEnabled)
        {
            if (!File.Exists(ConfigFilePath))
            {
                Console.WriteLine("Config file not found.");
                return;
            }

            using (var stream = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                string jsonData = reader.ReadToEnd();

                try
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                    var featureFlags = jsonObject["featureFlags"] as JObject;

                    if (featureFlags[feature] != null)
                    {
                        featureFlags[feature] = isEnabled;
                        stream.SetLength(0);
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
                        }
                        Console.WriteLine($"Feature '{feature}' updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Feature not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating JSON: " + ex.Message);
                }
            }
        }

        public static void PrintSecureApiEndpoints()
        {
            if (!File.Exists(ConfigFilePath))
            {
                Console.WriteLine("Config file not found.");
                return;
            }

            using (var stream = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                string jsonData = reader.ReadToEnd();

                try
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                    var apiEndpoints = jsonObject["apiEndpoints"] as JArray;

                    var secureEndpoints = apiEndpoints.Where(e => (bool)e["isSecure"])
                                                       .Select(e => e.ToString(Formatting.Indented));

                    Console.WriteLine("Secure API Endpoints:");
                    foreach (var endpoint in secureEndpoints)
                    {
                        Console.WriteLine(endpoint);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }
        }

        public static void AddApiEndpoint(string name, string url, bool isSecure)
        {
            if (!File.Exists(ConfigFilePath))
            {
                Console.WriteLine("Config file not found.");
                return;
            }

            using (var stream = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                string jsonData = reader.ReadToEnd();

                try
                {
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                    var apiEndpoints = jsonObject["apiEndpoints"] as JArray;

                    if (apiEndpoints.Any(e => (string)e["name"] == name))
                    {
                        Console.WriteLine("API endpoint with this name already exists.");
                        return;
                    }

                    var newEndpoint = new JObject
                    {
                        { "name", name },
                        { "url", url },
                        { "isSecure", isSecure }
                    };

                    apiEndpoints.Add(newEndpoint);
                    stream.SetLength(0);
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
                    }

                    Console.WriteLine("API endpoint added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating JSON: " + ex.Message);
                }
            }
        }

        public static void RunTask2()
        {
            // Example Usage
            LoadAndDisplayConfig();
            ToggleFeatureFlag("darkMode", false);
            PrintSecureApiEndpoints();
            AddApiEndpoint("OrderService", "https://api.example.com/orders", true);
        }
    }
}
