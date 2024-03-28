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
    public class BidRepositoryTests
    {
        private ServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // J'ajoute les services n�cessaires au conteneur, y compris IHttpClientFactory si n�cessaire
            services.AddHttpClient();

            //J'ajoute d'autres services n�cessaires au conteneur
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
        public async Task AddBidUnitTest()
        {
            // Arrange
            var mockContext = new Mock<IDbContext>();
            var mockDbSet = new Mock<DbSet<Bid>>();

            List<Bid> addedBids = new List<Bid>(); 

            mockDbSet.Setup(m => m.Add(It.IsAny<Bid>())).Callback<Bid>((entity) =>
            {
                addedBids.Add(entity);
            });

            mockContext.Setup(m => m.Bids).Returns(mockDbSet.Object);

            var repository = new BidRepository(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            var bidTrue = new Bid
            {
                BidId = 1,
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

            var client = _clientFactory.CreateClient(); // Cr�ation d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //r�cup�ration du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var addedBidTrue = await repository.AddBid(bidTrue);

            // Assert
            Assert.IsNotNull(addedBidTrue);
        }

        [TestMethod]
        public async Task UpdateBidUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Bid>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la m�thode Find du DbSet simul� pour rechercher l'�l�ment dans la liste simul�e
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(new Bid 
            {
                BidId = 1,
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
            });

            mockContext.Setup(m => m.Bids).Returns(mockDbSet.Object);

            var repository = new BidRepository(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int bidId = 1;

            var bidUpdate = new Bid
            {
                BidId = 1,
                Account = "Test Account UPDATE",
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

            var client = _clientFactory.CreateClient(); // Cr�ation d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //r�cup�ration du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act           
            var result = await repository.UpdateBid(bidId, bidUpdate);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteBidUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Bid>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la m�thode Find du DbSet simul� pour rechercher l'�l�ment dans la liste simul�e
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(new Bid
            {
                BidId = 1,
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
            });

            mockContext.Setup(m => m.Bids).Returns(mockDbSet.Object);

            var repository = new BidRepository(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int bidId = 1;    

            var client = _clientFactory.CreateClient(); // Cr�ation d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //r�cup�ration du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var result = await repository.DeleteBid(bidId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}