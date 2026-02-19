using System;
using System.IO;
using System.Text.Json;

namespace Revenue.Tests.VehicleRego.BDD.Support
{
    public static class ConfigManager
    {
        private static readonly bool _logEnabled =
            Environment.GetEnvironmentVariable("CONFIG_DEBUG") == "true";

        public static string GetConfigValue(string key, string? defaultValue = null)
        {
            // 1. Environment variables (highest priority)
            var envValue = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                Log($"Config '{key}' loaded from environment variable");
                return envValue;
            }

            // 2. env.json file
            var envPath = FindEnvPath();
            try
            {
                if (File.Exists(envPath))
                {
                    var json = File.ReadAllText(envPath);
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty(key, out var prop) &&
                        prop.ValueKind == JsonValueKind.String)
                    {
                        var value = prop.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            Log($"Config '{key}' loaded from {envPath}");
                            return value!;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Warning: Failed to read config '{key}' from file: {ex.Message}");
            }

            // 3. Default value
            Log($"Config '{key}' using default value: {defaultValue}");
            return defaultValue ?? string.Empty;
        }

        private static void Log(string message)
        {
            if (_logEnabled)
                Console.WriteLine($"[ConfigManager] {message}");
        }

        private static string FindEnvPath()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "env.json");
                if (File.Exists(candidate))
                    return candidate;

                candidate = Path.Combine(dir.FullName, "Config", "env.json");
                if (File.Exists(candidate))
                    return candidate;

                dir = dir.Parent;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "env.json");
        }
    }
}
