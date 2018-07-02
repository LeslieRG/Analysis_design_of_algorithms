using System;
using System.ComponentModel.DataAnnotations;

namespace RinaLessons.Models{

    public class Homework{
        [Key]
        public int HomeworkId  { get; set; }
        public int SubjectId   { get; set; }
  
        public int TeacherId {get; set;}

        public DateTime? CreatedOn {get; set;}

        public string Description { get; set; }

        public string  Title { get; set; }
        
    }
}
