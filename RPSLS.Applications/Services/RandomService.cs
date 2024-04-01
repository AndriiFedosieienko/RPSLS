using Newtonsoft.Json;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Models;

namespace RPSLS.Applications.Services
{
    public class RandomService : IRandomService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private const string RandomGeneratorUrl = "https://rpssl.olegbelousov.online/random";

		public RandomService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<int> GenerateRandomNumberAsync(CancellationToken cancellationToken)
		{
			using var httpClient = _httpClientFactory.CreateClient();
			var response = await httpClient.GetAsync(RandomGeneratorUrl, cancellationToken);

			if (response == null || !response.IsSuccessStatusCode)
			{
				return 0;
			}

			var content = await response.Content.ReadAsStringAsync();
			var randomResponse = JsonConvert.DeserializeObject<RandomResponse>(content);
			return randomResponse.Random;
		}
	}
}
