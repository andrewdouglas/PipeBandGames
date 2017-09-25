using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using PipeBandGames.BusinessLayer.Services;
using PipeBandGames.DataLayer;
using PipeBandGames.DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace PipeBandGames.Test.Tests
{
    public class ContestServiceTests : TestBase
    {
        // the class being tested by this class
        private ContestService service;

        // mocks used
        private IPipeBandGamesContext dbContext;

        // test data
        List<Contest> emptyContestList = new List<Contest>();

        [SetUp]
        public void Setup()
        {
            this.dbContext = Substitute.For<IPipeBandGamesContext>();
            this.service = new ContestService(dbContext);
        }

        [Test]
        public void CreateContest_HappyPath()
        {
            // Create a fake Contest entity to simulate one being created in the UI
            Contest fakeContest = this.GetFakeContest();

            // Arrange for the mocks to return no existing contests in the database
            DbSet<Contest> mockSet = CustomTestUtils.FakeDbSet<Contest>(this.emptyContestList);
            this.dbContext.Contests.Returns(mockSet);

            // Call the method being tested
            Contest contest = this.service.CreateContest(fakeContest);

            // Assert that calls were made against the mock dbContext and that we have a result
            this.dbContext.Received(1).Add(Arg.Is<Contest>(x => x.Name == fakeContest.Name));
            this.dbContext.Received(1).SaveChanges();
            Assert.That(contest, Is.Not.Null);
        }

        [Test]
        public void CreateContest_InvalidDate_Throws()
        {
            // This test ensures that users can't create contests for past dates
            Contest fakeContest = new Contest { ContestDate = DateTime.Today.AddDays(-10) };

            // An exception should be thrown if this is the case
            Assert.Throws<InvalidOperationException>(() => this.service.CreateContest(fakeContest));

            // Ensure that no calls to the database were made
            this.dbContext.DidNotReceiveWithAnyArgs().SaveChanges();
        }

        [Test]
        public void CreateContest_DuplicateContest_Throws()
        {
            // This test ensures that the user will be prevented from saving a duplicate test as determined by 
            // the same Name and ContestDate
            Contest fakeContest = this.GetFakeContest();

            // Arrange for the mocks to return an existing Contest that matches with the fake being saved
            Contest oneExisting = new Contest { Name = fakeContest.Name, ContestDate = fakeContest.ContestDate };
            DbSet<Contest> mockSet = CustomTestUtils.FakeDbSet<Contest>(new List<Contest> { oneExisting });
            this.dbContext.Contests.Returns(mockSet);

            Assert.Throws<InvalidOperationException>(() => this.service.CreateContest(fakeContest));

            this.dbContext.DidNotReceiveWithAnyArgs().SaveChanges();
        }

        // Helper method to prevent code duplication
        private Contest GetFakeContest()
        {
            return new Contest
            {
                Name = "Contest Name",
                ContestDate = DateTime.Today.AddDays(10)
            };
        }
    }
}
