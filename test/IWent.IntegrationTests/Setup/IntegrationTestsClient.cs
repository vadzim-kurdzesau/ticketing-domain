using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using Newtonsoft.Json;

namespace IWent.IntegrationTests.Setup;

internal sealed class IntegrationTestsClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public IntegrationTestsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task AddItemToCartAsync(string cartId, OrderItem item)
    {
        var url = $"api/orders/carts/{cartId}";
        var content = SerializeToHttpContent(item);
        return MakeRequestAsync(client => client.PostAsync(url, content));
    }

    public async Task<IEnumerable<OrderItem>> GetItemsInCartAsync(string cartId)
    {
        var url = $"api/orders/carts/{cartId}";
        var response = await MakeRequestAsync(client => client.GetAsync(url));
        return await DeserializeResponseAsync<IEnumerable<OrderItem>>(response);
    }

    public async Task<PaymentInfo> BookItemsInCartAsync(string cartId)
    {
        var url = $"api/orders/carts/{cartId}/book";
        var response = await MakeRequestAsync(client => client.PutAsync(url, content: null));
        return await DeserializeResponseAsync<PaymentInfo>(response);
    }

    public async Task<PaymentInfo> GetPaymentInfoAsync(string paymentId)
    {
        var url = $"api/payments?paymentId={paymentId}";
        var response = await MakeRequestAsync(client => client.GetAsync(url));
        return await DeserializeResponseAsync<PaymentInfo>(response);
    }

    public Task CompleteOrderPaymentAsync(string paymentId)
    {
        var url = $"api/payments/{paymentId}/complete";
        return MakeRequestAsync(client => client.PostAsync(url, content: null));
    }

    public Task FailOrderPaymentAsync(string paymentId)
    {
        var url = $"api/payments/{paymentId}/failed";
        return MakeRequestAsync(client => client.PostAsync(url, content: null));
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private async Task<HttpResponseMessage> MakeRequestAsync(Func<HttpClient, Task<HttpResponseMessage>> requestAction)
    {
        var response = await requestAction(_httpClient);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiClientException($"The request was not successful: ({response.StatusCode}): {response.ReasonPhrase}.");
        }

        return response;
    }

    private static HttpContent SerializeToHttpContent<T>(T data)
    {
        var jsonContent = JsonConvert.SerializeObject(data);
        return new StringContent(jsonContent, Encoding.UTF8, mediaType: "application/json");
    }

    private static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json)
            ?? throw new ApiClientException($"Unable to deserialize the HTTP response into the '{typeof(T)}'.");
    }
}
