using Amazon.DynamoDBv2.DataModel;

namespace SeriesDynamoDB.Models
{
    [DynamoDBTable("Series")]
    public class Serie
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("SerieId")]
        public string? SerieId { get; set; }
        [DynamoDBProperty("Titulo")]
        public string? Titulo { get; set; }
        [DynamoDBProperty("Genero")]
        public string? Genero { get; set; }
        [DynamoDBProperty("Temporadas")]
        public int? Temporadas { get; set; }
        [DynamoDBProperty("DisponibleEn")]
        public string? DisponibleEn { get; set; }
    }
}
