using System;
using System.IO;
using System.Text.Json;

namespace Revenue.Tests.VehicleRego.BDD.Support
{
    public static class ConfigManager
    {
        public static string GetConfigValue(string key, string? defaultValue = null)
        {
            // 1. First, try to get from environment variables
            var envValue = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrWhiteSpace(envValue))
                return envValue;

            // 2. Fall back to env.json file
            var envPath = FindEnvPath();

            try
            {
                if (File.Exists(envPath))
                {
                    var json = File.ReadAllText(envPath);
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty(key, out var prop) && prop.ValueKind == JsonValueKind.String)
                    {
                        var value = prop.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                            return value!;
                    }
                }
            }
            catch
            {
                // Ignore and fall back to default
            }

            // 3. Return default value
            return defaultValue ?? string.Empty;
        }

        private static string FindEnvPath()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "Config", "env.json");
                if (File.Exists(candidate))
                    return candidate;
                dir = dir.Parent;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "Config", "env.json");
        }
    }
}
