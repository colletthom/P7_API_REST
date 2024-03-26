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
using NuGet.Common;

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

            // J'ajoute les services n�cessaires au conteneur, y compris IHttpClientFactory si n�cessaire
            services.AddHttpClient();
            // J'ajoute d'autres services n�cessaires au conteneur

            // J'ajoute la configuration � partir de appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

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

            // J'utilise HttpClient pour envoyer une requ�te POST � mon API pour obtenir un jeton
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
        [Description("...")]
        //public async Task BidControllerTest()
        public async Task BidControllerTest()
        {
            //Assert.Fail("test");
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            // Arrange
            var newBid = new Bid
            {
                // Remplissez les propri�t�s du nouvel objet Bid selon les besoins de votre test
                Account = "Test Account",
                BidType = "Test BidType",
                BidQuantity = 0,
                AskQuantity = 0,
                Bid2 = 0,
                Ask = 0,
                Benchmark = "Test int�gration",
                BidListDate = DateTime.UtcNow,
                Commentary = "Test int�gration",
                BidSecurity = "Test int�gration",
                BidStatus = "Test int�gration",
                Trader = "Test int�gration",
                Book = "Test int�gration",
                CreationName = "Test int�gration",
                CreationDate = DateTime.UtcNow,
                RevisionName = "Test int�gration",
                RevisionDate = DateTime.UtcNow,
                DealName = "Test int�gration",
                DealType = "Test int�gration",
                SourceListId = "Test int�gration",
                Side = "Test int�gration"
            };



            /*// R�cup�rer la cha�ne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Utiliser la cha�ne de connexion pour cr�er une instance du contexte de base de donn�es
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlServer(connectionString)
                .Options;*/

            var client = _clientFactory.CreateClient(); // Cr�ation d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //r�cup�ration du Token
            var token = await GetValidToken(); // R�cup�rer le jeton d'authentification
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.PostAsJsonAsync("/api/Bid", newBid);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}