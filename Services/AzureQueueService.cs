using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace platzi_blobConsole.Services
{
    public static class AzureQueueService
    {
        public static QueueConfigurationModel ConfigureConfig(IConfigurationSection config)
        {
            return new QueueConfigurationModel
            {
                ConnectionString = config["ConnectionString"],
                QueueName = config["QueueName"]
            };
        }


        public static IList<StandartMessageModel> GenerateMessages(int customMessageAmount, int moviesMessageAmount)
        {
            var listReturn = new List<StandartMessageModel>();

            for (int i = 0; i < customMessageAmount; i++)
            {
                var customMessageModel = new CustomMessageModel
                {
                    Age = i,
                    Email = $"test_0{i}@pruebas.com",
                    Subject = $"test_0{i}",
                    Name = "pruebas pruebas"
                };
                listReturn.Add(new StandartMessageModel
                {
                    MessageType = nameof(CustomMessageModel),
                    MessageBody = JsonSerializer.Serialize(customMessageModel)
                });
            }

            for (int i = 0; i < moviesMessageAmount; i++)
            {
                var moviesMessageModel = new MoviesMessageModel
                {
                    Director = $"director {i}",
                    Name = $"Title name #{i}",
                    Year = 2000 + i
                };
                listReturn.Add(new StandartMessageModel
                {
                    MessageType = nameof(MoviesMessageModel),
                    MessageBody = JsonSerializer.Serialize(moviesMessageModel)
                });
            }

            return listReturn;
        }



        public static async Task SendMessage<T>(T entity, QueueConfigurationModel config)
        {
            await using (var client = new ServiceBusClient(config.ConnectionString))
            {

                var sender = client.CreateSender(config.QueueName);
                // var queueClient = new QueueClient(config.ConnectionString, config.QueueName);
                string messageBody = JsonSerializer.Serialize(entity);

                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
                await sender.SendMessageAsync(message);
            }
        }

        public static async Task<IList<T>> ReadMessage<T>(QueueConfigurationModel config)
        {
            var listMessages = new List<T>();
            await using (var serviceBusClient = new ServiceBusClient(config.ConnectionString))
            {

                var sender = serviceBusClient.CreateReceiver(config.QueueName);
                // var queue = new QueueClient(config.ConnectionString, config.QueueName);

                var messages = await sender.ReceiveMessagesAsync(maxMessages: 10, maxWaitTime: null, cancellationToken: default);
                foreach (var message in messages)
                {
                    var x = JsonSerializer.Deserialize<T>(message.Body);
                    listMessages.Add(x);
                    
                    await sender.CompleteMessageAsync(message);
                }
            }
            return listMessages;
        }
    }
}