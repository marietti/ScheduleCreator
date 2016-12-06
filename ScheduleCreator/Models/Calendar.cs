using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleCreator.Models
{
    public class Calendar
    {
        string startDate  { get; set; }
        string timeFormat { get; set; }
        string columnDateFormat { get; set; }
        string minTime { get; set; }
        string maxTime { get; set; }
        int slotDuration { get; set; }
        int timeGranularity { get; set; }

        List<Event> events;

        string overlapColor { get; set; }
        string overlapTextColor { get; set; }
        string overlapTitle { get; set; }

        public Calendar(List<Event> events, string maxTime, string minTime = "07:00:00")
        {
            startDate = "01-01-1900"; // OR 31/10/2104
            timeFormat = "hh:mm a";
            columnDateFormat = "ddd";
            slotDuration = 30;
            timeGranularity = 15;
            overlapColor = "#F00";
            overlapTextColor = "#FFF";
            overlapTitle = "Multiple";
        }
    }

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
        /// <summary>
        /// Generate a dictionary with all the instructors sections as events for the calendar
        /// </summary>
        /// <param name="Instructors"></param>
        /// <returns>Dictionary<int, string> instructorEvents</returns>
        public static Dictionary<int, string> GetInstructorEvents(List<Instructor> Instructors)
        {
            Dictionary<int, string> instructorEvents = new Dictionary<int, string>();
            foreach (Instructor instructor in Instructors)
            {
                foreach (Section section in instructor.Sections)
                {
                    if (section.daysTaught != null)
                    {
                        List<Event> events = new List<Event>();
                        foreach (char day in section.daysTaught)
                        {
                            string daysToAdd = "0";
                            daysToAdd = char.ToLower(day) == 'm' ? "1" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 't' ? "2" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 'w' ? "3" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 'r' ? "4" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 'f' ? "5" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 's' ? "6" : daysToAdd;

                            string date = "0" + daysToAdd + "-01-1900";
                            string startTime = section.courseStartTime.ToString();
                            string endTime = section.courseEndTime.ToString();
                            startTime = date + " " + startTime;
                            endTime = date + " " + endTime;

                            string id = section.section_id.ToString();
                            string title = section.coursePrefix + section.courseNumber + "<br/>" + section.buildingPrefix + section.roomNumber;
                            string start = startTime;
                            string end = endTime;
                            Event temp = new Event(id, title, start, end);
                            events.Add(temp);
                        }
                        string jsonEvents = JsonConvert.SerializeObject(events);
                        instructorEvents.Add(section.section_id, jsonEvents);

                    }
                }
            }
            return instructorEvents;
        }
    }

}