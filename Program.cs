using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Azure;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;

namespace platzi_blobConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            // IConfiguration config = new ConfigurationBuilder()
            // .AddJsonFile("appsettings.json", true, true)
            // .Build();
            var config = builder.Build();

            string getstringConn = config.GetSection("ConnectionStrings").Value;
            string getstringConn1 = config["pruebas"];

            // Console.WriteLine("Hello World!");
            Console.WriteLine(getstringConn);
            Console.WriteLine(getstringConn1);

            string getstring = config["AzureConnection"];

            CloudStorageAccount cuentaAlmacenamiento = CloudStorageAccount.Parse(config["AzureConnection"]);
            
        }
    }
}
