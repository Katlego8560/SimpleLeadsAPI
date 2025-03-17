using EasyNetQ;
using Messages;
using SimpleLeadsAPI.Controllers;
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

            await bus.PubSub.SubscribeAsync<LeadMessage>("new-lead", message => {
                
                dbContext.Leads.Add(new Lead()
                {
                    Id = Guid.NewGuid(),
                    FullName = message.FullName,
                    ContactNumber = message.ContactNumber,
                    CurrentlyInsured = message.CurrentlyInsured,
                    OtherInsurer = message.OtherInsurer,
                    Insurer = message.Insurer,
                });

                dbContext.SaveChanges();
            });

        
        }
    }
}
