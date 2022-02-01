using Azure.Storage.Queues;
using AzureStorageTest.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace AzureStorageTest.Controllers;

[Route("api/notifications")]
[ApiController]
public sealed class NotificationController : ControllerBase
{
    private readonly QueueClient _queueClient;

    public NotificationController(QueueClient queueClient) => _queueClient = queueClient;

    [HttpPost]
    [SwaggerResponse(statusCode: 201)]
    [SwaggerOperation(Summary = "Add notification", Description = "Add a new notification in the queue")]
    public async Task<IActionResult> SendNotification([FromBody][Required] Notification notification,
                                                      CancellationToken cancellation = default)
    {
        var notificationInJson = JsonSerializer.Serialize(notification);
        var response = await _queueClient.SendMessageAsync(notificationInJson, cancellation);

        return Created(string.Empty, new { response.Value.MessageId });
    }
}
