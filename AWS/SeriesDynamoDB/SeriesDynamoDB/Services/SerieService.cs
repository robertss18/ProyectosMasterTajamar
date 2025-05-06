using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using SeriesDynamoDB.Models;
using System.Reflection.PortableExecutable;

namespace SeriesDynamoDB.Services
{
    public class SerieService
    {

        private readonly DynamoDBContext _context;

        public SerieService(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }

        public async Task<List<Serie>> GetAllSeriesAsync()
        {
            var request = new List<ScanCondition>();
            return await _context.ScanAsync<Serie>(request).GetRemainingAsync();
        }

        public async Task<Serie> GetSerieByIdAsync(string id)
        {
            var serie = await _context.LoadAsync<Serie>(id);
            return serie;
        }

        public async Task<Serie> CreateSerieAsync(Serie serie)
        {
            serie.SerieId = Guid.NewGuid().ToString();
            await _context.SaveAsync(serie);
            return serie;
        }

        public async Task<Serie> UpdateSerieAsync(Serie serie)
        {
            await _context.SaveAsync(serie);
            return serie;
        }

        public async Task DeleteSerieAsync(string id)
        {
            var serie = await _context.LoadAsync<Serie>(id);
            if (serie != null)
            {
                await _context.DeleteAsync(serie);
            }
        }

    }
}
