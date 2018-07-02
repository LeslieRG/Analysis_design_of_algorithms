using System;
using System.ComponentModel.DataAnnotations;


namespace RinaLessons.Models{

    public class Subject{
        [Key]
        public int SubjectId { get; set; }
        public int  UserId { get; set; }
        public string SubjectName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}