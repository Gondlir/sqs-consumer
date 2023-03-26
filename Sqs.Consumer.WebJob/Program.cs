// See https://aka.ms/new-console-template for more information
using Sqs.Consumer.WebJob;
// A console project to consume the messages 
Console.WriteLine("Hello, World!");
await EventAWSConsumer.MainAsync();