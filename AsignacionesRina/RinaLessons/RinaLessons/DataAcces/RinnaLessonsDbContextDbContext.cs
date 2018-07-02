using Microsoft.EntityFrameworkCore;
using RinaLessons.Models;


namespace RinaLessons.DataAcces{
  
    public class RinnaLessonsDbContext : DbContext{
        public RinnaLessonsDbContext(DbContextOptions<RinnaLessonsDbContext> data)
        :base (data){}


        public DbSet<User> Users{get; set;}
        public DbSet<Homework> HomeWorks{get; set;}
        public DbSet<Enrollment> Enrollments{get; set;}
        public DbSet<Subject> Subjects{get; set;}
     
        public DbSet<AssignamentsStudent> Asigned {get; set;}
    }
}