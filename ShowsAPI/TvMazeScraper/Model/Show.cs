using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Model
{
    public class Show
    {
        public Show(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Person> Cast { get; set; }
    }
}
