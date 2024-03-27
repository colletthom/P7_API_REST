using System.Net.Http.Json;
using System.Net;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Models;
using System.Net.Http.Headers;
using LoginModel = P7CreateRestApi.Models.LoginModel;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Common;
using Microsoft.EntityFrameworkCore;
using System.Configuration;


namespace P7CreateRestApiMSTEST
{
    [TestClass]
    [Authorize(Policy = "AccessWriteActions")]
    public class RatingControllerIntegrationTests
    {
        private ServiceProvider _serviceProvider;

        [TestInitialize]

        public void Setup()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<LocalDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), providerOptions => providerOptions.EnableRetryOnFailure()));

            // J'ajoute les services nécessaires au conteneur, y compris IHttpClientFactory si nécessaire
            services.AddHttpClient();
            // J'ajoute d'autres services nécessaires au conteneur

            _serviceProvider = services.BuildServiceProvider();
        }

        private async Task<string> GetValidToken()
        {
            var _client = _serviceProvider.GetRequiredService<HttpClient>();

            LoginModel loginModel = new LoginModel
            {
                UserName = "test5",
                Password = "P@ssword123"
            };

            // J'utilise HttpClient pour envoyer une requête POST à mon API pour obtenir un jeton
            _client.BaseAddress = new Uri("https://localhost:7210");
            var response = await _client.PostAsJsonAsync("/token/GetToken", loginModel);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenObject = JObject.Parse(responseContent);
                var token = tokenObject["token"]?.ToString();
                return token;
            }
            else
            {
                return null;
            }
        }

        [TestMethod]
        [Description("Test to create with good Rating and Bad Rating, Update and Delete")]
        public async Task AddPutDeleteRatingControllerTest()
        {
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var _context = _serviceProvider.GetRequiredService<LocalDbContext>();

            // Arrange
            var newRating = new Rating
            {
                // Remplissez les propriétés du nouvel objet Rating selon les besoins de votre test
                MoodysRating = "A++++",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            };

            var newRatingUpdate = new Rating
            {
                // Remplissez les propriétés du nouvel objet Rating selon les besoins de votre test
                MoodysRating = "A----",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            };

            var newRatingFalse = new Rating
            {
                // Remplissez les propriétés du nouvel objet Rating selon les besoins de votre test
                //MoodysRating = "A++++",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken(); // Récupérer le jeton d'authentification
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.PostAsJsonAsync("/api/Rating", newRating);
            var responseFalse = await client.PostAsJsonAsync("/api/Rating", newRatingFalse);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(HttpStatusCode.OK, responseFalse.StatusCode);

            //Update
            // Récupérer l'ID de la ressource à supprimer
            var id = _context.Ratings
                .Where(b => b.SandPRating == "A++++")
                .OrderBy(b => b.Id)
                .Select(b => b.Id)
                .LastOrDefault();

            if (id != null)
            {
                string updateUri = $"/api/Rating/{id}";
                var responseUpdate = await client.PutAsJsonAsync(updateUri, newRatingUpdate);

                responseUpdate.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }

            //Delete

            if (id != null)
            {
                string deleteUri = $"/api/Rating/{id}";
                var responseDelete = await client.DeleteAsync(deleteUri);

                responseDelete.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [TestMethod]
        [Description("Test to GetAll Rating")]
        public async Task GetAllRatingControllerTest()
        {
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken(); // Récupérer le jeton d'authentification
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/api/Rating");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [Description("Test to Get Rating By Id")]

        public async Task GetRatingByIdControllerTest()
        {
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var _context = _serviceProvider.GetRequiredService<LocalDbContext>();

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken(); // Récupérer le jeton d'authentification
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var id = _context.Ratings
                .OrderBy(b => b.Id)
                .Select(b => b.Id)
                .LastOrDefault();

            // Act
            var response = await client.GetAsync($"/api/Rating/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}