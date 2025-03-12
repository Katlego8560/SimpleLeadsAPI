using EasyNetQ;
using SimpleLeadsAPI.Models;
using SimpleLeadsAPI.Services;

namespace SimpleLeadsAPI
{
    public class QueueProcessor(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider _ServiceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IServiceScope scope = _ServiceProvider.CreateScope();
            IBus bus = scope.ServiceProvider.GetRequiredService<IBus>();

            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await bus.PubSub.SubscribeAsync<LeadDTO>("new-lead", HandleNewLeadAsync);
        }

        private void HandleNewLeadAsync(LeadDTO dto)
        {
            Console.WriteLine(dto.ContactNumber);
            Console.WriteLine(dto.FullName);
        }
    }
}
