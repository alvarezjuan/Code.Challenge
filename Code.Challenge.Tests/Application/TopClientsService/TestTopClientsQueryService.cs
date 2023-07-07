using Code.Challenge.Application.ClientPositionService;
using Code.Challenge.Application.TopClientsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Code.Challenge.Tests.Application.TopClientsService
{
    [TestClass]
    public class TestTopClientsQueryService
    {
        [TestMethod]
        public async Task Test_ValidNumberOfResults_ReturnOk()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new TopClientsQueryRequest()
            {
                NumberOfResults = 5,
            };

            // Act
            var response = await sut.Send(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(5, response.Count());
        }

        [TestMethod]
        public async Task Test_InValidNumberOfResults_ThrowsHttpResponseException()
        {
            // Arrange
            var sut = TestHelpers.BuildSut();

            var request = new TopClientsQueryRequest()
            {
                NumberOfResults = 0,
            };

            // Act
            var exception = await Assert.ThrowsExceptionAsync<HttpResponseException>(() =>
                sut.Send(request, CancellationToken.None));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }
    }
}