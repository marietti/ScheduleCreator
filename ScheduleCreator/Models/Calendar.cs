using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleCreator.Models
{
    public class Calendar
    {
        public class Event
        {
            public string id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string backgroundColor { get; set; }
            public string textColor { get; set; }

            public Event(string id, string title, string start, string end)
            {
                this.id = id;
                this.title = title;
                this.start = start;
                this.end = end;
                backgroundColor = "#492365";
                textColor = "#FFF";
            }
        }
    }
}