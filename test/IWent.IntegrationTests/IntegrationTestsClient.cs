using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using Newtonsoft.Json;

namespace IWent.IntegrationTests;

internal sealed class IntegrationTestsClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public IntegrationTestsClient(HttpClient httpClient)
	{
        _httpClient = httpClient;
    }

    public async Task AddItemToCartAsync(string cartId, OrderItem item)
    {
        var url = $"api/orders/carts/{cartId}";
        var content = JsonContent.Create(item);
        var response = await _httpClient.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not working");
        }
    }

    public async Task<IEnumerable<OrderItem>> GetItemsInCartAsync(string cartId)
    {
        var url = $"api/orders/carts/{cartId}";
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrderItem>>(url);
        return response;
        //if (!response.IsSuccessStatusCode)
        //{
        //    throw new Exception("Not working");
        //}
    }

    public async Task<PaymentInfo> BookItemsInCartAsync(string cartId)
    {
        var url = $"api/orders/carts/{cartId}/book";
        var response = await _httpClient.PutAsync(url, content: null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"{response.StatusCode}: {response.ReasonPhrase}");
        }

        return await DeserializeResponseAsync<PaymentInfo>(response);
    }

    public async Task<PaymentInfo> GetPaymentInfoAsync(string paymentId)
    {
        var url = $"api/payments?paymentId={paymentId}";
        var response = await _httpClient.GetAsync(url);
        return await DeserializeResponseAsync<PaymentInfo>(response);
    }

    public async Task CompleteOrderPaymentAsync(string paymentId)
    {
        var url = $"api/payments/{paymentId}/complete";
        await _httpClient.PostAsync(url, content: null);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync() ?? throw new Exception();
        return JsonConvert.DeserializeObject<T>(json);
    }
}
