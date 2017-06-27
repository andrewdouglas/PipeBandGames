using PipeBandGames.BusinessLayer.Interfaces;
using PipeBandGames.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.BusinessLayer.Services
{
    public class ScheduleService : IScheduleService
    {
        // This service depends on another service, but the dependency is to an interface.  This promotes loose-coupling.
        private readonly IStartTimeService startTimeService;

        public ScheduleService(IStartTimeService startTimeService)
        {
            this.startTimeService = startTimeService;
        }

        public List<SoloEvent> GetSoloEventSchedule(Contest contest)
        {
            // Ensure that the data is in a valid state to begin scheduling
            this.Validate(contest);

            // For scheduling purposes, we only care about solo events that have competitors associated with them
            List<SoloEvent> soloEvents = contest.SoloEvents.Where(x => x.SoloEventCompetitors.Any()).ToList();

            // Set the Start times of all solo events
            this.startTimeService.SetStartTimes(contest, soloEvents);

            return soloEvents;
        }

        private void Validate(Contest contest)
        {
            if (contest == null)
            {
                throw new ArgumentNullException(nameof(contest));
            }

            if (!contest.RegistrationOpen.HasValue)
            {
                throw new InvalidOperationException("No date/time set for the opening of the registration table.  The contest must have Doors Open and Registration Open values set before scheduling can begin.");
            }

            if (contest.ContestJudges.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(contest.ContestJudges));
            }

            if (contest.Competitors.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(contest.Competitors));
            }
        }
    }
}
