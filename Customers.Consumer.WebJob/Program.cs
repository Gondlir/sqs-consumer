using Amazon.SQS;
using Customers.Consumer.WebJob;
using Customers.Consumer.WebJob.Settings;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// DI
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
//builder.Services.AddMediatR(typeof(Program)); => got me problems
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
//app.MapGet("/", () => "Hello World!");
var app = builder.Build();
app.Run();
