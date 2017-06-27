using Microsoft.Extensions.DependencyInjection;
using PipeBandGames.DataLayer;
using PipeBandGames.DataLayer.Entities;
using System;

namespace PipeBandGames.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();

            CreateDatabase();

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private static void CreateDatabase()
        {
            using (var db = new PipeBandGamesContext())
            {
                // This works on initial run, but throws exception on second run...why?
                //db.Database.Migrate();

                // Creates the DB if it doesn't exist, but doesn't do anything if it does.  Does NOT apply migrations.
                db.Database.EnsureCreated();

                // This works!
                db.Persons.Add(new Person { FirstName = "Bob", LastName = "Marley", Created = DateTime.Now, CreatedBy = 1 });
                db.SaveChanges();
            }
        }
    }
}