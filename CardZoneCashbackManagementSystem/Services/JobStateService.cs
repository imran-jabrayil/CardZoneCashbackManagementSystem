using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;

namespace CardZoneCashbackManagementSystem.Services;

public class JobStateService : IJobStateService
{
    private readonly IUnitOfWork _unitOfWork;

    public JobStateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<JobState?> GetJobStateAsync(string jobName)
    {
        return await _unitOfWork.JobStateRepository.GetJobStateAsync(jobName);
    }

    public async Task UpdateJobStateAsync(string jobName, DateTime lastExecutionDay)
    {
        await _unitOfWork.JobStateRepository.UpdateJobStateAsync(jobName, lastExecutionDay);
        await _unitOfWork.SaveAsync();
    }
}