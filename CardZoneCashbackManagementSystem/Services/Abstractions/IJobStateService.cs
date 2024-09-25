using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Services.Abstractions;

public interface IJobStateService
{
    Task<JobState?> GetJobStateAsync(string jobName);
    Task UpdateJobStateAsync(string jobName, DateTime lastExecutionDay);
}