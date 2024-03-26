using Microsoft.AspNetCore.Mvc.Testing;
using static System.Net.WebRequestMethods;
using System.Text;
using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Azure.Identity;
using LoginModel = P7CreateRestApi.Models.LoginModel;
using Microsoft.AspNetCore.Mvc;
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

namespace P7CreateRestApiMSTEST
{
    [TestClass]
    [Authorize(Policy = "AccessWriteActions")]
    public class BidControllerIntegrationTests
    {
        //private readonly IConfiguration _configuration;
        //private readonly IHttpClientFactory _clientFactory;
        private ServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Ajoutez les services nécessaires au conteneur, y compris IHttpClientFactory si nécessaire
            services.AddHttpClient();
            // Ajoutez d'autres services nécessaires au conteneur

            // Ajoutez la configuration à partir de appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            _serviceProvider = services.BuildServiceProvider();
        }

         [TestMethod]
        [Description("...")]
        //public async Task BidControllerTest()
        public void BidControllerTest()
        {
            Assert.Fail("test");
            /*
            // Arrange
            var newBid = new Bid
            {
                // Remplissez les propriétés du nouvel objet Bid selon les besoins de votre test
                Account = "Test Account",
                BidType = "Test BidType",
                BidQuantity = 0,
                AskQuantity = 0,
                Bid2 = 0,
                Ask = 0,
                Benchmark = "Test intégration",
                BidListDate = DateTime.UtcNow,
                Commentary = "Test intégration",
                BidSecurity = "Test intégration",
                BidStatus = "Test intégration",
                Trader = "Test intégration",
                Book = "Test intégration",
                CreationName = "Test intégration",
                CreationDate = DateTime.UtcNow,
                RevisionName = "Test intégration",
                RevisionDate = DateTime.UtcNow,
                DealName = "Test intégration",
                DealType = "Test intégration",
                SourceListId = "Test intégration",
                Side = "Test intégration"
            };
            */
            /*// Récupérer la chaîne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Utiliser la chaîne de connexion pour créer une instance du contexte de base de données
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlServer(connectionString)
                .Options;*/
            /*
            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.PostAsJsonAsync("/api/Bid", newBid);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);*/
        }
    }
}