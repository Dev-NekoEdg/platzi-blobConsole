namespace platzi_blobConsole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text.Json;
    using System.IO;
    using Microsoft.Azure.Cosmos;
    using platzi_blobConsole.Models;
    using Microsoft.Extensions.Configuration;
    using Azure.Data.Tables;
    using System.Configuration;
    using Azure.Data.Tables.Models;

    public static class AzureTableService
    {
        /// <sumary>
        /// Create DB, container and a table to work.
        /// </sumary>
        public static async Task<Container> CreateDB(AzureTableConfiguration config)
        {
            CosmosClient cosmosClient = new CosmosClient(config.EndPoint,
                config.Key,
                new CosmosClientOptions() { AllowBulkExecution = true });

             Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(config.ContainerName);

            // Configure indexing policy to exclude all attributes to maximize RU/s usage
            Console.WriteLine($"Creating a container if not already exists...");
            await database.DefineContainer(config.ContainerName, config.PartitionKey)
                    .WithIndexingPolicy()
                    .WithIndexingMode(IndexingMode.Consistent)
                    .WithIncludedPaths()
                    .Attach()
                    .WithExcludedPaths()
                    .Path("/*")
                    .Attach() // Por qué dos .Attach()? | si se quita el attachda error en  el create async...
                    .Attach()
                    .CreateAsync();

            Container container = database.GetContainer(config.ContainerName);
            return container;
        }

        public static AzureTableConfiguration ConfigureConfig(IConfigurationSection config)
        {
            return new AzureTableConfiguration
            {
                ConnectionString = config["ConnectionString"].ToString(),
                EndPoint = config["EndPoint"].ToString(),
                Key = config["Key"].ToString(),
                AccountName = config["AccountName"].ToString(),
                TableName = config["TableName"].ToString(),
                ContainerName = config["ContainerName"].ToString(),
                PartitionKey = config["PartitionKey"].ToString(),
            };

        }

        public static async Task<TableServiceClient> CreateTableServiceClient(AzureTableConfiguration config)
        {
            TableServiceClient client = new(
                connectionString: config.ConnectionString
            );

            return client;
        }

        public static async Task<TableClient> CreateTable(TableServiceClient client, 
        AzureTableConfiguration config)
        {
            TableClient tableClient = client.GetTableClient(tableName: config.TableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public static async Task<TableItem> InsertItem(TableServiceClient client, AzureTableConfiguration config)
        {
            TableClient tableClient = client.GetTableClient(tableName: config.TableName);
            return await tableClient.CreateIfNotExistsAsync();
        }

    }
}