using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using platzi_blobConsole.Models;

namespace platzi_blobConsole.Services
{
    public static class BlobService
    {
        public static Dictionary<string, string> MimetypesAzures;

        public static BlobConfiguration ConfigureConfig(IConfigurationSection config)
        {
            return new BlobConfiguration
            {
                Connection = config["StorageAcccount"].ToString(),
                OriginPath = config["OriginDestination"].ToString(),
                DestinationPath = config["DestinationContainer"].ToString(),
                Prefix = config["CompletePrefix"].ToString(),
            };

        }

        public static void UploadFile(BlobConfiguration config)
        {
            // Console.WriteLine("Hello World!");
            Console.WriteLine(config.Connection);

            Console.WriteLine($"Conexion de Azure: {config.Connection}");

            CloudStorageAccount cuentaAlmacenamiento = CloudStorageAccount.Parse(config.Connection);
            CloudBlobClient client = cuentaAlmacenamiento.CreateCloudBlobClient();

            // los containers no aceptan mayusculas.
            CloudBlobContainer container = client.GetContainerReference(config.DestinationPath);
            // Crea el container si no existe.
            container.CreateIfNotExists();

            // Colocar permisos para poder subir imagenes.
            container.SetPermissions(new BlobContainerPermissions { 
                PublicAccess = BlobContainerPublicAccessType.Blob 
                });

            // string newName = Guid.NewGuid().ToString() + ".jpg";
            // CloudBlockBlob miBlod = container.GetBlockBlobReference(newName);
            // string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var currentDirectory = Directory.GetCurrentDirectory(); 
            // string imageRoot = Path.Combine(baseFolderImg, "FXgvpruWIAE4uNr.jpg");
            var imagesRoot = Path.Combine(currentDirectory, config.OriginPath);
            var filesInFolder = Directory.GetFiles(imagesRoot);
            var filesFiltered = filesInFolder.ToList().Where(x=> !x.Contains(config.Prefix));
            foreach(var rootFile in filesFiltered)
            {
                var x = GetMIMEContent(Path.GetExtension(rootFile));
                
                // var newNameBlob = string.Concat(Guid.NewGuid().ToString(),Path.GetExtension(rootFile));
                
                // CloudBlockBlob miBlod = container.GetBlockBlobReference(newNameBlob.Replace("-",""));
                // var file = File.OpenRead(rootFile);
                // miBlod.UploadFromStream(file);
                // miBlod.Properties.ContentType = GetMIMEContent(Path.GetExtension(rootFile));
                // miBlod.SetProperties();
                // // TODO: validateExtensions.
                // // miBlod.Properties.ContentType = "image/jpeg";
                // file.Close();

                // // Moving files already on blobstorage
                // var fileToMove = string.Concat(config.Prefix, Path.GetFileName(rootFile));

                // string completePathFileToMove = string.Concat(
                //     rootFile.Replace(Path.GetFileName(rootFile),string.Empty),
                //     fileToMove);

                // File.Delete(completePathFileToMove);
                // File.Move(rootFile, completePathFileToMove);
            }
        }

        private static string GetMIMEContent(string ext)
        {
            var mimeDefault= MimetypesAzures.Single(x=> x.Key.ToLower() =="default" );
            var mimeFound =  MimetypesAzures.FirstOrDefault(x=> ext.Contains(x.Value));
            return mimeFound.Equals(default) ? mimeDefault.Value : mimeDefault.Value;
        }
    }
}