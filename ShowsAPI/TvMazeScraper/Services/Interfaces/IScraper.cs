using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TvMazeScraper.Model;

namespace TvMazeScraper.Services.Interfaces
{
    public interface IScraper
    {
        Task<List<Show>> GetShowsAsync(int page, int pageSize);
    }
}
