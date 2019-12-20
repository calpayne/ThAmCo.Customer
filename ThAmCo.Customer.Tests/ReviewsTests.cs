using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Orders;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Reviews;
using ThAmCo.Customer.Web.Controllers;

namespace ThAmCo.Customer.Tests
{
    [TestClass]
    public class ReviewsTests
    {
        [TestMethod]
        public async Task CreatingAReview_ValidProduct_LoggedIn_ShouldRedirectToAction()
        {
            // Arrange
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "f32d935b-f175-4450-a93e-e48711c4d481"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new ReviewsController(new FakeProductsService(), new FakeOrdersService(), new FakeReviewsService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Create(new ReviewDto 
            { 
                ProductId = 1,
                Rating = 5,
                Description = "This is a good review",
                Title = "I like this product"
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Index");
            Assert.AreEqual(objResult.ControllerName, "Orders");
        }

        [TestMethod]
        public async Task CreatingAReview_InvalidProduct_LoggedIn_ShouldBadRequest()
        {
            // Arrange
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "f32d935b-f175-4450-a93e-e48711c4d481"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new ReviewsController(new FakeProductsService(), new FakeOrdersService(), new FakeReviewsService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Create(new ReviewDto
            {
                ProductId = 99999,
                Rating = 5,
                Description = "This is a good review",
                Title = "I like this product"
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task CreatingAReview_ValidProduct_NotLoggedIn_ShouldBadRequest()
        {
            // Arrange
            var controller = new ReviewsController(new FakeProductsService(), new FakeOrdersService(), new FakeReviewsService());

            // Act
            var result = await controller.Create(new ReviewDto
            {
                ProductId = 1,
                Rating = 5,
                Description = "This is a good review",
                Title = "I like this product"
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task CreatingAReview_ValidProduct_LoggedIn_UserCantReview_ShouldRedirectToAction()
        {
            // Arrange
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "has-not-ordered"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new ReviewsController(new FakeProductsService(), new FakeOrdersService(), new FakeReviewsService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            var tempData = new TempDataDictionary(contextMock.Object, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            // Act
            var result = await controller.Create(new ReviewDto
            {
                ProductId = 1,
                Rating = 5,
                Description = "This is a good review",
                Title = "I like this product"
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Details");
            Assert.AreEqual(objResult.ControllerName, "Products");
            Assert.AreEqual(tempData.Count, 1);
            Assert.IsTrue(tempData.ContainsKey("error"));
            Assert.IsTrue(tempData.ContainsValue("You cannot review this product."));
        }
    }
}
