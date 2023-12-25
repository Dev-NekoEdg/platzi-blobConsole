using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;

public static class QueueServices
{

    public static async Task CreateMessage<T>(T entity, QueueConfigurationModel config)
    {
        var queueClient = new QueueClient(config.ConnectionString, config.QueueName);

    }



}