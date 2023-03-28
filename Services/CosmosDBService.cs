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

    public static class CosmosDBService
    {
        /// <sumary>
        /// Create DB, container and a table to work.
        /// </sumary>
        public static async Task<Container> CreateDB(CosmosConfiguration config)
        {
            CosmosClient cosmosClient = new CosmosClient(config.EndPoint, 
                config.Key,
                new CosmosClientOptions() { AllowBulkExecution = true });

                Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(config.DatabaseName);

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
            return container ;
        }


    }
}