using Microsoft.EntityFrameworkCore;
using PipeBandGames.DataLayer.Entities;

namespace PipeBandGames.DataLayer
{
    public class PipeBandGamesContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DbSet<Judge> Judges { get; set; }

        public DbSet<PipeBandAssociation> PipeBandAssociations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=pipe_band_games.db");
        }
    }
}
