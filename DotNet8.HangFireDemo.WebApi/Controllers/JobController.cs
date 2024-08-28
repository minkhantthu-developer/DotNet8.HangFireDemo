using DotNet8.HangFireDemo.WebApi.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8.HangFireDemo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("CreateBackgroundJob")]
        public IActionResult CreateBackgroundJob()
        {
            // BackgroundJob.Enqueue(() => Console.WriteLine("Background Job Triggered"));
            BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("Background Job Triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateScheduledJob")]
        public IActionResult CreateScheduledJob()
        {
            var scheduledDateTime = DateTime.Now.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);
            //  BackgroundJob.Schedule(()=>Console.WriteLine("Scheduled Job Triggered"), dateTimeOffSet);
            BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Scheduled Job Triggered"), dateTimeOffSet);
            return Ok();
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        public IActionResult CreateContinuationJob()
        {
            var scheduledDateTime = DateTime.Now.AddSeconds(6);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);
            var jobId = BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Scheduled Jo Triggered"), dateTimeOffSet);
            var job2Id = BackgroundJob.ContinueJobWith<TestJob>(jobId, x => x.WriteLog("Continuation Job1 Triggered"));
            var job3Id = BackgroundJob.ContinueJobWith<TestJob>(job2Id, x => x.WriteLog("Continuation Job2 Triggered"));
            var job4Id = BackgroundJob.ContinueJobWith<TestJob>(job3Id, x => x.WriteLog("Continuation Job3 Triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public IActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate<TestJob>("Recurring Job 1", x => x.WriteLog("Recurring Job Triggered"), "* * * * *");
            return Ok();
        }
    }
}
