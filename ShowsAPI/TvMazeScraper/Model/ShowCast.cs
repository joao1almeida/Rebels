using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TvMazeScraper.Model
{
    public class ShowCast
    {
        public int Id { get; set; }
        public int IdShow { get; set; }
        [ForeignKey(nameof(IdShow))]
        public Show Show { get; set; }
        public int IdPerson { get; set; }
        [ForeignKey(nameof(IdPerson))]
        public Person Person { get; set; }
    }
}

