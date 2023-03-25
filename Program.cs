﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Azure;
using Microsoft.Azure.Storage.Blob;

using Microsoft.Azure.Storage;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Security;

namespace platzi_blobConsole
{
    class Program
    {

        public static IConfigurationRoot config;

        static async Task Main(string[] args)
        {
            // Cambiar el appsettings.
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Local.json");

            config = builder.Build();

            // UploadFile();

            var cosmosConfig = new CosmosConfiguration();
            cosmosConfig.EndPoint = config.GetSection("CosmosConfiguration")["EndPoint"];
            cosmosConfig.Key = config.GetSection("CosmosConfiguration")["Key"];
            cosmosConfig.ContainerName = config.GetSection("CosmosConfiguration")["Container"];
            cosmosConfig.DatabaseName = config.GetSection("CosmosConfiguration")["DataBase"];

            await CreateDB(cosmosConfig);
        }



        private static async Task CreateDB(CosmosConfiguration cosmosConfiguration)
        {
            CosmosClient cosmosClient = new CosmosClient(cosmosConfiguration.EndPoint, cosmosConfiguration.Key);
            var db = cosmosClient.GetDatabase("TablesDB");



            var createdDB = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosConfiguration.DatabaseName);

            var containerProps = new ContainerProperties { Id = cosmosConfiguration.ContainerName };
            await createdDB.Database.CreateContainerIfNotExistsAsync(containerProps);

            //var db = cosmosClient.GetDatabase(cosmosConfiguration.DatabaseName);
            await db.CreateContainerIfNotExistsAsync(containerProps);

            // Base --> https://www.youtube.com/watch?v=DTb4-79aNNo
            // base 2 --> https://www.youtube.com/watch?v=5ZU2xA_Y3G8

        }


        private static void UploadFile()
        {
            string getstringConn1 = config["pruebas"];

            // Console.WriteLine("Hello World!");
            Console.WriteLine(getstringConn1);

            Console.WriteLine("Conexion de Azure");
            string getstring = config["AzureConnection"];

            CloudStorageAccount cuentaAlmacenamiento = CloudStorageAccount.Parse(config["AzureConnection"]);
            CloudBlobClient client = cuentaAlmacenamiento.CreateCloudBlobClient();

            // los containers no aceptan mayusculas.
            CloudBlobContainer container = client.GetContainerReference("containernuevo");
            // Crea el container si no existe.
            container.CreateIfNotExists();

            // Colocar permisos para poder subir imagenes.
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            string newName = Guid.NewGuid().ToString() + ".jpg";
            CloudBlockBlob miBlod = container.GetBlockBlobReference(newName);
            // string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string baseFolderImg = config["baseImageRoot"]; ;
            string imageRoot = Path.Combine(baseFolderImg, "FXgvpruWIAE4uNr.jpg");

            using (var file = System.IO.File.OpenRead(imageRoot))
            {
                miBlod.UploadFromStream(file);
            }

        }
    }
}
