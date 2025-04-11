using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class BatchesServiceTests : ServiceTestBase
    {
        private readonly BatchService _service;

        public BatchesServiceTests()
        {
            _service = new BatchService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_batch()
        {
            var batch = new Batch
            {
                Code = "B001",
                Description = "Test batch",
                Date = DateTime.Today
            };
            DbContext.Batches.Add(batch);
            DbContext.SaveChanges();

            var result = await _service.GetById(batch.Id);

            Assert.NotNull(result);
            Assert.Equal("B001", result.Code);
        }


        [Fact]
        public async Task Get_should_return_null_for_invalid_id()
        {
            var result = await _service.GetById(-1);
            Assert.Null(result);
        }

        [Fact]
        public async Task List_should_return_all_batches()
        {
            // Arrange: добавляем два объекта Batch с обязательными полями
            DbContext.Batches.AddRange(
                new Batch { Code = "A", Description = "Batch A", Date = DateTime.Today },
                new Batch { Code = "B", Description = "Batch B", Date = DateTime.Today.AddDays(1) }
            );
            DbContext.SaveChanges();

            // Act: выполняем метод List с реальными параметрами
            var result = await _service.List(1, 10, new BatchesSearch());

            // Assert: проверяем, что результат не null
            Assert.NotNull(result);
            Assert.NotNull(result.Results);  // Проверяем, что Results не null
            Assert.Equal(2, result.Results.Count);  // Проверяем, что в Results два элемента
        }



        [Fact]
        public async Task Save_should_create_new_batch()
        {
            var batch = new Batch { Code = "C001", Description = "Test Batch", Date = DateTime.Today };

            await _service.Save(batch);

            Assert.Equal(1, DbContext.Batches.Count());
            Assert.Equal("C001", DbContext.Batches.First().Code);
        }


        [Fact]
        public async Task Save_should_update_existing_batch()
        {
            var batch = new Batch { Code = "ToUpdate", Description = "Batch to update", Date = DateTime.Today };
            DbContext.Batches.Add(batch);
            DbContext.SaveChanges();

            batch.Code = "Updated";
            await _service.Save(batch);

            var updated = DbContext.Batches.First();
            Assert.Equal("Updated", updated.Code);
        }


        [Fact]
        public async Task Delete_should_remove_existing_batch()
        {
            var batch = new Batch
            {
                Code = "ToDelete",
                Description = "Test delete batch",
                Date = DateTime.Today
            };
            DbContext.Batches.Add(batch);
            DbContext.SaveChanges();

            await _service.Delete(batch.Id);

            Assert.Empty(DbContext.Batches);
        }


        [Fact]
        public async Task Delete_should_do_nothing_for_nonexistent_batch()
        {
            await _service.Delete(-999);

            Assert.Empty(DbContext.Batches);
        }
    }
}
