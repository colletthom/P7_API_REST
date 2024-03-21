// using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System;
using static System.Net.WebRequestMethods;

namespace HttpClientSample
{
    public class Rating
    {
        public int Id { get; set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }
        public byte? OrderNumber { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Rating rating)
        {
            Console.WriteLine($"MoodysRating: {rating.MoodysRating}\tSandPRating: " +
                $"{rating.SandPRating}\tFitchRating: {rating.FitchRating}\tOrderNumber: {rating.OrderNumber}");
        }

        static async Task<Uri> CreateRatingAsync(Rating rating)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Rating", rating);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            int newResourceId = 16;
            return new Uri($"https://localhost:7210/api/Rating/{newResourceId}");

        }

        static async Task<Rating> GetRatingAsync(string path)
        {
            Rating rating = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                rating = await response.Content.ReadFromJsonAsync<Rating>();
            }
            return rating;
        }

        static async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/Rating/{rating.Id}", rating);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            rating = await response.Content.ReadFromJsonAsync<Rating>();
            return rating;
        }

        static async Task<HttpStatusCode> DeleteRatingAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/Rating/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:7210/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Rating rating = new Rating
                {
                    MoodysRating = "a+",
                    SandPRating = "a-",
                    FitchRating = "AA+",
                    OrderNumber = 10
                };

                var url = await CreateRatingAsync(rating);
                Console.WriteLine($"Created at {url}");
                /*
                // Get the product
                rating = await GetRatingAsync(url.PathAndQuery);
                ShowProduct(rating);

                // Update the product
                Console.WriteLine("Updating ...");
                rating.MoodysRating = "B";
                await UpdateRatingAsync(rating);

                // Get the updated product
                rating = await GetRatingAsync(url.PathAndQuery);
                ShowProduct(rating);

                // Delete the product
                var statusCode = await DeleteRatingAsync(rating.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");*/

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
