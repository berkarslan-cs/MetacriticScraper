using System;
using System.Collections.Generic;
using MetacriticScraper.Infrastructure.Site;
using MetacriticScraper.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetacriticScraper.Controllers
{
    /// <summary>
    /// Main API for Metacritic.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MetacriticController : ControllerBase
    {
        private const string NotValidDateErrorMessage = "Not valid date.";
        private readonly IMetacriticSite site;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetacriticController"/> class.
        /// </summary>
        /// <param name="site">Injected automatically.</param>
        public MetacriticController(IMetacriticSite site) => this.site = site;

        /// <summary>
        /// Returns all of the games which qualify the given filter.
        /// </summary>
        /// <param name="gameFilter">Filter to apply to the games.</param>
        /// <returns>Game list.</returns>
        // GET api/Metacritic
        [HttpGet]
        public ActionResult<IList<Game>> Get([FromQuery]GameFilter gameFilter)
        {
            // Validate input
            if (gameFilter?.MinReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError(nameof(gameFilter.MinReleaseDate), NotValidDateErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Process
            return Ok(site.GetGames(gameFilter));
        }
    }
}
