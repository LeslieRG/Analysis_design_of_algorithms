using System;
using System.ComponentModel.DataAnnotations;

namespace RinaLessons.Models{
    public class Enrollment{
        [Key]
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte status {get; set;}
    }
}