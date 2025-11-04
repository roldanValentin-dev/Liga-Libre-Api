using Amazon.SQS;
using Amazon.SQS.Model;
using LigaLibre.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace LigaLibre.Tests.Services;

public class SqsServiceTests
{
    private readonly Mock<IAmazonSQS> _mockSqs;
    private readonly Mock<ILogger<SqsService>> _mockLogger;
    private readonly SqsService _service;

    public SqsServiceTests()
    {
        _mockSqs = new Mock<IAmazonSQS>();
        _mockLogger = new Mock<ILogger<SqsService>>();
        _service = new SqsService(_mockSqs.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ValidMessage_SendsToQueue()
    {
        var message = new { EventType = "TestEvent", Id = 1 };
        var queueUrl = new GetQueueUrlResponse { QueueUrl = "https://sqs.test.com/queue" };
        var sendResponse = new SendMessageResponse { MessageId = "msg-123" };

        _mockSqs.Setup(s => s.GetQueueUrlAsync("test-queue", default)).ReturnsAsync(queueUrl);
        _mockSqs.Setup(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), default)).ReturnsAsync(sendResponse);

        await _service.SendMessageAsync(message, "test-queue");

        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Once);
    }

    [Fact]
    public async Task ReceiveMessageAsync_MessagesAvailable_ReturnsMessages()
    {
        var queueUrl = new GetQueueUrlResponse { QueueUrl = "https://sqs.test.com/queue" };
        var receiveResponse = new ReceiveMessageResponse
        {
            Messages = new List<Message>
            {
                new() { MessageId = "msg-1", Body = "{\"test\":\"data\"}", ReceiptHandle = "receipt-1" }
            }
        };

        _mockSqs.Setup(s => s.GetQueueUrlAsync("test-queue", default)).ReturnsAsync(queueUrl);
        _mockSqs.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default)).ReturnsAsync(receiveResponse);

        var result = await _service.ReceiveMessageAsync("test-queue");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("msg-1", result.First().MessageId);
    }

    [Fact]
    public async Task DeleteMessageAsync_ValidReceipt_DeletesMessage()
    {
        var queueUrl = new GetQueueUrlResponse { QueueUrl = "https://sqs.test.com/queue" };

        _mockSqs.Setup(s => s.GetQueueUrlAsync("test-queue", default)).ReturnsAsync(queueUrl);
        _mockSqs.Setup(s => s.DeleteMessageAsync(It.IsAny<string>(), It.IsAny<string>(), default)).ReturnsAsync(new DeleteMessageResponse());

        await _service.deleteMessageAsync("test-queue", "receipt-123");

        _mockSqs.Verify(s => s.DeleteMessageAsync(It.IsAny<string>(), "receipt-123", default), Times.Once);
    }
}
