using NSubstitute;
using NUnit.Framework;
using PipeBandGames.BusinessLayer.Interfaces;
using PipeBandGames.BusinessLayer.Services;
using PipeBandGames.DataLayer.Constants;
using PipeBandGames.DataLayer.Entities;
using PipeBandGames.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.Test.Tests
{
    /// <summary>
    /// This test class validates the logic in ScheduleService.  Notice that this class inherits from TestBase
    /// </summary>
    public class ScheduleServiceTests : TestBase
    {
        // The class being tested by this test class
        private ScheduleService service;

        // mocks
        private IJudgeService judgeService;

        // The SetUp attribute is an NUnit attribute.  This method will run before each and every test method below.
        [SetUp]
        public void Setup()
        {
            this.judgeService = Substitute.For<IJudgeService>();
            this.service = new ScheduleService(this.judgeService);
        }

        // The Test attribute is an NUnit attribute that denotes a test.
        [Test]
        public void GetSoloEventSchedule_OneCompetitor_ReturnsTrivialSchedule()
        {
            // Test: contest with one competitor in one event should have one event
            Contest contest = this.Contest;
            contest.ContestJudges = this.OneContestJudgeList(contest);
            Competitor competitor = GetFakeCompetitor(1, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd);
            contest.Competitors = new List<Competitor> { competitor };

            // Call method being tested
            List<SoloEvent> result = this.service.GetSoloEventSchedule(contest);

            // Assert that the values set match with our expectations
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(competitor.RegisteredSoloEvents.Count));
            var soloEvent = result.Single();
            Assert.That(soloEvent.SoloEventId, Is.EqualTo(competitor.RegisteredSoloEvents.Single().SoloEventId));
            Assert.That(soloEvent.DurationMinutes, Is.EqualTo(Config.PiobaireachdEventDuration));
            ////Assert.That(soloEvent.Start, Is.EqualTo(contest.RegistrationOpen.Value.AddMinutes(Config.FirstEventRegistrationOpenOffset)));
            ////Assert.That(contest.LastSoloEventComplete, Is.EqualTo(soloEvent.Start.Value.AddMinutes(soloEvent.DurationMinutes)));
        }

        [Test]
        public void GetSoloEventSchedule_TwoCompetitorsSameGrade_ReturnsSameEventCount()
        {
            // Test: contest with 2 grade 4 junior competitors registered in 2 events should have 2 events
            Contest contest = this.Contest;
            contest.ContestJudges = this.OneContestJudgeList(contest);

            Competitor competitor1 = GetFakeCompetitor(1, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24);
            Competitor competitor2 = GetFakeCompetitor(2, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24);
            contest.Competitors = new List<Competitor> { competitor1, competitor2 };

            // Call method being tested
            List<SoloEvent> result = this.service.GetSoloEventSchedule(contest);

            Assert.That(result.Count, Is.EqualTo(competitor1.RegisteredSoloEvents.Count));

            // The piob event should be 30 minutes and the non-piob should be 10
            SoloEvent piobEvent = result.Single(x => x.Idiom == Idiom.Piobaireachd);
            SoloEvent lightMusicEvent = result.Single(x => x.Idiom != Idiom.Piobaireachd);
            Assert.That(piobEvent.DurationMinutes, Is.EqualTo(contest.Competitors.Count * Config.PiobaireachdEventDuration));
            Assert.That(lightMusicEvent.DurationMinutes, Is.EqualTo(contest.Competitors.Count * Config.LightMusicEventDuration));

            ////// The piob event should be the first event
            ////Assert.That(piobEvent.Start.Value, Is.EqualTo(contest.RegistrationOpen.Value.AddMinutes(Config.FirstEventRegistrationOpenOffset)));

            ////// The light music event cannot begin until the light music event is over
            ////Assert.That(lightMusicEvent.Start.Value, Is.EqualTo(piobEvent.Start.Value.AddMinutes(piobEvent.DurationMinutes)));
        }

        [Test]
        public void SetStartTimes_OneJudgeThreeCompetitors_ReturnsChainedStartTimes()
        {
            // Test: 1 judge for 3 different events and 3 different competitors registered to those events.
            // Expected result: the event start times should come one after another.
            Contest contest = this.Contest;
            // Get 1 fake judge, doesn't matter what the idioms are because we'll mock the call to the JudgeService below
            contest.ContestJudges = this.GetContestJudges(contest, Instrument.Bagpipe, Idiom.Piobaireachd);
            var competitors = new List<Competitor>();
            Enumerable.Range(1, 3).ToList().ForEach(x =>
            {
                competitors.Add(GetFakeCompetitor(x, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24, Idiom.March68));
            });
            contest.Competitors = competitors;

            List<SoloEvent> soloEvents = contest.SoloEvents.Where(x => x.SoloEventCompetitors.Any()).ToList();

            // Return fake results from the JudgeService
            this.judgeService.GetMatchingJudge(null, null).ReturnsForAnyArgs(contest.ContestJudges.Single().Judge);

            // call method being tested
            this.service.GetSoloEventSchedule(contest);

            // Order by the start time
            soloEvents = soloEvents.OrderBy(x => x.Start).ToList();

            // First event should start after the offset between registration opening
            Assert.That(soloEvents[0].Start, Is.EqualTo(contest.RegistrationOpen.Value.AddMinutes(Config.FirstEventRegistrationOpenOffset)));

            // Second event should start after the first event is complete
            Assert.That(soloEvents[1].Start, Is.EqualTo(soloEvents[0].Start.Value.AddMinutes(soloEvents[0].DurationMinutes + Config.BreakBetweenEvents)));

            // Third event should start after the first event is complete
            Assert.That(soloEvents[2].Start, Is.EqualTo(soloEvents[1].Start.Value.AddMinutes(soloEvents[1].DurationMinutes + Config.BreakBetweenEvents)));
        }

        [Test]
        public void SetStartTimes_ThreeJudgesTenCompetitorsInSameGrade_ReturnsSameStartTime()
        {
            // Test: 3 judges for 3 different idioms, and 10 competitors registered for those 3 idioms.
            // Expected result: all 3 events should be scheduled to start at the same time.
            Contest contest = this.Contest;
            contest.ContestJudges = this.GetContestJudges(contest, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24, Idiom.March68);
            var competitors = new List<Competitor>();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                competitors.Add(GetFakeCompetitor(x, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.March24, Idiom.March68));
            });
            contest.Competitors = competitors;

            Judge piobaireachdJudge = contest.ContestJudges.Single(x => x.Judge.Idioms.Contains(Idiom.Piobaireachd)).Judge;
            this.judgeService.GetMatchingJudge(Arg.Any<Contest>(), Arg.Is<SoloEvent>(x => x.Idiom == Idiom.Piobaireachd)).Returns(piobaireachdJudge);
            Judge march24Judge = contest.ContestJudges.Single(x => x.Judge.Idioms.Contains(Idiom.March24)).Judge;
            this.judgeService.GetMatchingJudge(Arg.Any<Contest>(), Arg.Is<SoloEvent>(x => x.Idiom == Idiom.March24)).Returns(march24Judge);
            Judge march68Judge = contest.ContestJudges.Single(x => x.Judge.Idioms.Contains(Idiom.March68)).Judge;
            this.judgeService.GetMatchingJudge(Arg.Any<Contest>(), Arg.Is<SoloEvent>(x => x.Idiom == Idiom.March68)).Returns(march68Judge);

            List<SoloEvent> soloEvents = contest.SoloEvents.Where(x => x.SoloEventCompetitors.Any()).ToList();

            // call method being tested
            this.service.GetSoloEventSchedule(contest);

            DateTime oneEventStart = soloEvents.First().Start.Value;
            Assert.That(soloEvents.All(x => x.Start.Value == oneEventStart), Is.True);
        }

        [Test]
        public void GetSoloEventSchedule_NullContest_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => this.service.GetSoloEventSchedule(null));
        }

        [Test]
        public void GetSoloEventSchedule_NullRegistrationOpen_Throws()
        {
            Contest contest = this.Contest;
            contest.RegistrationOpen = null;
            Assert.Throws<InvalidOperationException>(() => this.service.GetSoloEventSchedule(contest));
        }

        [Test]
        public void GetSoloEventSchedule_EmptyJudges_Throws()
        {
            Contest contest = this.Contest;
            contest.Competitors = new List<Competitor> { new Competitor() };
            Assert.Throws<ArgumentNullException>(() => this.service.GetSoloEventSchedule(this.Contest));
        }

        [Test]
        public void GetSoloEventSchedule_EmptyCompetitors_Throws()
        {
            Contest contest = this.Contest;
            contest.ContestJudges = this.OneContestJudgeList(contest);
            Assert.Throws<ArgumentNullException>(() => this.service.GetSoloEventSchedule(this.Contest));
        }
    }
}
