using NUnit.Framework;
using PipeBandGames.BusinessLayer.Services;
using PipeBandGames.DataLayer.Constants;
using PipeBandGames.DataLayer.Entities;
using PipeBandGames.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.Test.Tests
{
    public class StartTimeServiceTests : TestBase
    {
        private StartTimeService service;

        [SetUp]
        public void Setup()
        {
            this.service = new StartTimeService();
        }

        [Test]
        public void SetStartTimes_OneJudgeThreeCompetitors_ReturnsChainedStartTimes()
        {
            // Test: 1 judge for 3 different events and 3 different competitors registered to those events.
            // Expected result: the event start times should come one after another.
            Contest contest = this.Contest;
            contest.ContestJudges = this.GetContestJudges(Idiom.Piobaireachd, Idiom.March24, Idiom.March68);
            var competitors = new List<Competitor>();
            Enumerable.Range(1, 3).ToList().ForEach(x =>
            {
                competitors.Add(GetFakeCompetitor(x, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24, Idiom.March68));
            });
            contest.Competitors = competitors;

            List<SoloEvent> soloEvents = contest.SoloEvents.Where(x => x.SoloEventCompetitors.Any()).ToList();

            // call method being tested
            this.service.SetStartTimes(contest, soloEvents);

            // Order by the start time
            soloEvents = soloEvents.OrderBy(x => x.Start).ToList();

            // First event should start after the offset between registration opening
            Assert.That(soloEvents.First().Start, Is.EqualTo(contest.RegistrationOpen.Value.AddMinutes(Config.FirstEventRegistrationOpenOffset)));

            // Second event should start after the first event is complete
            Assert.That(soloEvents[1].Start, Is.EqualTo(soloEvents[0].Start.Value.AddMinutes(soloEvents[0].DurationMinutes)));

            // Third event should start after the first event is complete
            Assert.That(soloEvents[2].Start, Is.EqualTo(soloEvents[1].Start.Value.AddMinutes(soloEvents[1].DurationMinutes)));
        }

        [Test]
        public void SetStartTimes_ThreeJudgesTenCompetitorsInSameGrade_ReturnsSameStartTime()
        {
            // Test: 3 judges for 3 different idioms, and 10 competitors registered for those 3 idioms.
            // Expected result: all 3 events should be scheduled to start at the same time.
            Contest contest = this.Contest;
            contest.ContestJudges = this.GetContestJudges(Idiom.Piobaireachd, Idiom.March24, Idiom.March68);
            var competitors = new List<Competitor>();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                competitors.Add(GetFakeCompetitor(x, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24, Idiom.March68));
            });
            contest.Competitors = competitors;

            List<SoloEvent> soloEvents = contest.SoloEvents.Where(x => x.SoloEventCompetitors.Any()).ToList();

            // call method being tested
            this.service.SetStartTimes(contest, soloEvents);

            DateTime oneEventStart = soloEvents.First().Start.Value;
            Assert.That(soloEvents.All(x => x.Start.Value == oneEventStart), Is.True);
        }
    }
}
