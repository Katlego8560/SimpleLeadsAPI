using EasyNetQ;
using Messages;
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
            await bus.PubSub.SubscribeAsync<LeadMessage>("new-lead", HandleNewLeadAsync);
        }

        private void HandleNewLeadAsync(LeadMessage dto)
        {
            Console.WriteLine("Lead received");
            Console.WriteLine(dto.ContactNumber);
            Console.WriteLine(dto.FullName);
        }
    }
}
