using AutoFixture.Xunit2;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services;
using FluentAssertions;
using Moq;

namespace CardZoneCashbackManagementSystem.UnitTests.Services;

public class JobStateServiceTests
{
    [Theory, InlineAutoData]
    public async Task GetJobStateAsync_ExceptionNotThrown(
        JobState jobState,
        Mock<IJobStateRepository> repository,
        Mock<IUnitOfWork> unitOfWork)
    {
        repository.Setup(r => r.GetJobStateAsync(jobState.JobName))
            .ReturnsAsync(jobState)
            .Verifiable();
        unitOfWork.Setup(uow => uow.JobStateRepository)
            .Returns(repository.Object)
            .Verifiable();
        
        var sut = new JobStateService(unitOfWork.Object);
        var actualJobState = await sut.GetJobStateAsync(jobState.JobName);

        actualJobState.Should().BeEquivalentTo(jobState);
        repository.Verify(r => r.GetJobStateAsync(jobState.JobName), Times.Once());
        unitOfWork.Verify(uow => uow.JobStateRepository, Times.Once());
    }

    [Theory, InlineAutoData]
    public async Task UpdateJobStateAsync_ExceptionNotThrown(
        JobState jobState,
        DateTime lastExecutionTime,
        Mock<IJobStateRepository> repository,
        Mock<IUnitOfWork> unitOfWork)
    {
        repository.Setup(r => r.UpdateJobStateAsync(jobState.JobName, lastExecutionTime))
            .Returns(Task.CompletedTask)
            .Verifiable();
        unitOfWork.Setup(uow => uow.JobStateRepository)
            .Returns(repository.Object)
            .Verifiable();
        
        var sut = new JobStateService(unitOfWork.Object);
        await sut.UpdateJobStateAsync(jobState.JobName, lastExecutionTime);
        
        repository.Verify(r => r.UpdateJobStateAsync(jobState.JobName, lastExecutionTime), Times.Once());
        unitOfWork.Verify(uow => uow.JobStateRepository, Times.Once());
    }
}