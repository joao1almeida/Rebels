using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Model;
using TvMazeScraper.Services.Interfaces;
using Flurl;
using Flurl.Http;
using TvMazeScraper.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace TvMazeScraper.Services
{
    public class Scraper : IScraper
    {
        private const string API_URL = "http://api.tvmaze.com";

        private readonly Func<ShowDbContext> _contextFactory;

        public Scraper(Func<ShowDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Show>> GetShowsAsync(int page, int pageSize)
        {
            using (var _context = _contextFactory())
            {
                // pagination
                var shows = await _context.Show.Where(x => x.Id > (page - 1) * pageSize && x.Id <= ((page - 1) * pageSize) + pageSize).ToListAsync();

                // getting show cast
                foreach (var item in shows)
                {
                    item.Cast = await _context.ShowCast.Where(x => x.Show == item).Select(x => x.Person).ToListAsync();
                }

                // getting missing IDs from the API
                var existingIds = shows.Select(x => x.Id).ToList();
                var missingIds = Enumerable.Range((page - 1) * pageSize + 1, pageSize).Except(existingIds).ToList();

                foreach (var id in missingIds)
                {
                    // it's possible the TvMazeAPI to return null if the ID isn't associated with a show, so we're checking if it's null
                    var newShowFromAPI = await GetShowFromAPI(id);
                    if (newShowFromAPI != null)
                        shows.Add(newShowFromAPI);
                }

                // ordering by birthday descending
                foreach (var show in shows)
                {
                    if (show.Cast != null)
                        show.Cast = show.Cast.OrderByDescending(x => x.Birthday).ToList();
                }

                return shows;
            }
        }

        private async Task<Show> GetShowFromAPI(int showid)
        {
            // get show from the API with the given show ID
            var currentTvMazeShow = new ShowTvMaze();
            try
            {
                currentTvMazeShow = (await API_URL
                    .AppendPathSegments("shows", showid)
                    .SetQueryParam("embed", "cast")
                    .GetJsonAsync<ShowTvMaze>());
            }
            catch (FlurlHttpException ex)
            {
                // here we would log ex
                return null;
            }

            // show and show cast in a separate lists so it's easier to work with
            var currentShowCast = new List<Person>();
            foreach (var item in currentTvMazeShow._embedded.Cast)
            {
                currentShowCast.Add(item.person);
            }

            // removing duplicates, it can happen one person being mentioned in the cast multiple times, having multiple entries
            currentShowCast = currentShowCast.GroupBy(x => x.Id).Select(y => y.First()).ToList();

            var currentShow = new Show(currentTvMazeShow.Id, currentTvMazeShow.Name);

            // persist show and cast
            using (var _context = _contextFactory())
            {
                // adding without checking if the entry exists, because if it did, this code wouldn't be executed
                await _context.Show.AddAsync(currentShow);

                // getting person ids that are already exists in DB
                var currentPersonIds = currentShowCast.Select(x => x.Id).ToList();
                var personDictionary = await _context.Person.Where(x => currentPersonIds.Contains(x.Id)).ToDictionaryAsync(x=> x.Id);

                foreach (var item in currentShowCast)
                {
                    // add person if doesn't exist
                    if (personDictionary.ContainsKey(item.Id) == false)
                    {
                        await _context.Person.AddAsync(item);
                        personDictionary.Add(item.Id, item);
                    }

                    // add showcast to relate show and current person
                    var currentShowCastPerson = new ShowCast()
                    {
                        Show = currentShow,
                        Person = personDictionary[item.Id]
                    };

                    await _context.ShowCast.AddAsync(currentShowCastPerson);
                }
                await _context.SaveChangesAsync();
            }
            currentShow.Cast = currentShowCast;
            return currentShow;
        }
    }
}
