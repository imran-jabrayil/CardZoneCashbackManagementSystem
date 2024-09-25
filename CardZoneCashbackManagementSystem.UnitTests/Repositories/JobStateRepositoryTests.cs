using AutoFixture.Xunit2;
using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CardZoneCashbackManagementSystem.UnitTests.Repositories;

public class JobStateRepositoryTests
{
    private readonly AppDbContext _dbContext;

    public JobStateRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
    }

    [Theory, InlineAutoData]
    public async Task GetJobStateAsync_ExistingJobNamePassed_UpdatedJobStateReturned(
        JobState jobState, 
        Mock<ILogger<JobStateRepository>> logger)
    {
        _dbContext.JobStates.Add(jobState);
        await _dbContext.SaveChangesAsync();

        var sut = new JobStateRepository(logger.Object, _dbContext);
        var actualJobState = await sut.GetJobStateAsync(jobState.JobName);

        actualJobState.Should().BeEquivalentTo(jobState);
    }
    
    [Theory, InlineAutoData]
    public async Task GetJobStateAsync_NonExistingJobNamePassed_NullReturned(
        string jobName, 
        Mock<ILogger<JobStateRepository>> logger)
    {
        var sut = new JobStateRepository(logger.Object, _dbContext);
        var actualJobState = await sut.GetJobStateAsync(jobName);

        actualJobState.Should().BeNull();
    }

    [Theory, InlineAutoData]
    public async Task UpdateJobStateAsync_ExistingJobNamePassed_JobStateUpdated(
        JobState jobState,
        DateTime lastExecutionTime,
        Mock<ILogger<JobStateRepository>> logger)
    {
        _dbContext.JobStates.Add(jobState);
        await _dbContext.SaveChangesAsync();

        DateTime lastExecutionDay = lastExecutionTime.Date;
        
        var sut = new JobStateRepository(logger.Object, _dbContext);
        await sut.UpdateJobStateAsync(jobState.JobName, lastExecutionDay);

        jobState.LastExecutionDay.Should().Be(lastExecutionDay);
    }
}