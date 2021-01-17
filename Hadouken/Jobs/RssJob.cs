using System.Threading.Tasks;

using Quartz;

namespace Hadouken.Jobs
{
    public class RssJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}