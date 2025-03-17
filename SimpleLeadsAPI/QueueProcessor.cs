using EasyNetQ;
using Messages;
using SimpleLeadsAPI.Controllers;
using SimpleLeadsAPI.Models;
using SimpleLeadsAPI.Services;

namespace SimpleLeadsAPI
{
    public class QueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _ServiceProvider;
        private readonly ApplicationDbContext _Context;

        public QueueProcessor(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            _ServiceProvider = serviceProvider;
            _Context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IServiceScope scope = _ServiceProvider.CreateScope();
            IBus bus = scope.ServiceProvider.GetRequiredService<IBus>();

            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await bus.PubSub.SubscribeAsync<LeadMessage>("new-lead", message => {
                Console.WriteLine("Lead received");
                Console.WriteLine(message.ContactNumber);
                Console.WriteLine(message.FullName);
            });

            _Context.Leads.Add(new Lead()); 
            _Context.SaveChanges();
        }
    }
}
