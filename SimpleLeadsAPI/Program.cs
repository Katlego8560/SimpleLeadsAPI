using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using SimpleLeadsAPI;
using SimpleLeadsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEasyNetQ("host=localhost").UseSystemTextJson();
builder.Services.AddHostedService<QueueProcessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
