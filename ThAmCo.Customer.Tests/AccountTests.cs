using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;
using ThAmCo.Customer.Services.Auth;
using ThAmCo.Customer.Services.Profiles;
using ThAmCo.Customer.Web.Controllers;
using ThAmCo.Customer.Web.Models;

namespace ThAmCo.Customer.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public async Task UpdatingAccount_LoggedIn_ShouldRedirectToAction()
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

            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Update(new UpdateViewModel 
            {
                Id = "f32d935b-f175-4450-a93e-e48711c4d481",
                FirstName = "Esting",
                Surname = "Sting",
                Email = "example@gmail.com",
                TelNo = "1234",
                DeliverAddress1 = "DeliverAddress1",
                DeliverAddress2 = "DeliverAddress2",
                DeliverAddress3 = "DeliverAddress3",
                Postcode = "1234"

            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Index");
            Assert.AreEqual(objResult.ControllerName, "Products");
        }

        [TestMethod]
        public async Task UpdatingAccount_NotLoggedIn_ShouldBadRequest()
        {
            // Arrange
            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            // Act
            var result = await controller.Update(new UpdateViewModel
            {
                Id = "f32d935b-f175-4450-a93e-e48711c4d481",
                FirstName = "Esting",
                Surname = "Sting",
                Email = "example@gmail.com",
                TelNo = "1234",
                DeliverAddress1 = "DeliverAddress1",
                DeliverAddress2 = "DeliverAddress2",
                DeliverAddress3 = "DeliverAddress3",
                Postcode = "1234"

            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task UpdatingAccount_LoggedIn_TryingToUpdateAnotherUsersDetails_ShouldBadRequest()
        {
            // Arrange
            IEnumerable<Claim> fakeClaims = new List<Claim>
            {
                new Claim("sub", "NOT-f32d935b-f175-4450-a93e-e48711c4d481"),
                new Claim("preferred_username", "email@example.com"),
                new Claim("name", "Mr Esting")
            };

            var claimsMock = new Mock<ClaimsPrincipal>();
            claimsMock.Setup(m => m.Claims).Returns(fakeClaims);

            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.Update(new UpdateViewModel
            {
                Id = "f32d935b-f175-4450-a93e-e48711c4d481",
                FirstName = "Esting",
                Surname = "Sting",
                Email = "example@gmail.com",
                TelNo = "1234",
                DeliverAddress1 = "DeliverAddress1",
                DeliverAddress2 = "DeliverAddress2",
                DeliverAddress3 = "DeliverAddress3",
                Postcode = "1234"

            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task LoggingIn_InvalidLogin_ShouldLocalRedirect()
        {
            // Arrange
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                           .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(_ => _.GetService(typeof(IAuthenticationService)))
                               .Returns(authServiceMock.Object);

            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = serviceProviderMock.Object
                    }
                }
            };

            // Act
            var result = await controller.Login(new LoginViewModel
            {
                Email = "example@email.com",
                Password = "Password"
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as LocalRedirectResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public void LoggingIn_WhenAlreadyLoggedIn_ShouldLocalRedirect()
        {
            // Arrange
            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User.Identity.IsAuthenticated).Returns(true);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = controller.Login();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as LocalRedirectResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task RequestDeletion_LoggedIn_ShouldRedirectToAction()
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

            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = await controller.RequestDeletion();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Logout");
        }

        [TestMethod]
        public async Task RequestDeletion_NotLoggedIn_ShouldBadRequest()
        {
            // Arrange
            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            // Act
            var result = await controller.RequestDeletion();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as BadRequestResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public void Register_NotLoggedIn_ShouldShowView()
        {
            // Arrange
            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User.Identity.IsAuthenticated).Returns(false);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as ViewResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public void Register_LoggedIn_ShouldLocalRedirect()
        {
            // Arrange
            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User.Identity.IsAuthenticated).Returns(true);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as LocalRedirectResult;
            Assert.IsNotNull(objResult);
        }

        [TestMethod]
        public async Task Register_NotLoggedIn_ValidData_ShouldRedirectToAction()
        {
            // Arrange
            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            RegisterViewModel rvm = new RegisterViewModel
            {
                EmailAddress = "test@email.com",
                Password = "Password123",
                Username = "username"
            };

            // Act
            var result = await controller.Register(rvm);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as RedirectToActionResult;
            Assert.IsNotNull(objResult);
            Assert.AreEqual(objResult.ActionName, "Login");
        }

        [TestMethod]
        public void Register_NotLoggedIn_InvalidData_ShouldBeFalse()
        {
            // Arrange
            var validationResultList = new List<ValidationResult>();
            RegisterViewModel rvm = new RegisterViewModel
            {
                EmailAddress = "test@email.com",
                Password = "Password123"
            };

            // Act
            var result = Validator.TryValidateObject(rvm, new ValidationContext(rvm), validationResultList, true);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResultList.Count);
            Assert.AreEqual("Username", validationResultList[0].MemberNames.ElementAt(0));
            Assert.AreEqual("The Username field is required.", validationResultList[0].ErrorMessage);
        }

        [TestMethod]
        public async Task Register_NotLoggedIn_WithInvalidModelState_ShouldReturnView()
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

            var controller = new AccountController(new FakeProfilesService(), new FakeAuthService());

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(claimsMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            };

            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = await controller.Register(new RegisterViewModel
            {
                EmailAddress = "test@email.com",
                Password = "Password123"
            });

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as ViewResult;
            Assert.IsNotNull(objResult);
        }
    }
}
