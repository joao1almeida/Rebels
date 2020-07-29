using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Model
{
    /// <summary>
    /// This class is to mirror the JSON structure returned from the TvMaze API
    /// </summary>
    public class ShowTvMaze
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Embedded _embedded { get; set; }
    }

    /// <summary>
    /// This class is to mirror the JSON structure returned from the TvMaze API
    /// </summary>
    public class Embedded
    {
        public List<Cast> Cast { get; set; }
    }

    /// <summary>
    /// This class is to mirror the JSON structure returned from the TvMaze API
    /// </summary>
    public class Cast
    {
        public Person person { get; set; }
    }
}
