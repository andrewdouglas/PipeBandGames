using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PipeBandGames.DataLayer;

namespace PipeBandGames.DataLayer.Migrations
{
    [DbContext(typeof(PipeBandGamesContext))]
    [Migration("20170604202110_GamesMigration")]
    partial class GamesMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("PipeBandGames.DataLayer.Entities.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime?>("Deleted");

                    b.Property<int?>("DeletedBy");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<string>("Title");

                    b.Property<DateTime?>("Updated");

                    b.Property<int?>("UpdatedBy");

                    b.HasKey("PersonId");

                    b.ToTable("Person");
                });
        }
    }
}
