﻿using System;
using MetacriticScraper.Infrastructure.HtmlParser;
using MetacriticScraper.Models;
using NUnit.Framework;
using Shouldly;

namespace MetacriticScraper.UnitTests.Infrastructure.HtmlParser
{
    /// <summary>
    /// Includes tests for <see cref="MetacriticGameConverter"/>.
    /// </summary>
    public class MetacriticGameConverterTests
    {
        [Test]
        public void ConvertToGameEntity_WithUnrecognizedParameters_ThrowsArgumentException()
        {
            // Arrange
            var converter = new MetacriticGameConverter();
            var game = new MetacriticGame();

            // Act & Assert
            Should.Throw<Exception>(() => converter.ConvertToGameEntity(game));
        }

        [TestCase("100", "Name", GamePlatform.PC, "Dec 16", "http://sample.org", "invalid")]
        [TestCase("invalid", "Name", GamePlatform.PC, "Dec 16", "http://sample.org", "10")]
        [TestCase("100", "Name", GamePlatform.PC, "invalid", "http://sample.org", "10")]
        public void ConvertToGameEntity_WithUnrecognizedParameters_ThrowsArgumentException(
            string metaScore,
            string name,
            GamePlatform platform,
            string releaseDate,
            string url,
            string userScore)
        {
            // Arrange
            var converter = new MetacriticGameConverter();
            var game = new MetacriticGame()
            {
                MetaScore = metaScore,
                Name = name,
                Platform = platform,
                ReleaseDate = releaseDate,
                Url = url,
                UserScore = userScore,
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => converter.ConvertToGameEntity(game));
        }

        [TestCase("TbD", "\r\t\nName\r\t\n", GamePlatform.PS4, "Dec 16", "/test/", "TbD", null, "Name", GamePlatform.PS4, "12-16", "https://www.metacritic.com/test/", null)]
        [TestCase("100", "Name", GamePlatform.PS4, "Dec 16", "test", "5.5", 100, "Name", GamePlatform.PS4, "12-16", "https://www.metacritic.com/test", 5.5)]
        public void ConvertToGameEntity_WithValidParameters_ReturnsSuccessfully(
            string metaScore,
            string name,
            GamePlatform platform,
            string releaseDate,
            string url,
            string userScore,
            int? expectedMetaScore,
            string expectedName,
            GamePlatform expectedPlatform,
            DateTime expectedReleaseDateWithoutCorrectYearWithoutAYear,
            string expectedUrl,
            decimal? expectedUserScore)
        {
            // Arrange
            var converter = new MetacriticGameConverter();
            var game = new MetacriticGame()
            {
                MetaScore = metaScore,
                Name = name,
                Platform = platform,
                ReleaseDate = releaseDate,
                Url = url,
                UserScore = userScore,
            };

            // Act
            var result = converter.ConvertToGameEntity(game);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.MetaScore.ShouldBe(expectedMetaScore),
                () => result.Name.ShouldBe(expectedName),
                () => result.Platform.ShouldBe(expectedPlatform),
                () => result.ReleaseDate.ShouldBe(DateTime.MinValue),
                () => result.ReleaseDateWithoutCorrectYear.ShouldBe(expectedReleaseDateWithoutCorrectYearWithoutAYear),
                () => result.Url.ShouldBe(expectedUrl),
                () => result.UserScore.ShouldBe(expectedUserScore));
        }
    }
}
