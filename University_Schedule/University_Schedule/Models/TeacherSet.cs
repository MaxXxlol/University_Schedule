//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace University_Schedule.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeacherSet
    {
        public TeacherSet()
        {
            this.CourseSet = new HashSet<CourseSet>();
        }
    
        public int Id { get; set; }
        public string FIO { get; set; }
    
        public virtual ICollection<CourseSet> CourseSet { get; set; }
    }
}
