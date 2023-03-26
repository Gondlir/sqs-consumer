using Amazon.SQS;
using Customers.Consumer.WebJob;
using Customers.Consumer.WebJob.Settings;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
// DI
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddMediatR(typeof(Program));
//app.MapGet("/", () => "Hello World!");
var app = builder.Build();
app.Run();
