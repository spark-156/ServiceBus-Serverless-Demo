using Azure.Messaging.ServiceBus;
using System;

namespace RestAPI_2._0;

public class MessageSender : IMessageSender
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    
    public MessageSender()
    {
        const string connectionString = "<connection_string>";
        const string queueName = "<queue_name>";

        _client = new ServiceBusClient(connectionString);
        _sender = _client.CreateSender(queueName);
    }
    
    public async Task Send(string message)
    {
        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(message);
        await _sender.SendMessageAsync(serviceBusMessage);
        Console.WriteLine("Sent message: " + message);
    }
}