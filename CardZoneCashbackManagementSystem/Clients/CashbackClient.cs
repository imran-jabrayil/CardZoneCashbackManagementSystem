using CardZoneCashbackManagementSystem.Clients.Settings;
using CardZoneCashbackManagementSystem.Models.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CardZoneCashbackManagementSystem.Clients;

public class CashbackClient
{
    private readonly HttpClient _httpClient;
    private readonly CashbackClientSettings _settings;


    public CashbackClient(
        HttpClient httpClient,
        IOptions<CashbackClientSettings> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }


    public async Task<decimal?> GetCashbackAmountAsync(double transactionAmount)
    {
        var requestUri = $"{_settings.ApiUrl}?transactionAmount={transactionAmount}";

        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var cashbackResponse = JsonConvert.DeserializeObject<GetCashbackAmountResponse>(jsonResponse);
        return cashbackResponse?.CashbackAmount;
    }
}