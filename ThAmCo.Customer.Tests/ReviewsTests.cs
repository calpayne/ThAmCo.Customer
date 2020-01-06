using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Auth;
using ThAmCo.Customer.Services.Orders;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Reviews;
using ThAmCo.Customer.Web.Controllers;

namespace ThAmCo.Customer.Tests
{
    [TestClass]
    public class ReviewsTests
    {
        private Mock<HttpMessageHandler> CreateHttpMock(HttpResponseMessage expected)
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expected)
                .Verifiable();

            return mock;
        }

        private ReviewsService CreateMoqReviews(Mock<HttpMessageHandler> mock)
        {
            var client = new HttpClient(mock.Object);
            client.BaseAddress = new System.Uri("https://localhost:44353/");
            var service = new ReviewsService(client, new FakeAuthService());
            return service;
        }

        [TestMethod]
        public async Task GettingReviews_ValidProduct_ShouldReturnReviews()
        {
            // Arrange
            IEnumerable<ReviewDto> fakeReviews = new List<ReviewDto>
            {
                new ReviewDto { Id = 1, ProductId = 1, Title = "Amazing!", Description = "I really think that this product is outstanding!", Rating = 5 },
                new ReviewDto { Id = 2, ProductId = 1, Title = "Good!", Description = "I really think that this product is good!", Rating = 4 },
                new ReviewDto { Id = 3, ProductId = 1, Title = "Ok!", Description = "I really think that this product is ok!", Rating = 3 },
                new ReviewDto { Id = 4, ProductId = 1, Title = "Meh!", Description = "I really think that this product is meh!", Rating = 2 },
                new ReviewDto { Id = 5, ProductId = 1, Title = "Bad!", Description = "I really think that this product is bad!", Rating = 1 }
            };

            var service = new FakeReviewsService();
            var productId = 1;

            // Act
            var result = await service.GetAllAsync(productId);

            // Assert
            Assert.IsNotNull(result);
            var reviews = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviews);
            Assert.AreEqual(fakeReviews.Count(), reviews.Count());
            for (int i = 0; i < reviews.Count(); i++)
            {
                Assert.AreEqual(fakeReviews.ElementAt(i).Id, reviews.ElementAt(i).Id);
                Assert.AreEqual(fakeReviews.ElementAt(i).Description, reviews.ElementAt(i).Description);
                Assert.AreEqual(fakeReviews.ElementAt(i).Title, reviews.ElementAt(i).Title);
                Assert.AreEqual(fakeReviews.ElementAt(i).Rating, reviews.ElementAt(i).Rating);
                Assert.AreEqual(fakeReviews.ElementAt(i).ProductId, reviews.ElementAt(i).ProductId);
            }
        }

        [TestMethod]
        public async Task GettingReviews_ValidProduct_Moq_ShouldReturnReviews()
        {
            // Arrange
            IEnumerable<ReviewDto> fakeReviews = new List<ReviewDto>
            {
                new ReviewDto { Id = 1, ProductId = 1, Title = "Amazing!", Description = "I really think that this product is outstanding!", Rating = 5 },
                new ReviewDto { Id = 2, ProductId = 1, Title = "Good!", Description = "I really think that this product is good!", Rating = 4 },
                new ReviewDto { Id = 3, ProductId = 1, Title = "Ok!", Description = "I really think that this product is ok!", Rating = 3 },
                new ReviewDto { Id = 4, ProductId = 1, Title = "Meh!", Description = "I really think that this product is meh!", Rating = 2 },
                new ReviewDto { Id = 5, ProductId = 1, Title = "Bad!", Description = "I really think that this product is bad!", Rating = 1 }
            };

            var expectedJson = JsonConvert.SerializeObject(fakeReviews);
            var expectedUri = new Uri("https://localhost:44353/");
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
            };

            var mock = CreateHttpMock(expectedResponse);
            var service = CreateMoqReviews(mock);
            var productId = 1;

            // Act
            var result = await service.GetAllAsync(productId);

            // Assert
            Assert.IsNotNull(result);
            var reviews = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviews);
            Assert.AreEqual(fakeReviews.Count(), reviews.Count());
            for (int i = 0; i < reviews.Count(); i++)
            {
                Assert.AreEqual(fakeReviews.ElementAt(i).Id, reviews.ElementAt(i).Id);
                Assert.AreEqual(fakeReviews.ElementAt(i).Description, reviews.ElementAt(i).Description);
                Assert.AreEqual(fakeReviews.ElementAt(i).Title, reviews.ElementAt(i).Title);
                Assert.AreEqual(fakeReviews.ElementAt(i).Rating, reviews.ElementAt(i).Rating);
                Assert.AreEqual(fakeReviews.ElementAt(i).ProductId, reviews.ElementAt(i).ProductId);
            }
        }

        [TestMethod]
        public async Task GettingReviews_InvalidProduct_ShouldReturnEmpty()
        {
            // Arrange
            var service = new FakeReviewsService();
            var productId = 9999;

            // Act
            var result = await service.GetAllAsync(productId);

            // Assert
            Assert.IsNotNull(result);
            var reviews = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviews);
            Assert.AreEqual(0, reviews.Count());
        }

        [TestMethod]
        public async Task GettingReviews_InvalidProduct_Moq_ShouldReturnEmpty()
        {
            // Arrange
            var expectedJson = JsonConvert.SerializeObject(Array.Empty<ReviewDto>());
            var expectedUri = new Uri("https://localhost:44353/");
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
            };

            var mock = CreateHttpMock(expectedResponse);
            var service = CreateMoqReviews(mock);
            var productId = 9999;

            // Act
            var result = await service.GetAllAsync(productId);

            // Assert
            Assert.IsNotNull(result);
            var reviews = result as IEnumerable<ReviewDto>;
            Assert.IsNotNull(reviews);
            Assert.AreEqual(0, reviews.Count());
        }

        [TestMethod]
        public async Task CreatingAReview_ValidProduct_LoggedIn_ShouldShowView()
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
            var result = await controller.Create(1);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
        }

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

        [TestMethod]
        public void CreateAReview_ModelValidate_ShouldBeFalse()
        {
            // Arrange
            var validationResultList = new List<ValidationResult>();
            ReviewDto review = new ReviewDto
            {
                ProductId = 1,
                Rating = 6,
                Description = "This is a good review",
                Title = "I like this product"
            };

            // Act
            var result = Validator.TryValidateObject(review, new ValidationContext(review), validationResultList, true);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResultList.Count);
            Assert.AreEqual("Rating", validationResultList[0].MemberNames.ElementAt(0));
            Assert.AreEqual("The field Rating must be between 1 and 5.", validationResultList[0].ErrorMessage);
        }

        [TestMethod]
        public async Task CreateAReview_WithInvalidModelState_ShouldReturnView()
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

            controller.ModelState.AddModelError("test", "test");

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
            var objResult = result as ViewResult;
            Assert.IsNotNull(objResult);
        }
    }
}
