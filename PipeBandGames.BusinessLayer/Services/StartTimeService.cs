using PipeBandGames.BusinessLayer.Interfaces;
using System.Collections.Generic;
using PipeBandGames.DataLayer.Entities;
using System.Linq;
using PipeBandGames.DataLayer.Constants;

namespace PipeBandGames.BusinessLayer.Services
{
    public class StartTimeService : IStartTimeService
    {
        public void SetStartTimes(Contest contest, List<SoloEvent> soloEvents)
        {
            // TODO: Replace this naive implementation with a better one
            soloEvents.First().Start = contest.RegistrationOpen.Value.AddMinutes(Config.FirstEventRegistrationOpenOffset);

            // This should depend on how many judges there are, and the judges' "disciplines"; which events they're able to judge.
            // Judges should have blocks of time filled up.
            // It's preferable to keep grades roughly together e.g. we don't want to have one grade's events separated by 4 hours.
            // Judges should get breaks periodically - ideally between each grade if possible.

            for (int i = 1; i < soloEvents.Count; i++)
            {
                SoloEvent previousEvent = soloEvents[i - 1];
                soloEvents[i].Start = previousEvent.Start.Value.AddMinutes(previousEvent.DurationMinutes);
            }
        }
    }
}
