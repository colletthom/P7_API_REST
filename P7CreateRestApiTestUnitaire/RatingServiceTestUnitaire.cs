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
    public class RatingTests
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
        public async Task AddRatingUnitTest()
        {
            // Arrange
            var mockContext = new Mock<IDbContext>();
            var mockDbSet = new Mock<DbSet<Rating>>();

            List<Rating> addedRatings = new List<Rating>();

            mockDbSet.Setup(m => m.Add(It.IsAny<Rating>())).Callback<Rating>((entity) =>
            {
                addedRatings.Add(entity);
            });

            mockContext.Setup(m => m.Ratings).Returns(mockDbSet.Object);

            var repository = new RatingService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            var RatingTrue = new Rating
            {
                Id = 1,
                MoodysRating = "A++++",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var addedRatingTrue = await repository.AddRating(RatingTrue);

            // Assert
            Assert.IsNotNull(addedRatingTrue);
        }

        [TestMethod]
        public async Task UpdateRatingUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Rating>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(new Rating
            {
                Id = 1,
                MoodysRating = "A++++",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            });

            mockContext.Setup(m => m.Ratings).Returns(mockDbSet.Object);

            var repository = new RatingService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int RatingId = 1;

            var RatingUpdate = new Rating
            {
                Id = 1,
                //Update RatingId
                MoodysRating = "A----",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act           
            var result = await repository.UpdateRatingById(RatingId, RatingUpdate);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteRatingUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Rating>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(new Rating
            {
                Id = 1,
                MoodysRating = "A++++",
                SandPRating = "A++++",
                FitchRating = "A++++",
                OrderNumber = 0,
            });

            mockContext.Setup(m => m.Ratings).Returns(mockDbSet.Object);

            var repository = new RatingService(mockContext.Object);
            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int RatingId = 1;

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var result = await repository.DeleteRatingById(RatingId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
