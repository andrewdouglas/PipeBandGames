using PipeBandGames.BusinessLayer.Interfaces;
using PipeBandGames.DataLayer.Entities;
using System;
using System.Linq;

namespace PipeBandGames.BusinessLayer.Services
{
    public class JudgeService : IJudgeService
    {
        public Judge GetMatchingJudge(Contest contest, SoloEvent soloEvent)
        {
            // Is a judge already set to adjudicate this event?
            if (soloEvent.Judge != null)
            {
                // yes - find the judge
                return contest.ContestJudges.Single(x => x.SoloEvents.Any(y => y.SoloEventId == soloEvent.SoloEventId)).Judge;
            }

            // Look for judges certified to the event who aren't already set to adjudicate it, selecting the least-busy judge
            ContestJudge contestJudge = contest.ContestJudges
                .OrderBy(x => x.SoloEvents.Sum(y => y.DurationMinutes))
                .FirstOrDefault(x => x.Judge.Instruments.Contains(soloEvent.Instrument)
                    && x.Judge.Idioms.Contains(soloEvent.Idiom)
                    && !x.SoloEvents.Any(y => y.SoloEventId == soloEvent.SoloEventId));

            if (contestJudge == null)
            {
                // No matching judge found
                throw new InvalidOperationException($"No judge is available to adjudicate event: {soloEvent.ToString()}");
            }

            // We found a judge.  Return him/her.
            return contestJudge.Judge;
        }
    }
}
