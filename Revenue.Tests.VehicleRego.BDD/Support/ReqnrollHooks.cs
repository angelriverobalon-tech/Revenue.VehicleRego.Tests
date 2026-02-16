using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Reflection;
using Revenue.Tests.VehicleRego.BDD.Model.Pages;

namespace Revenue.Tests.VehicleRego.BDD.Support
{
    [SetUpFixture]
    public class ReqnrollHooks
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            var headlessStr = ConfigManager.GetConfigValue("PLAYWRIGHT_HEADLESS", "true");
            var headless = !string.Equals(headlessStr, "false", StringComparison.OrdinalIgnoreCase);

            await PlaywrightDriver.InitAsync(headless);
            var page = await PlaywrightDriver.NewPageAsync();

            try
            {
                // Try to locate the Reqnroll/BoDi ObjectContainer via reflection and register instances
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Type? containerType = null;
                object? containerInstance = null;

                foreach (var asm in assemblies)
                {
                    Type[] types;
                    try { types = asm.GetTypes(); } catch { continue; }
                    foreach (var t in types)
                    {
                        if (t.Name == "ObjectContainer" || t.Name == "ObjectContainer`1")
                        {
                            containerType = t;
                            break;
                        }
                    }
                    if (containerType != null) break;
                }

                if (containerType != null)
                {
                    // try common static accessors
                    var prop = containerType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static)
                               ?? containerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    if (prop != null)
                    {
                        containerInstance = prop.GetValue(null);
                    }
                    else
                    {
                        var field = containerType.GetField("Current", BindingFlags.Public | BindingFlags.Static)
                                    ?? containerType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
                        if (field != null)
                            containerInstance = field.GetValue(null);
                    }

                    if (containerInstance != null)
                    {
                        // find RegisterInstanceAs generic method
                        var regMethod = containerInstance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            .FirstOrDefault(m => m.Name == "RegisterInstanceAs" && m.IsGenericMethod && m.GetParameters().Length == 1);

                        if (regMethod != null)
                        {
                            var regIPage = regMethod.MakeGenericMethod(new Type[] { typeof(IPage) });
                            regIPage.Invoke(containerInstance, new object[] { page });

                            var vehiclePage = new VehicleRegistrationsPage(page);
                            var regVehicle = regMethod.MakeGenericMethod(new Type[] { typeof(VehicleRegistrationsPage) });
                            regVehicle.Invoke(containerInstance, new object[] { vehiclePage });
                        }
                    }
                }
            }
            catch
            {
                // If registration fails, swallow the error so tests can still run with manual setup
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            try
            {
                await PlaywrightDriver.DisposeAsync();
            }
            catch
            {
                // ignore
            }
        }
    }
}
