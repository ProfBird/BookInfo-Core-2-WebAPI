using System;

namespace BookInfo.Models
{
    // This view model is used for API data, there is no view to go with it.
    public class PatchViewModel
    {
        public String Op { get; set; }
        public String Path { get; set; }
        public String Value { get; set; }
    }
}
