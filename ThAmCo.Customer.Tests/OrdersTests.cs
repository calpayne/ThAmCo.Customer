using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Orders;
using ThAmCo.Customer.Web.Controllers;

namespace ThAmCo.Customer.Tests
{
    [TestClass]
    public class OrdersTests
    {
        [TestMethod]
        public async Task GetAllOrders_LoggedIn_UserHasOrders_ShouldBeValid()
        {
            // Arrange
            IEnumerable<OrderGetDto> fakeOrders = new List<OrderGetDto>
            {
                new OrderGetDto { Id = 1, TimePlaced = new DateTime(2019, 12, 7), Price = 10.25, TimeDispatched = new DateTime(2019, 12, 8), Product = new ProductDto { Id = 1, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 }, CustomerId = "f32d935b-f175-4450-a93e-e48711c4d481" },
                new OrderGetDto { Id = 2, TimePlaced = new DateTime(2019, 12, 9), Price = 50.25, TimeDispatched = null, Product = new ProductDto { Id = 2, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", Name = "Chocolate Cover", Price = 50.25, StockLevel = 12 }, CustomerId = "f32d935b-f175-4450-a93e-e48711c4d481" }
            };

            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "f32d935b-f175-4450-a93e-e48711c4d481"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new OrdersController(new FakeOrdersService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as IEnumerable<OrderGetDto>;
            Assert.IsNotNull(model);
            var orders = model;
            Assert.IsNotNull(orders);
            Assert.AreEqual(fakeOrders.Count(), orders.Count());
            for (int i = 0; i < orders.Count(); i++)
            {
                Assert.AreEqual(fakeOrders.ElementAt(i).Id, orders.ElementAt(i).Id);
                Assert.AreEqual(fakeOrders.ElementAt(i).Price, orders.ElementAt(i).Price);
                Assert.AreEqual(fakeOrders.ElementAt(i).TimePlaced, orders.ElementAt(i).TimePlaced);
                Assert.AreEqual(fakeOrders.ElementAt(i).TimeDispatched, orders.ElementAt(i).TimeDispatched);
                Assert.AreEqual(fakeOrders.ElementAt(i).CustomerId, orders.ElementAt(i).CustomerId);
            }
        }

        [TestMethod]
        public async Task GetAllOrders_LoggedIn_UserHasNoOrders_ShouldBeValid()
        {
            // Arrange
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "user-has-no-orders"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new OrdersController(new FakeOrdersService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as IEnumerable<OrderGetDto>;
            Assert.IsNotNull(model);
            var orders = model;
            Assert.IsNotNull(orders);
            Assert.AreEqual(orders.Count(), 0);
        }

        [TestMethod]
        public async Task GetAllOrders_NotLoggedIn_ShouldBadRequest()
        {
            // Arrange
            var controller = new OrdersController(new FakeOrdersService());

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }
    }
}
