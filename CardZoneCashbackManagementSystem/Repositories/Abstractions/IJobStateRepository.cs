using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Repositories.Abstractions;

public interface IJobStateRepository
{
    Task<JobState?> GetJobState(string jobName);
    Task UpdateJobState(string jobName, DateTime lastExecutionDay);
}