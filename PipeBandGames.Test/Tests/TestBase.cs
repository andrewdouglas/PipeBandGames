using NUnit.Framework;
using PipeBandGames.DataLayer.Constants;
using PipeBandGames.DataLayer.Entities;
using PipeBandGames.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeBandGames.Test.Tests
{
    /// <summary>
    /// Abstract base class for unit test classes in this assembly.  Provides the TestFixture and Category attributes as well as helpful setup for tests.
    /// </summary>
    [TestFixture]
    [Category("PipeBandGamesTests")]
    public abstract class TestBase
    {
        public Contest Contest
        {
            get
            {
                var today = DateTime.Today;
                var doorsOpen = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0);
                var registrationOpen = doorsOpen.AddMinutes(Config.RegistrationOpenDoorsOpenOffset);

                return new Contest
                {
                    DoorsOpen = doorsOpen,
                    RegistrationOpen = registrationOpen,
                    SoloEvents = new List<SoloEvent>
                    {
                        new SoloEvent { SoloEventId = 0, Grade = Grade.FourJunior, Instrument = Instrument.Bagpipe, Idiom = Idiom.Piobaireachd },
                        new SoloEvent { SoloEventId = 1, Grade = Grade.FourJunior, Instrument = Instrument.Bagpipe, Idiom = Idiom.March24 },
                        new SoloEvent { SoloEventId = 2, Grade = Grade.FourJunior, Instrument = Instrument.Bagpipe, Idiom = Idiom.March68 },
                        new SoloEvent { SoloEventId = 10, Grade = Grade.FourSenior, Instrument = Instrument.Bagpipe, Idiom = Idiom.Piobaireachd },
                        new SoloEvent { SoloEventId = 11, Grade = Grade.FourSenior, Instrument = Instrument.Bagpipe, Idiom = Idiom.March24 },
                        new SoloEvent { SoloEventId = 100, Grade = Grade.Novice, Instrument = Instrument.TenorDrum, Idiom = Idiom.QuickMarchMedley },
                        new SoloEvent { SoloEventId = 200, Grade = Grade.Three, Instrument = Instrument.SnareDrum, Idiom = Idiom.March24 },
                        new SoloEvent { SoloEventId = 300, Grade = Grade.Novice, Instrument = Instrument.BassDrum, Idiom = Idiom.MarchStrathspeyReel }
                    }
                };
            }
        }

        public Judge OneJudge
        {
            get
            {
                var judge = new Judge { JudgeId = 7 };
                judge.Instruments.Add(Instrument.Bagpipe);
                judge.Idioms.Add(Idiom.Piobaireachd);
                return judge;
            }
        }

        public List<ContestJudge> OneContestJudgeList(Contest contest)
        {
            Judge judge = this.OneJudge;
            return new List<ContestJudge>
            {
                new ContestJudge { Judge = judge, JudgeId = judge.JudgeId, Contest = contest }
            };
        }

        // Helper method that returns a Competitor instance with a populated List of SoloEventCompetitor instances matching with the given Contest
        protected static Competitor GetFakeCompetitor(int competitorId, Contest contest, Grade grade, Instrument instrument, params Idiom[] idioms)
        {
            var competitor = new Competitor { CompetitorId = competitorId };

            foreach (Idiom idiom in idioms)
            {
                SoloEvent targetSoloEvent = contest.SoloEvents.Single(x => x.Grade == grade && x.Idiom == idiom && x.Instrument == instrument);
                var soloEventCompetitor = new SoloEventCompetitor { CompetitorId = competitorId, SoloEventId = targetSoloEvent.SoloEventId, SoloEvent = targetSoloEvent };
                competitor.RegisteredSoloEvents.Add(soloEventCompetitor);
                targetSoloEvent.SoloEventCompetitors.Add(soloEventCompetitor);
            }

            return competitor;
        }

        protected List<ContestJudge> GetContestJudges(Contest contest, Instrument instrument, params Idiom[] idioms)
        {
            var result = new List<ContestJudge>();
            int judgeId = 0;
            foreach (Idiom idiom in idioms)
            {
                var judge = new Judge();
                judge.JudgeId = judgeId++;
                judge.Instruments.Add(instrument);
                judge.Idioms.Add(idiom);

                result.Add(new ContestJudge { JudgeId = judge.JudgeId, Judge = judge, Contest = contest });
            }

            return result;
        }
    }
}
