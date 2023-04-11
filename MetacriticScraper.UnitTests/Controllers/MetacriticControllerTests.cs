using System;
using MetacriticScraper.Controllers;
using MetacriticScraper.Infrastructure.Site;
using MetacriticScraper.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Controllers
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticController"/>.
    /// </summary>
    public class MetacriticControllerTests
    {
        [Test]
        public void Get_WithIncorrectGameFilter_ReturnsError()
        {
            // Arrange
            var siteMock = new Mock<IMetacriticSite>();
            var controller = new MetacriticController(siteMock.Object);
            var gameFilter = new GameFilter
            {
                MinMetaScore = 101,
            };
            controller.ModelState.AddModelError(nameof(GameFilter.MinMetaScore), $"{nameof(GameFilter.MinMetaScore)} is not in range!");

            // Act
            var result = controller.Get(gameFilter);

            // Assert
            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Get_WithCorrectGameFilter_ReturnsOk()
        {
            // Arrange
            var siteMock = new Mock<IMetacriticSite>();
            var controller = new MetacriticController(siteMock.Object);
            var gameFilter = new GameFilter
            {
                MinReleaseDate = DateTime.Now,
            };

            // Act
            var result = controller.Get(gameFilter);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }
    }
}