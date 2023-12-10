using System;
using Azure;
using Azure.Data.Tables;

namespace platzi_blobConsole.Models
{
    public class AzureTableConfiguration
    {
        public string EndPoint { get; set; }

        public string Key { get; set; }

        public string AccountName { get; set; }

        public string TableName { get; set; }

        public string ContainerName { get; set; }
        
        public string PartitionKey { get; set; }

        public string ConnectionString { get; set; }   
    }

    public record Pet : ITableEntity
    {
        #region ITableEntity Properties implementations
        public string PartitionKey { get; set; }
        public string RowKey { 
            get => Guid.NewGuid().ToString(); 
            set => RowKey = value; 
            }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
        #endregion

        public string Name { get; set; }

        public string Color { get; set; }
        public int Age { get; set; }
        public string PetType { get; set; }
    }
}