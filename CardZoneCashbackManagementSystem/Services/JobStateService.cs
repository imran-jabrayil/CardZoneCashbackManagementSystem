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

    public async Task<JobState?> GetJobState(string jobName)
    {
        return await _unitOfWork.JobStateRepository.GetJobState(jobName);
    }

    public async Task UpdateJobState(string jobName, DateTime lastExecutionDay)
    {
        await _unitOfWork.JobStateRepository.UpdateJobState(jobName, lastExecutionDay);
        await _unitOfWork.SaveAsync();
    }
}