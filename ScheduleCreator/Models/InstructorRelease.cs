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
    using System.ComponentModel.DataAnnotations;
    public partial class InstructorRelease
    {
        public int instructorRelease_id { get; set; }
        public int instructor_id { get; set; }
        public int semester_id { get; set; }
        [Required(ErrorMessage = "Instructor wnumber is required")]
        public string instructorWNumber { get; set; }
        public string semesterType { get; set; }
        public int semesterYear { get; set; }
        public string releaseDescription { get; set; }
        public Nullable<decimal> totalReleaseHours { get; set; }
    
        public virtual Instructor Instructor { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
