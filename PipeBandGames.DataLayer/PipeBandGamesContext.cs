using Microsoft.EntityFrameworkCore;
using PipeBandGames.DataLayer.Entities;

namespace PipeBandGames.DataLayer
{
    public class PipeBandGamesContext : DbContext
    {
        public DbSet<Competitor> Competitors { get; set; }

        public DbSet<Contest> Contest { get; set; }

        public DbSet<ContestJudge> ContestJudges { get; set; }

        public DbSet<Judge> Judges { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<PipeBandAssociation> PipeBandAssociations { get; set; }

        public DbSet<SoloEvent> SoloEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=pipe_band_games.db");
        }
    }
}
