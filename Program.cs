using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Configuration.Json;
using Microsoft.Azure;
using Microsoft.Azure.Storage.Blob;

using Microsoft.Azure.Storage;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Security;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Azure;
using platzi_blobConsole.Models;
using System.Reflection.Metadata;
using platzi_blobConsole.Services;
using System.Collections.Generic;
using Microsoft.Azure.Storage.Shared.Protocol;
using System.Text.Json;
using System.Net.Mail;
using Microsoft.Azure.Amqp.Framing;
using System.Collections;

namespace platzi_blobConsole
{
    class Program
    {

        // public static IConfiguration config;
        public static IConfigurationRoot config;

        static async Task Main(string[] args)
        {
            // Cambiar el appsettings.
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Local.json");

            config = builder.Build();

            var azureQueueSection = config.GetSection("QueueConfiguration");
            var azureQueueConfig = AzureQueueService.ConfigureConfig(azureQueueSection);

            var listMessages = AzureQueueService.GenerateMessages(2, 1);

            foreach (var m in listMessages)
            {
                await AzureQueueService.SendMessage<StandartMessageModel>(m, azureQueueConfig);
            }

            var gotMessages = await AzureQueueService.ReadMessage<StandartMessageModel>(azureQueueConfig);

            foreach (var item in gotMessages)
            {
                switch (item.MessageType)
                {
                    case "MoviesMessageModel":
                        var x = JsonSerializer.Deserialize<MoviesMessageModel>(item.MessageBody);
                        Console.WriteLine(x.Director);
                        Console.WriteLine(x.Name);
                        Console.WriteLine(x.Year);
                        break;
                    case "CustomMessageModel":
                        var y = JsonSerializer.Deserialize<CustomMessageModel>(item.MessageBody);
                        Console.WriteLine(y.Age);
                        Console.WriteLine(y.Name);
                        Console.WriteLine(y.Subject);
                        Console.WriteLine(y.Email);
                        break;
                }
            }
        }

        /// <summary>
        /// sending a message to a Queue.
        /// </summary>
        /// <returns></returns>
        private static async Task QueueMessaging()
        {
            var azureQueueSection = config.GetSection("QueueConfiguration");
            var azureQueueConfig = AzureQueueService.ConfigureConfig(azureQueueSection);
            var message = new CustomMessageModel
            {
                Age = 20,
                Email = "pruebas@pruebas.com",
                Name = "pruebas pruebas",
                Subject = "test 022"
            };
            // Send message to the queue.
            await AzureQueueService.SendMessage<CustomMessageModel>(message, azureQueueConfig);
            // Read and Complete queue's messages.
            var result = await AzureQueueService.ReadMessage<CustomMessageModel>(azureQueueConfig);
        }


        /// <summary>
        /// CRUD with Azure tables (Cosmos DB).
        /// </summary>
        /// <returns></returns>
        private static async Task CosmosDBImplementation()
        {
            var azureTableSectionConfig = config.GetSection("AzureTableConfiguration");
            var azureTableConfig = AzureTableService.ConfigureConfig(azureTableSectionConfig);

            var tableServiceClient = await AzureTableService.CreateTableServiceClient(azureTableConfig);
            var tableClient = await AzureTableService.CreateTable(tableServiceClient, azureTableConfig);

            // **** Insert ****
            Pet newPet = new()
            {
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = azureTableConfig.PartitionKey,
                Name = "Michi-chan",
                Color = "Blanco",
                PetType = "Felino",
                Age = 5
            };
            await tableClient.AddEntityAsync<Pet>(newPet);

            // **** Get and update data ****
            // var getData = await tableClient.GetEntityIfExistsAsync<Pet>(azureTableConfig.PartitionKey, "ce37de31-aa30-4748-bc98-e2f2eeb3add8", default, default);
            // getData.Value.Color = "blanco con cola y cabeza cafe";
            // await tableClient.UpsertEntityAsync<Pet>(getData.Value);

            // **** Delete data ****
            // await tableClient.DeleteEntityAsync(azureTableConfig.PartitionKey, "test-02");

        }

        /// <summary>
        /// Upload files to Azure Blob storage.
        /// </summary>
        /// <returns></returns>
        private static async Task BlobStorage()
        {
            #region Upload files to AzureContainer

            BlobService.MimetypesAzures = config.GetSection("CommonMIMETypes").Get<Dictionary<string, string>>();
            var blobSectionConfig = config.GetSection("BlobContainerConfiguration");
            var blobConfig = BlobService.ConfigureConfig(blobSectionConfig);

            BlobService.UploadFile(blobConfig);

            #endregion
        }

    }
}
