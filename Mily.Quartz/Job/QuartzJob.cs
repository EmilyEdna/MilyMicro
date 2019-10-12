using Quartz;
using System.Threading.Tasks;

namespace Mily.Quartz.Job
{
    public class QuartzJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}