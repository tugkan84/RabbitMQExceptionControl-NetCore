using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string customRetryHeaderName = "number-of-retries";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            MongoServices.Services.DataAccess db = new MongoServices.Services.DataAccess();

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                MessageWithError newMsg = null;
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    newMsg = JsonConvert.DeserializeObject<MessageWithError>(message);
                    int retryCount = ReceiverOps.GetRetryCount(ea.BasicProperties, customRetryHeaderName);
                    Console.WriteLine("[x] Recieved {0},{1},Header Count:{2}", newMsg.Comment, newMsg.ErrorCount, retryCount);

                    if (retryCount == 5)
                    {
                        channel.BasicNack(deliveryTag: ea.DeliveryTag, requeue: false, multiple: false);
                        Console.WriteLine("Message {0} deleted from query and added to MongoDb. Retry count: {1}", message, retryCount);
                        
                        db.Create(new MongoServices.Models.MongoMessage{ MessageId = newMsg.Id, Comment= newMsg.Comment });
                    }
                    else
                    {
                        IBasicProperties propertiesForCopy = channel.CreateBasicProperties();
                        IDictionary<string, object> headersCopy = ReceiverOps.CopyHeaders(ea.BasicProperties);
                        propertiesForCopy.Headers = headersCopy;
                        propertiesForCopy.Headers[customRetryHeaderName] = ++retryCount;
                        channel.BasicPublish(ea.Exchange, ea.RoutingKey, propertiesForCopy, ea.Body);
                        channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine("Message {0} thrown back at queue for retry. New retry count: {1}", message, retryCount);
                    }
                    // channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                    // channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "hello", autoAck: false, consumer: consumer);
                Console.WriteLine(" Press [enter] to exit.");
                db.RemoveAll();
                Console.ReadLine();
            }
        }
    }
}
