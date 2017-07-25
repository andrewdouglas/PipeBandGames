using Microsoft.Extensions.DependencyInjection;
using PipeBandGames.DataLayer;
using System;
using System.Linq;

namespace PipeBandGames.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Look into IoC and configuration
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();

            // Ensure that we have a local database up and running
            CreateDatabase();

            // Show the main menu to allow the user to select options for doing stuff
            new Program().ShowMenu();
        }

        // TODO: Move this elsewhere
        private static void CreateDatabase()
        {
            using (var db = new PipeBandGamesContext())
            {
                // This works on initial run, but throws exception on second run...why?
                //db.Database.Migrate();

                // Creates the DB if it doesn't exist, but doesn't do anything if it does.  Does NOT apply migrations.
                db.Database.EnsureCreated();

                // This works!
                // db.Persons.Add(new Person { FirstName = "Bob", LastName = "Marley", Created = DateTime.Now, CreatedBy = 1 });
                // db.SaveChanges();
            }
        }

        private void ShowMenu()
        {
            // Initialize the main menu
            Menu mainMenu = new MainMenu();

            // An infinite loop to keep prompting the user to make a selection until they quit the application
            while (true)
            {
                // Show the main menu
                Console.WriteLine(mainMenu.ToString());

                // Wait for the user to press a key corresponding to their desired menu item
                ConsoleKeyInfo selection = Console.ReadKey();

                // Attempt to reconcile the user's choice against the menu items in the main menu
                MenuItem selectedMenuItem = mainMenu.MenuItems.SingleOrDefault(x => x.Command == selection.KeyChar);

                // Show a blank line after the user's selected character or else the subsequently-displayed text runs into it
                Console.Write(Environment.NewLine);

                if (selectedMenuItem == null)
                {
                    // No matching menu item was found
                    Console.WriteLine("Invalid selection");
                    continue;
                }

                // The menu item was found, so execute its action, whatever that may be
                selectedMenuItem.Action();
            }
        }
    }
}