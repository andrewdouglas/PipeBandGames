using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PipeBandGames.DataLayer.Entities;

namespace PipeBandGames.DataLayer
{
    public interface IPipeBandGamesContext
    {
        DbSet<Competitor> Competitors { get; set; }

        DbSet<Contest> Contests { get; set; }

        DbSet<ContestJudge> ContestJudges { get; set; }

        DbSet<Judge> Judges { get; set; }

        DbSet<Person> Persons { get; set; }

        DbSet<PipeBandAssociation> PipeBandAssociations { get; set; }

        DbSet<SoloEvent> SoloEvents { get; set; }

        int SaveChanges();

        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
    }
}
