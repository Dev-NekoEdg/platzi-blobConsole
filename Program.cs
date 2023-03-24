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
            // UploadFile();
        }

        

        private void UploadFile()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Local.json"); // cambiar a appsettings.

            // IConfiguration config = new ConfigurationBuilder()
            // .AddJsonFile("appsettings.json", true, true)
            // .Build();
            var config = builder.Build();

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
