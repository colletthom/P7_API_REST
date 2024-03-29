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
    public class RuleNameTests
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
        public async Task AddRuleNameUnitTest()
        {
            // Arrange
            var mockContext = new Mock<IDbContext>();
            var mockDbSet = new Mock<DbSet<RuleName>>();

            List<RuleName> addedRuleNames = new List<RuleName>();

            mockDbSet.Setup(m => m.Add(It.IsAny<RuleName>())).Callback<RuleName>((entity) =>
            {
                addedRuleNames.Add(entity);
            });

            mockContext.Setup(m => m.RuleNames).Returns(mockDbSet.Object);

            var repository = new RuleNameService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            var RuleNameTrue = new RuleName
            {
                Id = 1,
                Name = "Test RuleName",
                Description = "Test RuleName",
                Json = "Test RuleName",
                Template = "Test RuleName",
                SqlStr = "Test RuleName",
                SqlPart = "Test RuleName",
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var addedRuleNameTrue = await repository.AddRuleName(RuleNameTrue);

            // Assert
            Assert.IsNotNull(addedRuleNameTrue);
        }

        [TestMethod]
        public async Task UpdateRuleNameUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<RuleName>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(new RuleName
            {
                Id = 1,
                Name = "Test RuleName",
                Description = "Test RuleName",
                Json = "Test RuleName",
                Template = "Test RuleName",
                SqlStr = "Test RuleName",
                SqlPart = "Test RuleName",
            });

            mockContext.Setup(m => m.RuleNames).Returns(mockDbSet.Object);

            var repository = new RuleNameService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int RuleNameId = 1;

            var RuleNameUpdate = new RuleName
            {
                Id = 1,
                //Update RuleNameId
                Name = "Test RuleName UPDATE",
                Description = "Test RuleName",
                Json = "Test RuleName",
                Template = "Test RuleName",
                SqlStr = "Test RuleName",
                SqlPart = "Test RuleName",
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act           
            var result = await repository.UpdateRuleNameById(RuleNameId, RuleNameUpdate);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteRuleNameUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<RuleName>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(new RuleName
            {
                Id = 1,
                Name = "Test RuleName",
                Description = "Test RuleName",
                Json = "Test RuleName",
                Template = "Test RuleName",
                SqlStr = "Test RuleName",
                SqlPart = "Test RuleName",
            });

            mockContext.Setup(m => m.RuleNames).Returns(mockDbSet.Object);

            var repository = new RuleNameService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int RuleNameId = 1;

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var result = await repository.DeleteRuleNameById(RuleNameId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
