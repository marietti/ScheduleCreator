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
    public partial class Building
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Building()
        {
            this.Classrooms = new HashSet<Classroom>();
        }

        public int building_id { get; set; }
        [Required(ErrorMessage = "A Building Prefix is required")]
        public string buildingPrefix { get; set; }
        [Required(ErrorMessage = "A Building Name is required")]
        public string buildingName { get; set; }
        [Required(ErrorMessage = "A Campus Prefix is required")]
        public string campusPrefix { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}
