


namespace SimpleLeadsAPI
{
    public class QueueProcessor(IServiceProvider serviceProvider) : BackgroundService
    {

        private readonly IServiceProvider _ServiceProvider = serviceProvider;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
