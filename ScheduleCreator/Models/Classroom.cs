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
    using System.Web.Mvc;
    public partial class Classroom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Classroom()
        {
            this.Sections = new HashSet<Section>();
        }
    
        public int classroom_id { get; set; }

        [DisplayName("Building")]
        public int building_id { get; set; }
        public string buildingPrefix { get; set; }

        [DisplayName("Room Number")]
        [Required(ErrorMessage = "A room number is required")]
        [Remote("IsRoomTaken", "Classrooms", AdditionalFields = "building_id")]
        public string roomNumber { get; set; }

        [DisplayName("Capacity")]
        [Required(ErrorMessage = "A classroom capacity is required")]
        public int classroomCapacity { get; set; }

        [DisplayName("Number of Computers")]
        [Required(ErrorMessage = "Number of computers is required")]
        public int computers { get; set; }

        [DisplayName("Available From")]
        [Required(ErrorMessage = "Avaliable from time is required")]
        public System.TimeSpan availableFromTime { get; set; }

        [DisplayName("Available Until")]
        [Required(ErrorMessage = "Avaliable to time is required")]
        public System.TimeSpan availableToTime { get; set; }

        [DisplayName("Active")]
        public string active { get; set; }
    
        public virtual Building Building { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Section> Sections { get; set; }
    }
}
