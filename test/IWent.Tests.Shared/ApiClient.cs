using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IWent.Api.Parameters;
using IWent.Services.DTO.Events;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using IWent.Services.DTO.Venues;
using Newtonsoft.Json;

namespace IWent.Tests.Shared;

public sealed class ApiClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(PaginationParameters? paginationParameters = null)
    {
        var url = paginationParameters == null
            ? $"api/events"
            : $"api/events?page={paginationParameters.Page}&size={paginationParameters.Size}";

        var response = await MakeRequestAsync(client => client.GetAsync(url));
        return await DeserializeResponseAsync<IEnumerable<Event>>(response);
    }

    public async Task<IEnumerable<SectionSeat>> GetSectionSeatsAsync(int eventId, int sectionId)
    {
        var url = $"api/events/{eventId}/sections/{sectionId}/seats";
        var response = await MakeRequestAsync(client => client.GetAsync(url));
        return await DeserializeResponseAsync<IEnumerable<SectionSeat>>(response);
    }

    public async Task<IEnumerable<VenueSection>> GetSectionsAsync(int venueId)
    {
        var url = $"api/venues/{venueId}/sections";
        var response = await MakeRequestAsync(client => client.GetAsync(url));
        return await DeserializeResponseAsync<IEnumerable<VenueSection>>(response);
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
            throw new ApiClientException($"The request was not successful: ({(int)response.StatusCode}): {response.ReasonPhrase}.");
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
