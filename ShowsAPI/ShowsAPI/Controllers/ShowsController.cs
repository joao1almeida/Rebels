using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Model;
using TvMazeScraper.Services;
using TvMazeScraper.Services.Interfaces;

namespace ShowsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IScraper _ScraperService;

        public ShowsController(IScraper TvMazeScraperService)
        {
            _ScraperService = TvMazeScraperService;
        }

        // GET: api/Shows/1
        [HttpGet("{page}")]
        public async Task<List<Show>> GetShows(int page)
        {
            return await _ScraperService.GetShowsAsync(page, 10);
        }
    }
}