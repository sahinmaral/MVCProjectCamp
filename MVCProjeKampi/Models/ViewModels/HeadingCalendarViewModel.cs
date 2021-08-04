using System;

namespace MVCProjeKampi.Models.ViewModels
{
    public class HeadingCalendarViewModel
    {
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string url { get; set; }
        public string backgroundColor { get; set; }

        
    }
}