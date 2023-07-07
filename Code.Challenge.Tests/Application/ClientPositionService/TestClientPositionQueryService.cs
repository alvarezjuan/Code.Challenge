using Code.Challenge.Application.ClientPositionService;
using System.Net;
using System.Web.Http;

namespace Code.Challenge.Tests.Application.ClientPositionService
{
    [TestClass]
    public class TestClientPositionQueryService
    {
        [TestMethod]
        public async Task Test_KnownPersonId_ReturnOk()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new ClientPositionQueryRequest()
            {
                PersonId = 2148,
            };

            // Act
            var response = await sut.Send(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Position > 0);
        }

        [TestMethod]
        public async Task Test_UnknownPersonId_ThrowsHttpResponseException()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new ClientPositionQueryRequest()
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