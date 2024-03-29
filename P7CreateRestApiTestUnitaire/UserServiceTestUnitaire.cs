using System;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Azure;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using P7CreateRestApi.Models;
using System.Net;


namespace Dot.Net.WebApi.Tests.Repositories
{
    [TestClass]
    public class UserTests
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
        public async Task AddUserUnitTest()
        {
            // Arrange
            var mockContext = new Mock<IDbContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            List<User> addedUsers = new List<User>();

            var User = new User
            {
                Id = 1,
                UserName = "TestIntegrationUser",
                FullName = "Test User"
            };

            var UserTrueModel = new RegisterModel
            {
                UserName = "TestIntegrationUser",
                Password = "P@ssword123",
                FullName = "Test User",
            };

            mockDbSet.Setup(m => m.Add(It.IsAny<User>())).Callback<User>((entity) =>
            {
                addedUsers.Add(entity);
            });

            mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);
            var mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var repository = new UserService(mockContext.Object, mockUserManager.Object, mockPasswordHasher.Object);

            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), UserTrueModel.Password)).ReturnsAsync(IdentityResult.Success);

            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var addedUserTrue = await repository.AddUser(UserTrueModel);

            // Assert
            Assert.IsFalse(addedUserTrue.GetType().GetProperty("Errors") != null, "Des erreurs ne doivent pas être retournées en cas d'échec de la mise à jour");
        }

        [TestMethod]
        public async Task UpdateUserUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns(new User
            {
                Id = 1,
                UserName = "TestIntegrationUser",
                PasswordHash = "P@ssword123",
                FullName = "Test User",
            });

            mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);
            var mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var repository = new UserService(mockContext.Object, mockUserManager.Object, mockPasswordHasher.Object);

            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int UserId = 1;

            var UserUpdate = new UpdateModel
            {
                //Update UserId
                UserName = "TestIntegrationUser",
                Role = "Admin"
            };

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act           
            var result = await repository.UpdateUserById(UserId, UserUpdate);

            // Assert
            Assert.IsFalse(result.GetType().GetProperty("Errors") != null, "Des erreurs ne doivent pas être retournées en cas d'échec de la mise à jour");
        }

        [TestMethod]
        public async Task DeleteUserUnitTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<IDbContext>();

            // Configure la méthode Find du DbSet simulé pour rechercher l'élément dans la liste simulée
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(new User
            {
                Id = 1,
                UserName = "TestIntegrationUser",
                PasswordHash = "P@ssword123",
                FullName = "Test User",
            });

            mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);
            var mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var repository = new UserService(mockContext.Object, mockUserManager.Object, mockPasswordHasher.Object);

            var _clientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            int UserId = 1;

            var client = _clientFactory.CreateClient(); // Création d'une instance d'objet HttpClient
            client.BaseAddress = new Uri("https://localhost:7210");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //récupération du Token
            var token = await GetValidToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var result = await repository.DeleteUserById(UserId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}