using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieStore.Models
{
    public class Movie
    {
        public int MovieID { get; set; }

        public string Title { get; set; }

        public int YearRelease { get; set; }
    }
}