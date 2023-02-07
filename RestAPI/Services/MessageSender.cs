using Azure.Messaging.ServiceBus;
using System;
using Newtonsoft.Json;

namespace RestAPI.Services;

public class MessageSender : IMessageSender
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    
    public MessageSender(string topicName)
    {
        const string connectionString = "Endpoint=sb://servicebus-serverless-demo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=np/LDqsORggdtWjY4yrOo8FDhgswnMyPyb1cAFOCUyQ=";

        _client = new ServiceBusClient(connectionString);
        _sender = _client.CreateSender(topicName);
    }
    
    public async Task Send(object message)
    {
        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(message));
        await _sender.SendMessageAsync(serviceBusMessage);
        Console.WriteLine("Sent message: " + message);
    }
}