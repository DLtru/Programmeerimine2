using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class BatchesServiceTests : HomeServiceTestBase
    {
        private readonly IBatchService _service;
        private readonly DateTime _currentDate = DateTime.Parse("2025-05-30 11:42:46");
        private const string _currentUser = "DLtru";

        public BatchesServiceTests()
        {
            _service = new BatchService(DbContext);
        }

        [Fact]
        public async Task List_should_return_all_batches()
        {
            // Arrange
            var batches = new[]
            {
            new Batch { Code = "B1", Description = "Test 1", Date = _currentDate },
            new Batch { Code = "B2", Description = "Test 2", Date = _currentDate }
        };

            DbContext.Batches.AddRange(batches);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.List(1, 10, new BatchesSearch());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Total);
        }

        [Fact]
        public async Task GetById_should_return_batch_by_id()
        {
            // Arrange
            var batch = new Batch
            {
                Code = "B1",
                Description = "Test Batch",
                Date = _currentDate
            };
            DbContext.Batches.Add(batch);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetById(batch.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("B1", result.Code);
        }

        [Fact]
        public async Task AddBatchAsync_should_add_new_batch()
        {
            // Arrange
            var batch = new Batch
            {
                Code = "B1",
                Description = "New Batch",
                Date = _currentDate
            };

            // Act
            await _service.AddBatchAsync(batch);

            // Assert
            var saved = await DbContext.Batches.FindAsync(batch.Id);
            Assert.NotNull(saved);
            Assert.Equal("B1", saved.Code);
        }

        [Fact]
        public async Task UpdateBatchAsync_should_update_existing_batch()
        {
            // Arrange
            var batch = new Batch
            {
                Code = "B1",
                Description = "Original Batch",
                Date = _currentDate
            };
            DbContext.Batches.Add(batch);
            await DbContext.SaveChangesAsync();

            // Act
            batch.Description = "Updated Batch";
            await _service.UpdateBatchAsync(batch);

            // Assert
            var updated = await DbContext.Batches.FindAsync(batch.Id);
            Assert.NotNull(updated);
            Assert.Equal("Updated Batch", updated.Description);
        }

        [Fact]
        public async Task Delete_should_remove_existing_batch()
        {
            // Arrange
            var batch = new Batch
            {
                Code = "B1",
                Description = "To Delete",
                Date = _currentDate
            };
            DbContext.Batches.Add(batch);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(batch.Id);

            // Assert
            var deleted = await DbContext.Batches.FindAsync(batch.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_should_not_throw_for_non_existing_batch()
        {
            // Act & Assert
            await _service.Delete(999); // Не должно вызывать исключение
        }

        [Fact]
        public async Task BatchExists_should_return_true_for_existing_batch()
        {
            // Arrange
            var batch = new Batch
            {
                Code = "B1",
                Description = "Test Batch",
                Date = _currentDate
            };
            DbContext.Batches.Add(batch);
            await DbContext.SaveChangesAsync();

            // Act
            var exists = await _service.BatchExists(batch.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task BatchExists_should_return_false_for_non_existing_batch()
        {
            // Act
            var exists = await _service.BatchExists(999);

            // Assert
            Assert.False(exists);
        }
    }