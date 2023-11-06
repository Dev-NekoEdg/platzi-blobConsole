namespace platzi_blobConsole.Models
{
    public class CosmosConfiguration
    {
        public string EndPoint { get; set; }

        public string Key { get; set; }

        public string AccountName { get; set; }

        public string DatabaseName { get; set; }

        public string ContainerName { get; set; }
        
        public string PartitionKey { get; set; }
    }
}