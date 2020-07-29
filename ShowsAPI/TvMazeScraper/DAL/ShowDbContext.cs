using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TvMazeScraper.Model;

namespace TvMazeScraper.DAL
{
    public class ShowDbContext : DbContext
    {
        public ShowDbContext(DbContextOptions<ShowDbContext> options)
            : base(options)
        {
        }
        public DbSet<Show> Show { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<ShowCast> ShowCast { get; set; }
    }
}
