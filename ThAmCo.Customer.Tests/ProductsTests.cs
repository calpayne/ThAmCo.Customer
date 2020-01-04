using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Brands;
using ThAmCo.Customer.Services.Categories;
using ThAmCo.Customer.Services.Products;
using ThAmCo.Customer.Services.Profiles;
using ThAmCo.Customer.Services.Reviews;
using ThAmCo.Customer.Web.Controllers;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Tests
{
    [TestClass]
    public class ProductsTests
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

        private ProductsService CreateMoqProducts(Mock<HttpMessageHandler> mock)
        {
            var client = new HttpClient(mock.Object);
            client.BaseAddress = new System.Uri("https://localhost:44353/");
            var service = new ProductsService(client);
            return service;
        }

        [TestMethod]
        public async Task GetAllProducts_ShouldBeValid()
        {
            // Arrange
            IEnumerable<ProductDto> fakeProducts = new List<ProductDto>
            {
                new ProductDto { Id = 1, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 },
                new ProductDto { Id = 2, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", Name = "Chocolate Cover", Price = 50.25, StockLevel = 12 },
                new ProductDto { Id = 3, Currency = "£", BrandId = 3, CategoryId = 2, Description = "Lamely adapted used and dirty teatowel.  Guaranteed fewer than two holes.", Name = "Cloth Cover", Price = 100.25, StockLevel = 24 },
                new ProductDto { Id = 4, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Especially toughen and harden sponge entirely encases your device to prevent any interaction.", Name = "Harden Sponge Case", Price = 60.25, StockLevel = 4 },
                new ProductDto { Id = 5, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", Name = "Water Bath Case", Price = 20.25, StockLevel = 5 },
                new ProductDto { Id = 6, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only).", Name = "Smartphone Car Holder", Price = 200.25, StockLevel = 13 },
                new ProductDto { Id = 7, Currency = "£", BrandId = 3, CategoryId = 2, Description = "Keep your device on your arm with this general purpose sticky tape.", Name = "Sticky Tape Sport Armband", Price = 20.25, StockLevel = 18 },
                new ProductDto { Id = 8, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Stengthen HB pencils guaranteed to leave a mark.", Name = "Real Pencil Stylus", Price = 35.25, StockLevel = 14 },
                new ProductDto { Id = 9, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Coat your mobile device screen in a scratch resistant, opaque film.", Name = "Spray Paint Screen Protector", Price = 45.25, StockLevel = 8 },
                new ProductDto { Id = 10, Currency = "£", BrandId = 2, CategoryId = 3, Description = "For his or her sensory pleasure. Fits few known smartphones.", Name = "Rippled Screen Protector", Price = 85.25, StockLevel = 5 },
                new ProductDto { Id = 11, Currency = "£", BrandId = 3, CategoryId = 2, Description = "For an odour than lingers on your device.", Name = "Fish Scented Screen Protector", Price = 125.25, StockLevel = 3 },
                new ProductDto { Id = 12, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Guaranteed not to conduct electical charge from your fingers.", Name = "Non-conductive Screen Protector", Price = 99.25, StockLevel = 0 }
            };

            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());
            int[] brands = { 1, 2, 3, 4 };
            int[] categories = { 1, 2, 3, 4 };
            string search = ".";
            double minPrice = -1;
            double maxPrice = double.MaxValue;

            // Act
            var result = await controller.Index(brands, categories, search, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsIndexViewModel;
            Assert.IsNotNull(model);
            var products = model.Products;
            Assert.IsNotNull(products);
            Assert.AreEqual(fakeProducts.Count(), products.Count());
            for (int i = 0; i < products.Count(); i++)
            {
                Assert.AreEqual(fakeProducts.ElementAt(i).Id, products.ElementAt(i).Id);
                Assert.AreEqual(fakeProducts.ElementAt(i).Currency, products.ElementAt(i).Currency);
                Assert.AreEqual(fakeProducts.ElementAt(i).BrandId, products.ElementAt(i).BrandId);
                Assert.AreEqual(fakeProducts.ElementAt(i).CategoryId, products.ElementAt(i).CategoryId);
                Assert.AreEqual(fakeProducts.ElementAt(i).Description, products.ElementAt(i).Description);
                Assert.AreEqual(fakeProducts.ElementAt(i).Name, products.ElementAt(i).Name);
                Assert.AreEqual(fakeProducts.ElementAt(i).Price, products.ElementAt(i).Price);
                Assert.AreEqual(fakeProducts.ElementAt(i).StockLevel, products.ElementAt(i).StockLevel);
            }
        }

        [TestMethod]
        public async Task GetAllProducts_Moq_ShouldBeValid()
        {
            // Arrange
            IEnumerable<ProductDto> fakeProducts = new List<ProductDto>
            {
                new ProductDto { Id = 1, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 },
                new ProductDto { Id = 2, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", Name = "Chocolate Cover", Price = 50.25, StockLevel = 12 },
                new ProductDto { Id = 3, Currency = "£", BrandId = 3, CategoryId = 2, Description = "Lamely adapted used and dirty teatowel.  Guaranteed fewer than two holes.", Name = "Cloth Cover", Price = 100.25, StockLevel = 24 },
                new ProductDto { Id = 4, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Especially toughen and harden sponge entirely encases your device to prevent any interaction.", Name = "Harden Sponge Case", Price = 60.25, StockLevel = 4 },
                new ProductDto { Id = 5, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", Name = "Water Bath Case", Price = 20.25, StockLevel = 5 },
                new ProductDto { Id = 6, Currency = "£", BrandId = 2, CategoryId = 3, Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only).", Name = "Smartphone Car Holder", Price = 200.25, StockLevel = 13 },
                new ProductDto { Id = 7, Currency = "£", BrandId = 3, CategoryId = 2, Description = "Keep your device on your arm with this general purpose sticky tape.", Name = "Sticky Tape Sport Armband", Price = 20.25, StockLevel = 18 },
                new ProductDto { Id = 8, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Stengthen HB pencils guaranteed to leave a mark.", Name = "Real Pencil Stylus", Price = 35.25, StockLevel = 14 },
                new ProductDto { Id = 9, Currency = "£", BrandId = 1, CategoryId = 4, Description = "Coat your mobile device screen in a scratch resistant, opaque film.", Name = "Spray Paint Screen Protector", Price = 45.25, StockLevel = 8 },
                new ProductDto { Id = 10, Currency = "£", BrandId = 2, CategoryId = 3, Description = "For his or her sensory pleasure. Fits few known smartphones.", Name = "Rippled Screen Protector", Price = 85.25, StockLevel = 5 },
                new ProductDto { Id = 11, Currency = "£", BrandId = 3, CategoryId = 2, Description = "For an odour than lingers on your device.", Name = "Fish Scented Screen Protector", Price = 125.25, StockLevel = 3 },
                new ProductDto { Id = 12, Currency = "£", BrandId = 4, CategoryId = 1, Description = "Guaranteed not to conduct electical charge from your fingers.", Name = "Non-conductive Screen Protector", Price = 99.25, StockLevel = 0 }
            };

            var expectedJson = JsonConvert.SerializeObject(fakeProducts);
            var expectedUri = new Uri("https://localhost:44353/");
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
            };

            var mock = CreateHttpMock(expectedResponse);
            var service = CreateMoqProducts(mock);

            var controller = new ProductsController(service, new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());
            int[] brands = { 1, 2, 3, 4 };
            int[] categories = { 1, 2, 3, 4 };
            string search = ".";
            double minPrice = -1;
            double maxPrice = double.MaxValue;

            // Act
            var result = await controller.Index(brands, categories, search, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsIndexViewModel;
            Assert.IsNotNull(model);
            var products = model.Products;
            Assert.IsNotNull(products);
            Assert.AreEqual(fakeProducts.Count(), products.Count());
            for (int i = 0; i < products.Count(); i++)
            {
                Assert.AreEqual(fakeProducts.ElementAt(i).Id, products.ElementAt(i).Id);
                Assert.AreEqual(fakeProducts.ElementAt(i).Currency, products.ElementAt(i).Currency);
                Assert.AreEqual(fakeProducts.ElementAt(i).BrandId, products.ElementAt(i).BrandId);
                Assert.AreEqual(fakeProducts.ElementAt(i).CategoryId, products.ElementAt(i).CategoryId);
                Assert.AreEqual(fakeProducts.ElementAt(i).Description, products.ElementAt(i).Description);
                Assert.AreEqual(fakeProducts.ElementAt(i).Name, products.ElementAt(i).Name);
                Assert.AreEqual(fakeProducts.ElementAt(i).Price, products.ElementAt(i).Price);
                Assert.AreEqual(fakeProducts.ElementAt(i).StockLevel, products.ElementAt(i).StockLevel);
            }
        }

        [TestMethod]
        public async Task GetAllProducts_Moq_NoProducts_ShouldBeValid()
        {
            // Arrange
            IEnumerable<ProductDto> fakeProducts = Array.Empty<ProductDto>();

            var expectedJson = JsonConvert.SerializeObject(fakeProducts);
            var expectedUri = new Uri("https://localhost:44353/");
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
            };

            var mock = CreateHttpMock(expectedResponse);
            var service = CreateMoqProducts(mock);

            var controller = new ProductsController(service, new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());
            int[] brands = { 1, 2, 3, 4 };
            int[] categories = { 1, 2, 3, 4 };
            string search = ".";
            double minPrice = -1;
            double maxPrice = double.MaxValue;

            // Act
            var result = await controller.Index(brands, categories, search, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsIndexViewModel;
            Assert.IsNotNull(model);
            var products = model.Products;
            Assert.IsNotNull(products);
            Assert.AreEqual(fakeProducts.Count(), products.Count());
        }

        [TestMethod]
        public async Task GetAllProducts_WithValidSearch_ShouldBeValid()
        {
            // Arrange
            IEnumerable<ProductDto> fakeProducts = new List<ProductDto>
            {
                new ProductDto { Id = 5, BrandId = 1, CategoryId = 4, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", Name = "Water Bath Case", Price = 20.25, StockLevel = 5 },
                new ProductDto { Id = 9, BrandId = 1, CategoryId = 4, Description = "Coat your mobile device screen in a scratch resistant, opaque film.", Name = "Spray Paint Screen Protector", Price = 45.25, StockLevel = 8 },
                new ProductDto { Id = 10, BrandId = 2, CategoryId = 3, Description = "For his or her sensory pleasure. Fits few known smartphones.", Name = "Rippled Screen Protector", Price = 85.25, StockLevel = 5 }
            };

            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());
            int[] brands = { 1, 2, 3, 4 };
            int[] categories = { 1, 2, 3, 4 };
            string search = "fi";
            double minPrice = 20;
            double maxPrice = 85.25;

            // Act
            var result = await controller.Index(brands, categories, search, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsIndexViewModel;
            Assert.IsNotNull(model);
            var products = model.Products;
            Assert.IsNotNull(products);
            Assert.AreEqual(fakeProducts.Count(), products.Count());
            for (int i = 0; i < products.Count(); i++)
            {
                Assert.AreEqual(fakeProducts.ElementAt(i).Id, products.ElementAt(i).Id);
                Assert.AreEqual(fakeProducts.ElementAt(i).BrandId, products.ElementAt(i).BrandId);
                Assert.AreEqual(fakeProducts.ElementAt(i).CategoryId, products.ElementAt(i).CategoryId);
                Assert.AreEqual(fakeProducts.ElementAt(i).Description, products.ElementAt(i).Description);
                Assert.AreEqual(fakeProducts.ElementAt(i).Name, products.ElementAt(i).Name);
                Assert.AreEqual(fakeProducts.ElementAt(i).Price, products.ElementAt(i).Price);
                Assert.AreEqual(fakeProducts.ElementAt(i).StockLevel, products.ElementAt(i).StockLevel);
            }
        }

        [TestMethod]
        public async Task GetAllProducts_WithNoResultsSearch_ShouldBeEmpty()
        {
            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());
            int[] brands = { 20 };
            int[] categories = { 20 };
            string search = "fi";
            double minPrice = 20;
            double maxPrice = 85.25;

            // Act
            var result = await controller.Index(brands, categories, search, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsIndexViewModel;
            Assert.IsNotNull(model);
            var products = model.Products;
            Assert.IsNotNull(products);
            Assert.AreEqual(0, products.Count());
        }

        [TestMethod]
        public async Task GetProduct_WithValidID_ShouldBeValid()
        {
            // Arrange
            ProductDto fakeProduct = new ProductDto { Id = 1, BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 };

            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsDetailsViewModel;
            Assert.IsNotNull(model);
            var product = model.Product;
            Assert.IsNotNull(product);
            Assert.AreEqual(fakeProduct.Id, product.Id);
            Assert.AreEqual(fakeProduct.BrandId, product.BrandId);
            Assert.AreEqual(fakeProduct.CategoryId, product.CategoryId);
            Assert.AreEqual(fakeProduct.Description, product.Description);
            Assert.AreEqual(fakeProduct.Name, product.Name);
            Assert.AreEqual(fakeProduct.Price, product.Price);
            Assert.AreEqual(fakeProduct.StockLevel, product.StockLevel);
        }

        [TestMethod]
        public async Task GetProduct_WithValidID_Moq_ShouldBeValid()
        {
            // Arrange
            ProductDto fakeProduct = new ProductDto { Id = 1, BrandId = 1, CategoryId = 4, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", Name = "Wrap It and Hope Cover", Price = 10.25, StockLevel = 1 };

            var expectedJson = JsonConvert.SerializeObject(fakeProduct);
            var expectedUri = new Uri("https://localhost:44353/");
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
            };

            var mock = CreateHttpMock(expectedResponse);
            var service = CreateMoqProducts(mock);

            var controller = new ProductsController(service, new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsNotNull(result);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.ViewData.Model as ProductsDetailsViewModel;
            Assert.IsNotNull(model);
            var product = model.Product;
            Assert.IsNotNull(product);
            Assert.AreEqual(fakeProduct.Id, product.Id);
            Assert.AreEqual(fakeProduct.BrandId, product.BrandId);
            Assert.AreEqual(fakeProduct.CategoryId, product.CategoryId);
            Assert.AreEqual(fakeProduct.Description, product.Description);
            Assert.AreEqual(fakeProduct.Name, product.Name);
            Assert.AreEqual(fakeProduct.Price, product.Price);
            Assert.AreEqual(fakeProduct.StockLevel, product.StockLevel);
        }

        [TestMethod]
        public async Task GetProduct_WithInvalidID_ShouldNotFound()
        {
            // Arrange
            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            // Act
            var result = await controller.Details(99999);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as NotFoundResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task Purchasing_ValidProduct_LoggedIn_ShouldRedirectToAction()
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

            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Purchase(new PurchaseViewModel
            {
                Id = 1
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Index");
            Assert.AreEqual(objResult.ControllerName, "Orders");
        }

        [TestMethod]
        public async Task Purchasing_InvalidProduct_LoggedIn_ShouldBadRequest()
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

            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Purchase(new PurchaseViewModel
            {
                Id = 99999
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task Purchasing_ValidProduct_NotLoggedIn_ShouldBadRequest()
        {
            // Arrange
            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            // Act
            var result = await controller.Purchase(new PurchaseViewModel
            {
                Id = 1
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task Purchasing_ValidProduct_LoggedIn_UserCantPurchase_ShouldRedirectToAction()
        {
            // Arrange
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "not-allowed-to-purchase"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new ProductsController(new FakeProductsService(), new FakeBrandsService(), new FakeCategoriesService(), new FakeReviewsService(), new FakeProfilesService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            var tempData = new TempDataDictionary(contextMock.Object, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Purchase(new PurchaseViewModel
            {
                Id = 1
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Details");
            Assert.AreEqual(tempData.Count, 1);
            Assert.IsTrue(tempData.ContainsKey("error"));
            Assert.IsTrue(tempData.ContainsValue("You cannot purchase products. Please make sure that your account information is complete."));
        }
    }
}
