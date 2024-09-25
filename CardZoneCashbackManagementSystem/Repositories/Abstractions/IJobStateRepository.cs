using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Repositories.Abstractions;

public interface IJobStateRepository
{
    Task<JobState?> GetJobStateAsync(string jobName);
    Task UpdateJobStateAsync(string jobName, DateTime lastExecutionTime);
}