using System;
using System.ComponentModel.DataAnnotations;

namespace  RinaLessons.Models
{
    public class User{
        [Key]
        public int UserId { get; set; }
        public string  UserName { get; set; } 

        public string LastName { get; set; }
        public string   Cellphone { get; set; }

        public string  Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Role {get; set;}

        public string Password {get; set;}
    }
}