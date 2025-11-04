using Amazon.SQS;
using Amazon.SQS.Model;
using LigaLibre.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace LigaLibre.Infrastructure.Services;

public class SqsService(IAmazonSQS sqsClient, ILogger<SqsService> logger) : ISqsService
{

    //borrar mensaje de la cola
    public async Task deleteMessageAsync(string queueName, string receiptHandle)
    {
        var queueUrl = await sqsClient.GetQueueUrlAsync(queueName);

        await sqsClient.DeleteMessageAsync(queueUrl.QueueUrl, receiptHandle);

        logger.LogInformation($"Mensaje eliminado de la queue {queueName}. ReceiptHandle: {receiptHandle}");
    }

    //recibir mensajes de la cola
    public async Task<IEnumerable<QueueMessage>> ReceiveMessageAsync(string queueName, int maxMessage = 10)
    {
        try
        {
            //obtenemos la url de la cola
            var queueUrl = await sqsClient.GetQueueUrlAsync(queueName);
            //creamos la solicitud para recibir mensajes
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl.QueueUrl,
                MaxNumberOfMessages = maxMessage,
                WaitTimeSeconds = 5
            };
            //recibimos los mensajes
            var response = await sqsClient.ReceiveMessageAsync(request);
            //verificamos si hay mensajes
            if (response.Messages == null)
            {
                return new List<QueueMessage>();
            }
            //mapeamos los mensajes a la clase QueueMessage
            return response.Messages.Select(m => new QueueMessage
            {
                MessageId = m.MessageId,
                Body = m.Body,
                ReceiptHandle = m.ReceiptHandle
            });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al recibir mensajes de la queue {queueName}: {ex.Message}");
            return Enumerable.Empty<QueueMessage>();
        }
    }

    //enviar mensaje a la cola
    public async Task SendMessageAsync<T>(T message, string queueName, int delaySeconds = 0)
    {
        var queueUrl = await sqsClient.GetQueueUrlAsync(queueName);
        var messageBody = JsonSerializer.Serialize(message);

        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = messageBody,
            DelaySeconds = delaySeconds

        };
        var response = await sqsClient.SendMessageAsync(request);

        logger.LogInformation($"Mensaje enviado a la queue {queueName}. MessageId: {response.MessageId}");
    }
}
