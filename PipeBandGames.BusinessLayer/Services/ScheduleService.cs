using PipeBandGames.BusinessLayer.Interfaces;
using PipeBandGames.DataLayer.Constants;
using PipeBandGames.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.BusinessLayer.Services
{
    public class ScheduleService : IScheduleService
    {
        // This service depends on another service, but the dependency is to an interface.  This promotes loose-coupling.
        private readonly IJudgeService judgeService;

        public ScheduleService(IJudgeService judgeService)
        {
            this.judgeService = judgeService;
        }

        public List<SoloEvent> GetSoloEventSchedule(Contest contest)
        {
            // Ensure that the data is in a valid state to begin scheduling
            this.Validate(contest);

            // For scheduling purposes, we only care about solo events that have competitors associated with them
            List<SoloEvent> soloEvents = contest.SoloEvents.Where(x => x.SoloEventCompetitors.Any()).ToList();
            contest.ScheduledSoloEvents = soloEvents;

            // Assign the judges to events
            this.AssignJudgesToEvents(contest);

            // Set the Start times of all solo events
            this.SetStartTimes(contest);

            return soloEvents;
        }

        private void SetStartTimes(Contest contest)
        {
            // Now that the judges have been assigned events, assign times to them
            foreach (ContestJudge contestJudge in contest.ContestJudges)
            {
                DateTime currentEventStart = contest.FirstSoloEventStart.Value;
                foreach (SoloEvent soloEvent in contestJudge.SoloEvents)
                {
                    soloEvent.Start = currentEventStart;
                    currentEventStart = currentEventStart.AddMinutes(soloEvent.DurationMinutes + Config.BreakBetweenEvents);
                }
            }
        }

        private void AssignJudgesToEvents(Contest contest)
        {
            foreach (SoloEvent soloEvent in contest.ScheduledSoloEvents)
            {
                soloEvent.Judge = this.judgeService.GetMatchingJudge(contest, soloEvent);
            }
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
