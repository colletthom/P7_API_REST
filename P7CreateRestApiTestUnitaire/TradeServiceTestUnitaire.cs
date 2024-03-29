using System;
using System.Data;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using P7CreateRestApi.Models;


namespace Dot.Net.WebApi.Tests.Repositories
{
    [TestClass]
    public class TradeTests
    {
        private ServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // J'ajoute les services nécessaires au conteneur, y compris IHttpClientFactory si nécessaire
            services.AddHttpClient();

            //J'ajoute d'autres services nécessaires au conteneur
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
        public async Task AddTradeUnitTest()
        {
            // Arrange
            var mockContext = new Mock<IDbContext>();
            var mockDbSet = new Mock<DbSet<Trade>>();

            List<Trade> addedTrades = new List<Trade>();

            mockDbSet.Setup(m => m.Add(It.IsAny<Trade>())).Callback<Trade>((entity) =>
            {
                addedTrades.Add(entity);
            });

            mockContext.Setup(m => m.Trades).Returns(mockDbSet.Object);

            var repository = new TradeService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            var TradeTrue = new Trade
            {
                TradeId = 1,
                Account = "Test Trade",
                AccountType = "Test TradeType",
                BuyQuantity = 0,
                SellQuantity = 0,
                BuyPrice = 0,
                SellPrice = 0,
                TradeDate = DateTime.UtcNow,
                TradeSecurity = "Test intégration",
                TradeStatus = "Test intégration",
                Trader = "Test intégration",
                Benchmark = "Test intégration",
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

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var addedTradeTrue = await repository.AddTrade(TradeTrue);

            // Assert
            Assert.IsNotNull(addedTradeTrue);
        }

        [TestMethod]
        public async Task UpdateTradeUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Trade>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(new Trade
            {
                TradeId = 1,
                Account = "Test Trade",
                AccountType = "Test TradeType",
                BuyQuantity = 0,
                SellQuantity = 0,
                BuyPrice = 0,
                SellPrice = 0,
                TradeDate = DateTime.UtcNow,
                TradeSecurity = "Test intégration",
                TradeStatus = "Test intégration",
                Trader = "Test intégration",
                Benchmark = "Test intégration",
                Book = "Test intégration",
                CreationName = "Test intégration",
                CreationDate = DateTime.UtcNow,
                RevisionName = "Test intégration",
                RevisionDate = DateTime.UtcNow,
                DealName = "Test intégration",
                DealType = "Test intégration",
                SourceListId = "Test intégration",
                Side = "Test intégration"
            });

            mockContext.Setup(m => m.Trades).Returns(mockDbSet.Object);

            var repository = new TradeService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int TradeId = 1;

            var TradeUpdate = new Trade
            {
                TradeId = 1,
                //Update TradeId
                Account = "Test Trade UPDATE",
                AccountType = "Test TradeType",
                BuyQuantity = 0,
                SellQuantity = 0,
                BuyPrice = 0,
                SellPrice = 0,
                TradeDate = DateTime.UtcNow,
                TradeSecurity = "Test intégration",
                TradeStatus = "Test intégration",
                Trader = "Test intégration",
                Benchmark = "Test intégration",
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

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act           
            var result = await repository.UpdateTradeById(TradeId, TradeUpdate);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteTradeUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Trade>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(new Trade
            {
                TradeId = 1,
                Account = "Test Trade",
                AccountType = "Test TradeType",
                BuyQuantity = 0,
                SellQuantity = 0,
                BuyPrice = 0,
                SellPrice = 0,
                TradeDate = DateTime.UtcNow,
                TradeSecurity = "Test intégration",
                TradeStatus = "Test intégration",
                Trader = "Test intégration",
                Benchmark = "Test intégration",
                Book = "Test intégration",
                CreationName = "Test intégration",
                CreationDate = DateTime.UtcNow,
                RevisionName = "Test intégration",
                RevisionDate = DateTime.UtcNow,
                DealName = "Test intégration",
                DealType = "Test intégration",
                SourceListId = "Test intégration",
                Side = "Test intégration"
            });

            mockContext.Setup(m => m.Trades).Returns(mockDbSet.Object);

            var repository = new TradeService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int TradeId = 1;

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var result = await repository.DeleteTradeById(TradeId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}