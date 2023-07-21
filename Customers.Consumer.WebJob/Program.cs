using Amazon.SQS;
using Customers.Consumer.WebJob;
using Customers.Consumer.WebJob.Settings;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// DI
#region Added Dependencies
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
//builder.Services.AddMediatR(typeof(Program)); => got me problems
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
#endregion
var app = builder.Build();
var queueName = args.Length == 1 ? args[0] : "customers";
app.Run();
