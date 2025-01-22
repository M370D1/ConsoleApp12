using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class JsonConfigurationManager
{
    private const string ConfigFilePath = "config.json";

    public static void RunTask2()
    {
        while (true)
        {
            try
            {
                EnsureConfigFileExists();

                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Display all configuration settings");
                Console.WriteLine("2. Enable/disable feature flags");
                Console.WriteLine("3. Filter and display secure API endpoints");
                Console.WriteLine("4. Add a new API endpoint");
                Console.WriteLine("5. Exit");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        DisplayConfigSettings();
                        break;
                    case 2:
                        ToggleFeatureFlag();
                        break;
                    case 3:
                        DisplaySecureApiEndpoints();
                        break;
                    case 4:
                        AddApiEndpoint();
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

    private static void EnsureConfigFileExists()
    {
        if (!File.Exists(ConfigFilePath))
        {
            Console.WriteLine("Config file not found. Creating a new one...");
            var defaultConfig = new JObject
            {
                ["appSettings"] = new JObject
                {
                    ["appName"] = "MyApplication",
                    ["version"] = "1.0.0"
                },
                ["featureFlags"] = new JObject
                {
                    ["darkMode"] = true,
                    ["betaFeatures"] = false
                },
                ["apiEndpoints"] = new JArray()
            };
            File.WriteAllText(ConfigFilePath, defaultConfig.ToString(Formatting.Indented));
        }
    }

    private static JObject LoadConfig()
    {
        string json = File.ReadAllText(ConfigFilePath);
        return JObject.Parse(json);
    }

    private static void SaveConfig(JObject config)
    {
        File.WriteAllText(ConfigFilePath, config.ToString(Formatting.Indented));
    }

    private static void DisplayConfigSettings()
    {
        JObject config = LoadConfig();
        Console.WriteLine("Current Configuration:");
        Console.WriteLine(config.ToString(Formatting.Indented));
    }

    private static void ToggleFeatureFlag()
    {
        JObject config = LoadConfig();

        Console.WriteLine("Available feature flags:");
        var featureFlags = config["featureFlags"] as JObject;
        foreach (var flag in featureFlags.Properties())
        {
            Console.WriteLine($"{flag.Name}: {flag.Value}");
        }

        Console.WriteLine("Enter the name of the feature flag to toggle:");
        string flagName = Console.ReadLine();

        if (featureFlags.ContainsKey(flagName))
        {
            featureFlags[flagName] = !(bool)featureFlags[flagName];
            SaveConfig(config);
            Console.WriteLine($"Feature flag '{flagName}' has been toggled.");
        }
        else
        {
            Console.WriteLine("Feature flag not found.");
        }
    }

    private static void DisplaySecureApiEndpoints()
    {
        JObject config = LoadConfig();

        Console.WriteLine("Secure API Endpoints:");
        var secureApis = config["apiEndpoints"]
            .Where(api => (bool)api["isSecure"])
            .Select(api => $"{api["name"]}: {api["url"]}");

        foreach (var api in secureApis)
        {
            Console.WriteLine(api);
        }
    }

    private static void AddApiEndpoint()
    {
        JObject config = LoadConfig();

        Console.WriteLine("Enter new API details (name, url, isSecure as comma-separated values):");
        string[] input = Console.ReadLine().Split(',');

        string name = input[0];
        string url = input[1];
        bool isSecure = bool.Parse(input[2]);

        var apiEndpoints = config["apiEndpoints"] as JArray;

        if (apiEndpoints.Any(api => api["name"].ToString() == name))
        {
            Console.WriteLine("An API with the given name already exists.");
            return;
        }

        JObject newApi = new JObject
        {
            ["name"] = name,
            ["url"] = url,
            ["isSecure"] = isSecure
        };

        apiEndpoints.Add(newApi);
        SaveConfig(config);
        Console.WriteLine("New API endpoint added successfully.");
    }
}
