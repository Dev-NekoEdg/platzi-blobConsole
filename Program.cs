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
        }

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
    }
}
