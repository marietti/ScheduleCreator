//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScheduleCreator.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Section
    {
        public int section_id { get; set; }
        public int course_id { get; set; }
        public Nullable<int> classroom_id { get; set; }
        public Nullable<int> instructor_id { get; set; }
        public Nullable<int> semester_id { get; set; }
        public string coursePrefix { get; set; }
        public string courseNumber { get; set; }
        public string buildingPrefix { get; set; }
        public string roomNumber { get; set; }
        public string instructorWNumber { get; set; }
        public string semesterType { get; set; }
        public Nullable<int> semesterYear { get; set; }
        public string crn { get; set; }
        public string daysTaught { get; set; }
        public System.TimeSpan courseStartTime { get; set; }
        public System.TimeSpan courseEndTime { get; set; }
        public string block { get; set; }
        public string courseType { get; set; }
        public string pay { get; set; }
        public int sectionCapacity { get; set; }
        public Nullable<decimal> creditLoad { get; set; }
        public Nullable<decimal> creditOverload { get; set; }
        public string comments { get; set; }
    
        public virtual Classroom Classroom { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
