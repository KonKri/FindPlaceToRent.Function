using System;

namespace FindPlaceToRent.Function.Models
{
    public class Ad
    {
        public string Title { get; set; }
        public string Characteristics { get; set; }
        public string Details { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}