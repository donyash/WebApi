using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using API.Repositories;
using API.Models;
using API.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace Api.Tests.MsTest.Moq
{
    [TestClass]
    public class when_calling_paystations
    {
        [TestMethod]
        public async Task returns_list_of_paystations()
        {
            // Arrange
            var mockRepo = new Mock<IPayStationRepo>();
            mockRepo.Setup(repo => repo.Retrieve())
               .Returns(Task.FromResult(GetPayStations()));

            var controller = new PayStationController(mockRepo.Object);

            // Act
            var actionResult = await controller.GetAllPaymentStations();
            Assert.IsNotNull(actionResult);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.StatusCode.Equals(200));

            List<PaymentStationRecord> messages = okResult.Value as List<PaymentStationRecord>;
            Assert.IsTrue(messages.Count == 2);

            Assert.IsTrue(messages[0].PayStationName.Equals("Fred Meyer"));
            Assert.IsTrue(messages[1].PayStationName.Equals("Safeway #1"));

            //fluent assertions
            messages.Count.Should().Be(2);
            var okRes = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var station = okResult.Value.Should().BeAssignableTo<List<PaymentStationRecord>>().Subject;

            mockRepo.Verify();
        }

        [TestMethod]
        public async Task returns_no_content_result()
        {
            // Arrange
            var mockRepo = new Mock<IPayStationRepo>();
            mockRepo.Setup(repo => repo.Retrieve())
               .Returns(Task.FromResult(EmptyPayStations()));

            var controller = new PayStationController(mockRepo.Object);

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
