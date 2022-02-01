using Azure.Storage.Queues;
using AzureStorageTest.Models;

namespace AzureStorageTest;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly QueueClient _queueClient;

    public Worker(ILogger<Worker> logger, QueueClient queueClient)
    {
        _logger = logger;
        _queueClient = queueClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Listening queue");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Fetching messages");
            var messages = await _queueClient.ReceiveMessagesAsync();

            if (messages.Value != null)
            {
                foreach (var message in messages.Value)
                {
                    var notification = message.Body.ToObjectFromJson<Notification>();
                    _logger.LogInformation("Message - {Message}", notification.Message);

                    await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                }
            }

            await Task.Delay(millisecondsDelay: 5000, stoppingToken);
        }
    }
}