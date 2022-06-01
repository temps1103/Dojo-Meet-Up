using Microsoft.EntityFrameworkCore;

namespace Final_Exam.Models
{ 
    // the MyContext class representing a session with our MySQL 
    // database allowing us to query for or save data
    public class MyContext : DbContext 
    { 
        public MyContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Meet_Up> Meet_Ups {get;set;}
        public DbSet<Friend> Friends {get;set;}
    }
}
