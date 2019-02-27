using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookInfo.Models
{
    // This view model is used for API data, there is no view to go with it.
    public class BookViewModel
    {
        public int BookID { get; set; }
        public String Title { get; set; }
        // Publication date
        public String Date { get; set; }
        // Author's name
        public String Name { get; set; }
        // Author's birth date
        public String Birthday { get; set; }
    }
}
