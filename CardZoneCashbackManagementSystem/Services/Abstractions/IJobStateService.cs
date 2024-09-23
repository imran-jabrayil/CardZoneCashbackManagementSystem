using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Services.Abstractions;

public interface IJobStateService
{
    Task<JobState?> GetJobState(string jobName);
    Task UpdateJobState(string jobName, DateTime lastExecutionDay);
}