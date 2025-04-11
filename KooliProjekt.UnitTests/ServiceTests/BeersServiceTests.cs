using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.UnitTests.ServiceTestBase;
using KooliProjekt.UnitTests.ControllerTests;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class BeersServiceTests : BeersServiceTestBase
    {
        private readonly BeerService _service;

        public BeersServiceTests()
        {
            _service = new BeerService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_beer()
        {
            // Arrange: Create and add a new beer to the DbContext
            var beer = new Beer
            {
                Code = "B001",
                Name = "Test Beer",
                AlcoholContent = 5.0,
                Description = "A test beer",
                BrewingDate = DateTime.Today,
                Type = "Lager"  // Add the missing Type property
            };
            DbContext.Beers.Add(beer);
            await DbContext.SaveChangesAsync();  // Ensure SaveChanges is awaited

            // Act: Retrieve the beer using the GetById method
            var result = await _service.GetBeerByIdAsync(beer.Id);

            // Assert: Check if the result is not null and the properties are correct
            Assert.NotNull(result);
            Assert.Equal("B001", result.Code);
            Assert.Equal("Test Beer", result.Name);
            Assert.Equal(5.0, result.AlcoholContent);
        }



        [Fact]
        public async Task Get_should_return_null_for_invalid_id()
        {
            var result = await _service.GetBeerByIdAsync(-1);
            Assert.Null(result);
        }

        [Fact]
        public async Task List_should_return_all_beers()
        {
            // Add test beers to the database with Type property
            DbContext.Beers.AddRange(
                new Beer { Code = "A", Name = "Beer A", AlcoholContent = 5.0, Description = "Beer A description", BrewingDate = DateTime.Today, Type = "Lager" },
                new Beer { Code = "B", Name = "Beer B", AlcoholContent = 6.0, Description = "Beer B description", BrewingDate = DateTime.Today.AddDays(1), Type = "IPA" }
            );
            DbContext.SaveChanges();

            // List beers using the correct BeersSearch class
            var result = await _service.List(1, 10, new BeersSearch());

            // Assert the results
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(2, result.Results.Count);
        }




        [Fact]
        public async Task Save_should_create_new_beer()
        {
            var beer = new Beer
            {
                Code = "C001",
                Name = "New Beer",
                AlcoholContent = 4.5,
                Description = "New Beer Description",
                BrewingDate = DateTime.Today,
                Type = "Lager" // Set the Type property here
            };

            await _service.Save(beer);

            Assert.Equal(1, DbContext.Beers.Count());
            Assert.Equal("C001", DbContext.Beers.First().Code);
        }


        [Fact]
        public async Task Save_should_update_existing_beer()
        {
            var beer = new Beer
            {
                Code = "ToUpdate",
                Name = "Beer to update",
                AlcoholContent = 5.5,
                Description = "To update beer",
                BrewingDate = DateTime.Today,
                Type = "Lager" // Ensure the Type is set here
            };

            DbContext.Beers.Add(beer);
            DbContext.SaveChanges();

            beer.Name = "Updated Beer";
            await _service.Save(beer);

            var updated = DbContext.Beers.First();
            Assert.Equal("Updated Beer", updated.Name);
        }


        [Fact]
        public async Task Delete_should_remove_existing_beer()
        {
            var beer = new Beer
            {
                Code = "ToDelete",
                Name = "Test beer to delete",
                AlcoholContent = 5.0,
                Description = "Delete test beer",
                BrewingDate = DateTime.Today,
                Type = "Lager"  // Add a value for the missing Type property
            };

            DbContext.Beers.Add(beer);
            DbContext.SaveChanges();

            await _service.DeleteBeerAsync(beer.Id);

            Assert.Empty(DbContext.Beers);
        }


        [Fact]
        public async Task Delete_should_do_nothing_for_nonexistent_beer()
        {
            await _service.DeleteBeerAsync(-999);

            Assert.Empty(DbContext.Beers);
        }
    }
}
