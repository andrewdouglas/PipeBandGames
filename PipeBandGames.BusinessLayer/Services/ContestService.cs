using PipeBandGames.BusinessLayer.Interfaces;
using PipeBandGames.DataLayer;
using PipeBandGames.DataLayer.Entities;
using System;
using System.Linq;

namespace PipeBandGames.BusinessLayer.Services
{
    public class ContestService : IContestService
    {
        private readonly IPipeBandGamesContext dbContext;

        public ContestService(IPipeBandGamesContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Contest CreateContest(Contest contest)
        {
            if (contest == null)
            {
                throw new ArgumentNullException(nameof(contest));
            }

            // Contest Date must be today or in the future
            if (contest.ContestDate < DateTime.Today)
            {
                throw new InvalidOperationException("Contest Date must be today or in the future");
            }

            // Check for an existing contest with the same name (ignoring case sensitivity) and date
            Contest existing = this.dbContext.Contests.SingleOrDefault(c => c.Name.Equals(contest.Name, StringComparison.CurrentCultureIgnoreCase) && c.ContestDate == contest.ContestDate);
            if (existing != null)
            {
                throw new InvalidOperationException($"Contest {contest.Name} already exists on {contest.ContestDate}");
            }

            // Save the new contest and return it with its new ContestId populated
            this.dbContext.Add(contest);
            this.dbContext.SaveChanges();
            return contest;
        }
    }
}
