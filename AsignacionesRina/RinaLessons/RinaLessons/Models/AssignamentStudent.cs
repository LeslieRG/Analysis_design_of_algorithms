namespace RinaLessons.Models{
using System.ComponentModel.DataAnnotations;

    public class AssignamentsStudent{
       
        [Key]
        public int AssignamentId { get; set; }
        public int UserId { get; set; }
        public int SubjectID{get; set;}

        public int HomeworkId{get; set;}
    }
}