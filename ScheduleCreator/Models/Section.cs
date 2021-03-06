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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public partial class Section
    {
        // These are also database constraints if you want to add or modify these then you need to make changes to the
        // Database as well
        public static Dictionary<string, string> CourseTypes = new Dictionary<string, string> { { "Traditional", "TRAD" }, { "Online", "ONL" }, { "Hybrid", "HYB" } };
        public static Dictionary<string, string> BlockTypes = new Dictionary<string, string> { { "Semester", "S" }, { "First Block", "FB" }, { "Second Block", "SB" } };
        // A list of the posible characters for daysTaught
        public static List<string> days = new List<string> {"M", "T", "W", "R", "F", "S"};

        public int section_id { get; set; }
        [DisplayName("Course")]
        [Required(ErrorMessage = "Course is required")]
        public int course_id { get; set; }

        [DisplayName("Classroom")]
        public Nullable<int> classroom_id { get; set; }

        [DisplayName("Instructor")]
        public Nullable<int> instructor_id { get; set; }

        [DisplayName("Semester")]
        public Nullable<int> semester_id { get; set; }

        public string coursePrefix { get; set; }
        public string courseNumber { get; set; }
        public string buildingPrefix { get; set; }
        public string roomNumber { get; set; }
        public string instructorWNumber { get; set; }
        public string semesterType { get; set; }
        public Nullable<int> semesterYear { get; set; }

        [DisplayName("CRN")]
        [StringLength(10)]
        public string crn { get; set; }

        [DisplayName("Days Taught")]
        public string daysTaught { get; set; }

        [DisplayName("Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> courseStartTime { get; set; }

        [DisplayName("End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> courseEndTime { get; set; }

        [DisplayName("Block")]
        [Required(ErrorMessage = "Block is required")]
        public string block { get; set; }

        [DisplayName("Course Type")]
        [Required(ErrorMessage = "Course type is required")]
        public string courseType { get; set; }

        [DisplayName("Pay")]
        [StringLength(50)]
        public string pay { get; set; }

        [DisplayName("Section Capacity")]
        [Required(ErrorMessage = "Section capacity is required")]
        public int sectionCapacity { get; set; }

        [DisplayName("Credits")]
        public Nullable<decimal> creditLoad { get; set; }

        [DisplayName("Overload Credits")]
        public Nullable<decimal> creditOverload { get; set; }

        [DisplayName("Comments")]
        [StringLength(255)]
        public string comments { get; set; }
    
        public virtual Classroom Classroom { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
