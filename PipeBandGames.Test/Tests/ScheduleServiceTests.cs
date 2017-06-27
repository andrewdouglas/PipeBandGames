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
        private IStartTimeService startTimeService;

        // The SetUp attribute is an NUnit attribute.  This method will run before each and every test method below.
        [SetUp]
        public void Setup()
        {
            this.startTimeService = Substitute.For<IStartTimeService>();
            this.service = new ScheduleService(startTimeService);
        }

        // The Test attribute is an NUnit attribute that denotes a test.
        [Test]
        public void GetSoloEventSchedule_OneCompetitor_ReturnsTrivialSchedule()
        {
            // Test: contest with one competitor in one event should have one event
            Contest contest = this.Contest;
            contest.ContestJudges = this.OneContestJudgeList;
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
            contest.ContestJudges = this.OneContestJudgeList;

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
            contest.ContestJudges = this.OneContestJudgeList;
            Assert.Throws<ArgumentNullException>(() => this.service.GetSoloEventSchedule(this.Contest));
        }
    }
}
