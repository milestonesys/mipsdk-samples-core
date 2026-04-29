using sdk_core_crud.ItemOperations;
using sdk_core_crud.ItemOperations.Handlers;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.Platform.SDK.Core.Extensions;

namespace sdk_core_crud
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddMipServices();
            var serviceProvider = services.BuildServiceProvider();

            // Create session. To change default values, modify the Session
            var sessionHelper = new SessionHelper("http://localhost", UserType.DefaultWindows);
            var session = sessionHelper.CreateSession(serviceProvider);

            // Initialize all handlers
            var handlers = new List<IItemOperationHandler>
            {
                new AlarmDefinitionHandler(),
                new BasicUserHandler(),
                new CameraHandler(),
                new CameraGroupHandler(),
                new HardwareHandler(),
                new InputEventHandler(),
                new InputEventGroupHandler(),
                new MetadataHandler(),
                new MetadataGroupHandler(),
                new MicrophoneHandler(),
                new MicrophoneGroupHandler(),
                new OutputHandler(),
                new OutputGroupHandler(),
                new RecordingServerHandler(),
                new RoleHandler(),
                new SiteHandler(),
                new SpeakerHandler(),
                new SpeakerGroupHandler(),
                new UserDefinedEventHandler()
            };

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("  Configuration Item CRUD Operations");
                Console.WriteLine("========================================\n");
                Console.WriteLine("Select an item type to perform operations:\n");

                for (int i = 0; i < handlers.Count; i++)
                {
                    var handler = handlers[i];
                    var addDeleteSupport = handler.SupportsAddDelete ? "" : " (Read/Update only)";
                    Console.WriteLine($"{i + 1,2}. {handler.ItemTypeName}{addDeleteSupport}");
                }

                Console.WriteLine($"\n{handlers.Count + 1,2}. Exit\n");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == handlers.Count + 1)
                    {
                        running = false;
                        Console.WriteLine("\nExiting application...");
                    }
                    else if (choice > 0 && choice <= handlers.Count)
                    {
                        var selectedHandler = handlers[choice - 1];
                        try
                        {
                            await selectedHandler.ExecuteOperationsAsync(session);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nUnexpected error: {ex.Message}");
                            Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        }

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        await Task.Delay(1500);
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.");
                    await Task.Delay(1500);
                }
            }
        }
    }
}
