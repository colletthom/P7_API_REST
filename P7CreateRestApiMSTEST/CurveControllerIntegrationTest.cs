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
    public class CurveControllerIntegrationTests
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
        [Description("Test to create with good Curve and Bad Curve, Update and Delete")]
        public async Task AddPutDeleteCurveControllerTest()
        {
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var _context = _serviceProvider.GetRequiredService<LocalDbContext>();

            // Arrange
            var newCurve = new CurvePoint
            {
                // Remplissez les propriétés du nouvel objet Bid selon les besoins de votre test
                CurveId = 0,
                AsOfDate = DateTime.UtcNow,
                Term = 0,
                CurvePointValue = 0,
                CreationDate = DateTime.UtcNow,
            };

            var newCurveUpdate = new CurvePoint
            {
                // Remplissez les propriétés du nouvel objet Bid selon les besoins de votre test
                CurveId = 0,
                AsOfDate = DateTime.UtcNow,
                Term = 1,
                CurvePointValue = 0,
                CreationDate = DateTime.UtcNow,
            };

            var newCurveFalse = new CurvePoint
            {
                // Remplissez les propriétés du nouvel objet Bid selon les besoins de votre test
                CurveId = 0,
                AsOfDate = DateTime.UtcNow,
                //Terme doit être positif
                Term = -10,
                CurvePointValue = 0,
                CreationDate = DateTime.UtcNow,
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken(); // Récupérer le jeton d'authentification
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.PostAsJsonAsync("/api/Curve", newCurve);
            var responseFalse = await client.PostAsJsonAsync("/api/Curve", newCurveFalse);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(HttpStatusCode.OK, responseFalse.StatusCode);

            //Update
            // Récupérer l'ID de la ressource à supprimer
            var id = _context.CurvePoints
                .Where(b => b.CurveId == 0)
                .OrderBy(b => b.Id)
                .Select(b => b.Id)
                .LastOrDefault();

            if (id != null)
            {
                string updateUri = $"/api/Curve/{id}";
                var responseUpdate = await client.PutAsJsonAsync(updateUri, newCurveUpdate);

                responseUpdate.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }

            //Delete

            if (id != null)
            {
                string deleteUri = $"/api/Curve/{id}";
                var responseDelete = await client.DeleteAsync(deleteUri);

                responseDelete.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [TestMethod]
        [Description("Test to GetAll Curve")]
        public async Task GetAllCurveControllerTest()
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
            var response = await client.GetAsync("/api/Curve");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [Description("Test to Get Curve By Id")]

        public async Task GetCurveByIdControllerTest()
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

            var id = _context.CurvePoints
                .OrderBy(b => b.Id)
                .Select(b => b.Id)
                .LastOrDefault();

            // Act
            var response = await client.GetAsync($"/api/Curve/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
