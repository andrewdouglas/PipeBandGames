using NUnit.Framework;
using PipeBandGames.BusinessLayer.Services;
using PipeBandGames.DataLayer.Entities;
using PipeBandGames.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.Test.Tests
{
    public class JudgeServiceTests : TestBase
    {
        private JudgeService service;

        [SetUp]
        public void Setup()
        {
            this.service = new JudgeService();
        }

        [Test]
        public void GetMatchingJudge_ForPiobEvent_ReturnsJudge()
        {
            // Test: Contest with one piob judge - GetMatchingJudge should return that one judge
            Contest contest = this.Contest;
            contest.ContestJudges = this.OneContestJudgeList(contest);

            SoloEvent targetSoloEvent = contest.SoloEvents.First(x => x.Idiom == Idiom.Piobaireachd);

            Judge judge = this.service.GetMatchingJudge(contest, targetSoloEvent);

            Assert.That(judge.JudgeId, Is.EqualTo(contest.ContestJudges[0].JudgeId));
        }

        [Test]
        public void GetMatchingJudge_NoMatchingJudge_Throws()
        {
            // Test: GetMatchingJudge called for a BassDrum SoloEvent, but no judge exists with BassDrum expertise - should throw an error
            Contest contest = this.Contest;
            contest.ContestJudges.Add(new ContestJudge
            {
                // This judge has no instruments matching with the solo event
                Judge = new Judge { Instruments = new List<Instrument> { Instrument.BassDrum } },
                Contest = contest
            });

            Assert.Throws<InvalidOperationException>(() => this.service.GetMatchingJudge(contest, contest.SoloEvents.First()));
        }

        [Test]
        public void GetMatchingJudge_ExistingJudge_ReturnsJudge()
        {
            // Test: GetMatchingJudge for an event that already has a judge assigned
            Contest contest = this.Contest;
            contest.ContestJudges = this.OneContestJudgeList(contest);
            // Judge has already been assigned the Piob event
            SoloEvent soloEvent = contest.SoloEvents.First(x => x.Idiom == Idiom.Piobaireachd);
            soloEvent.Judge = contest.ContestJudges.Single().Judge;

            Judge judge = this.service.GetMatchingJudge(contest, soloEvent);

            Assert.That(judge.JudgeId, Is.EqualTo(soloEvent.Judge.JudgeId));
        }

        [Test]
        public void GetMatchingJudge_MultiplePiobJudges_ReturnsLeastBusyJudge()
        {
            // Test: Four piob judges - piob event should be assigned to the piob judge with the least amount of events in terms of time required

            // Setup here is 2 piob judges and 2 competitors registered for 2 different piob events.
            // Judge 1 (busyJudge) will be set up to already be judging the one event, so Judge 2 (availableJudge)
            // should be selected to judge the other event.
            Contest contest = this.Contest;
            contest.ContestJudges = this.GetContestJudges(contest, Instrument.Bagpipe, Idiom.Piobaireachd, Idiom.Piobaireachd);
            Judge busyJudge = contest.ContestJudges.First().Judge;
            Judge availableJudge = contest.ContestJudges.Last().Judge;
            contest.Competitors.Add(TestBase.GetFakeCompetitor(1, contest, Grade.FourJunior, Instrument.Bagpipe, Idiom.Piobaireachd));
            contest.Competitors.Add(TestBase.GetFakeCompetitor(2, contest, Grade.FourSenior, Instrument.Bagpipe, Idiom.Piobaireachd));

            // Add the first Piob event to this judge in order to make him busy
            ContestJudge contestJudge = contest.ContestJudges.Single(x => x.JudgeId == busyJudge.JudgeId);
            SoloEvent busyJudgeEvent = contest.SoloEvents.Single(x => x.Idiom == Idiom.Piobaireachd && x.Grade == Grade.FourJunior);
            busyJudgeEvent.Judge = busyJudge;

            // This is the event that should get assigned to availableJudge
            SoloEvent availableJudgeEvent = contest.SoloEvents.Single(x => x.Idiom == Idiom.Piobaireachd && x.Grade == Grade.FourSenior);

            // Call the method being tested
            Judge result = this.service.GetMatchingJudge(contest, availableJudgeEvent);

            // Ensure that the judge picked for the event is the available one
            Assert.That(result.JudgeId, Is.EqualTo(availableJudge.JudgeId));
        }
    }
}
