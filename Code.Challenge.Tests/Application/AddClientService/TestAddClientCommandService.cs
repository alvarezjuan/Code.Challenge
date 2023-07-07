using Code.Challenge.Application.AddClientService;
using System.Net;
using System.Web.Http;

namespace Code.Challenge.Tests.Application.AddClientService
{
    [TestClass]

    public class TestAddClientCommandService
    {
        [TestMethod]
        public async Task Test_FirstInsert_ReturnOk()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new AddClientCommandRequest()
            {
                PersonId = 111111,
                FirstName = "111111",
                LastName = "111111",
                CurrentRole = "111111",
                Country = "111111",
                Industry = "111111",
                NumberOfRecommendations = 111111,
                NumberOfConnections = 111111,
            };

            // Act
            var response = await sut.Send(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Priority);
        }

        [TestMethod]
        public async Task Test_SecondInsertDuplicate_ThrowsHttpResponseException()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new AddClientCommandRequest()
            {
                PersonId = 111111,
                FirstName = "111111",
                LastName = "111111",
                CurrentRole = "111111",
                Country = "111111",
                Industry = "111111",
                NumberOfRecommendations = 111111,
                NumberOfConnections = 111111,
            };

            // Act
            var exception = await Assert.ThrowsExceptionAsync<HttpResponseException>(() =>
                sut.Send(request, CancellationToken.None));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public async Task Test_InsertMissingData_ThrowsHttpResponseException()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new AddClientCommandRequest()
            {
                PersonId = 999999,
            };

            // Act
            var exception = await Assert.ThrowsExceptionAsync<HttpResponseException>(() =>
                sut.Send(request, CancellationToken.None));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }
    }
}