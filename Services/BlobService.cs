using System;
using System.IO;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace platzi_blobConsole.Services
{
    public static class BlobService
    {
        private static void UploadFile(IConfigurationRoot config)
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