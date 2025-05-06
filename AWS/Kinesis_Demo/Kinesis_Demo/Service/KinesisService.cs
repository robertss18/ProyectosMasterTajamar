using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

namespace Kinesis_Demo.Service
{
    public class KinesisService
    {

        private readonly AmazonKinesisClient? _kinesisClient;
        private readonly string? _streamName;
        private readonly IAmazonS3 _s3Client;
        private readonly string? _bucketName;
        public KinesisService(IConfiguration configuration, IAmazonS3 s3Client)
        {
            var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);
            _kinesisClient = new AmazonKinesisClient(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"], region);
            _streamName = configuration["AWS:StreamName"];
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"];

        }


        //Método para enviar mensajes a Kinesis
        public async Task SendMessageAsync(string message)
        {
            var request = new PutRecordRequest
            {
                StreamName = _streamName,
                Data = new MemoryStream(Encoding.UTF8.GetBytes(message)),
                PartitionKey = Guid.NewGuid().ToString()
            };
            await _kinesisClient!.PutRecordAsync(request);
        }

        //Método para recibir mensajes de Kinesis (Hasta 10)
        public async Task<List<MessageWithIndex>> ReadMessagesAsync(int count = 10)
        {
            var messages = new List<MessageWithIndex>();
            int index = 1;

            var describeRequest = new DescribeStreamRequest
            {
                StreamName = _streamName
            };
            var streamDesc = await _kinesisClient!.DescribeStreamAsync(describeRequest);
            var shards = streamDesc.StreamDescription.Shards;

            foreach (var shard in shards)
            {
                var iteratorRequest = new GetShardIteratorRequest
                {
                    StreamName = _streamName,
                    ShardId = shard.ShardId,
                    ShardIteratorType = ShardIteratorType.TRIM_HORIZON
                };

                var iteratorResponse = await _kinesisClient.GetShardIteratorAsync(iteratorRequest);

                var shardIterator = iteratorResponse.ShardIterator; //Aquí tengo el conteo de los mensajes

                var recordRequest = await _kinesisClient.GetRecordsAsync(new GetRecordsRequest
                {
                    ShardIterator = shardIterator,
                    Limit = count
                });

                foreach (var record in recordRequest.Records)
                {
                    string data = Encoding.UTF8.GetString(record.Data.ToArray());
                    messages.Add(new MessageWithIndex
                    {
                        Index = index++,
                        Message = data
                    });

                }
            }
            return messages;


        }

        //Método para guardar Un mensaje en Amazon S3
        public async Task SaveMessageToS3(string message, int index)
        {
            var key = $"message/{index}_{Guid.NewGuid()}.txt";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var stream = new MemoryStream(messageBytes);

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = "text/plain"
            };

            //Subimos al S3
            await _s3Client.PutObjectAsync(putRequest);
        }


        public class MessageWithIndex
        {
            public int Index { get; set; }
            public string? Message { get; set; }
        }
    }
}
