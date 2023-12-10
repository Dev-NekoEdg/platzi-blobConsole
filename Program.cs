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

            #region Upload files to AzureContainer
            /*
            BlobService.MimetypesAzures = config.GetSection("CommonMIMETypes").Get<Dictionary<string, string>>();
            var blobSectionConfig = config.GetSection("BlobContainerConfiguration");
            var blobConfig = BlobService.ConfigureConfig(blobSectionConfig);
            
            BlobService.UploadFile(blobConfig);
            */
            #endregion

            var azureTableSectionConfig = config.GetSection("AzureTableConfiguration");
            var azureTableConfig = AzureTableService.ConfigureConfig(azureTableSectionConfig);

            var tableServiceClient = await AzureTableService.CreateTableServiceClient(azureTableConfig);
            var tableClient = await AzureTableService.CreateTable(tableServiceClient, azureTableConfig);

            Pet newPet = new()
            {
                PartitionKey = azureTableConfig.PartitionKey,
                Name = "Nikkita",
                Color = "Blanco",
                PetType = "Felino",
                Age = 9
            };

            var result = await tableClient.AddEntityAsync<Pet>(newPet);

        }

        private static async Task CreateDB(AzureTableConfiguration cosmosConfiguration)
        {
            CosmosClient cosmosClient = new CosmosClient(cosmosConfiguration.EndPoint, cosmosConfiguration.Key);
            var db = cosmosClient.GetDatabase("TablesDB");

            // var createdDB = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosConfiguration.DatabaseName);

            // var containerProps = new ContainerProperties { Id = cosmosConfiguration.AccountName };
            // await createdDB.Database.CreateContainerIfNotExistsAsync(containerProps);

            //var db = cosmosClient.GetDatabase(cosmosConfiguration.DatabaseName);
            // await db.CreateContainerIfNotExistsAsync(containerProps);

            // Base --> https://www.youtube.com/watch?v=DTb4-79aNNo
            // base 2 --> https://www.youtube.com/watch?v=5ZU2xA_Y3G8

        }
    }
}
