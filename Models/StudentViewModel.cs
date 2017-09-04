using System;
using System.Collections.Generic;

namespace CORE2.Models
{
    public class StudentViewModel
    {   
        public Student Students { get; set; }
        public ICollection<Student> StudentsList { get; set; }
    }
}