using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScheduleCreator.Models;

namespace ScheduleCreator.ViewModel
{
    public class SectionByInstructorData
    {

        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}