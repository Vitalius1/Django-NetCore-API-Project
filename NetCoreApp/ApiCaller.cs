using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
 
namespace Auction
{
    public class WebRequest
    {
        public static async Task GetProductDataAsync(int id, Action<List<Dictionary<string, object>>> Callback)
        {
            using (var Client = new HttpClient())
            {
                try
                {
                    Client.BaseAddress = new Uri($"http://localhost:8000/allproducts/{id}");
                    HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
                    Response.EnsureSuccessStatusCode(); // Throw error if not successful.
                    string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                     
                    List<Dictionary<string, object>> JsonResponse = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(StringResponse);
                     
                    Callback(JsonResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
        }
// ============================================================================================================

        public static async Task SingleProductAsync(int id, Action<Dictionary<string, object>> Callback)
        {
            using (var Client = new HttpClient())
            {
                try
                {
                    Client.BaseAddress = new Uri($"http://localhost:8000/productdetails/{id}");
                    HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
                    Response.EnsureSuccessStatusCode(); // Throw error if not successful.
                    string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                     
                    Dictionary<string, object> JsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(StringResponse);
                     
                    Callback(JsonResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
        }
    }
}