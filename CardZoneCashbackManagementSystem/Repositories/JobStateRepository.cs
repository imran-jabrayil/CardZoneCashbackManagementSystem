using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CardZoneCashbackManagementSystem.Repositories;

public class JobStateRepository : IJobStateRepository
{
    private readonly ILogger<JobStateRepository> _logger;
    private readonly AppDbContext _dbContext;

    public JobStateRepository(
        ILogger<JobStateRepository> logger,
        AppDbContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<JobState?> GetJobState(string jobName)
    {
        return await _dbContext.JobStates.FirstOrDefaultAsync(js => js.JobName == jobName);
    }

    public async Task UpdateJobState(string jobName, DateTime lastExecutionDay)
    {
        var jobState = await _dbContext.JobStates.FirstOrDefaultAsync(js => js.JobName == jobName);

        if (jobState is null)
        {
            _logger.LogError("could not find job state for {JobName}", jobName);
            return;
        }

        jobState.LastExecutionDay = lastExecutionDay;
    }
}