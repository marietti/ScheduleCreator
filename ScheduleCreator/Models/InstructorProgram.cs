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
    public partial class InstructorProgram
    {
        public int instructorProgram_id { get; set; }

        [DisplayName("Program")]
        public int program_id { get; set; }

        [DisplayName("Instrctor")]
        public int instructor_id { get; set; }
        
        public string programPrefix { get; set; }
        public string instructorWNumber { get; set; }
    
        public virtual Instructor Instructor { get; set; }
        public virtual Program Program { get; set; }
    }
}
