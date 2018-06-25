using System;

namespace RinaLessons.Models{

    public class Subject{
        public int SubjectId { get; set; }
        public int  UserId { get; set; }
        public string SubjectName { get; set; }

        public DateTime CreatedOn { get; set; }
        public byte Status { get; set; }
    }
}