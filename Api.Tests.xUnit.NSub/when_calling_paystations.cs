using Xunit;
using System.Threading.Tasks;
using API.Repositories;
using API.Models;
using API.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentAssertions;

namespace Api.Tests.xUnit.NSub
{
    public class when_calling_paystations
    {

        [Fact]
        public async Task returns_list_of_paystations()
        {
            // Arrange
            var mockRepo = Substitute.For<IPayStationRepo>();
            mockRepo.Retrieve().Returns(Task.FromResult(GetPayStations()));

            var controller = new PayStationController(mockRepo);

            // Act
            var actionResult = await controller.GetAllPaymentStations();
            Assert.NotNull(actionResult);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            List<PaymentStationRecord> messages = okResult.Value as List<PaymentStationRecord>;
            Assert.Equal(2, messages.Count);
            Assert.Equal("Fred Meyer", messages[0].PayStationName);
            Assert.Equal("Safeway #1", messages[1].PayStationName);


            //fluent assertions
            messages.Count.Should().Be(2);
            var okRes = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var station = okResult.Value.Should().BeAssignableTo<List<PaymentStationRecord>>().Subject;
        }

        [Fact]
        public async Task returns_no_content_result()
        {
            // Arrange
            var mockRepo = Substitute.For<IPayStationRepo>();
            mockRepo.Retrieve().Returns(Task.FromResult(EmptyPayStations()));

            var controller = new PayStationController(mockRepo);

            // Act
            var actionResult = await controller.GetAllPaymentStations();

            //fluent assertion
            var okRes = actionResult.Should().BeOfType<NoContentResult>().Subject;

        }



        private List<PaymentStationRecord> GetPayStations()
        {

            var result = new List<PaymentStationRecord> {
                new PaymentStationRecord { PayStationName = "Fred Meyer"},
                new PaymentStationRecord { PayStationName = "Safeway #1" }
            };

            return result;
        }
        private List<PaymentStationRecord> EmptyPayStations()
        {
            return new List<PaymentStationRecord>();
        }

    }
}
