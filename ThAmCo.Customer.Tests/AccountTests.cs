using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
            Assert.AreEqual(objResult.ActionName, "Update");
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
    }
}
