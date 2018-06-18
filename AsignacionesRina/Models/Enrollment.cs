using System;

namespace RinaLessons.Models{
    public class Enrollment{
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}