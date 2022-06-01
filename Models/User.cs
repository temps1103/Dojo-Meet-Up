using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Exam.Models
{
    public class User
    {
        [Key]
        public int UserId   {get;set;}

        [Required]
        [MinLength(2)]
        public string First_Name {get;set;}

        [Required]
        [MinLength(2)]
        public string Last_Name {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;



        // Weddings User has Created 
        public List<Meet_Up> My_Meet_Ups {get;set;}
        
        // Connect to Many to Many
        public List<Friend> Meet_Up_Attending {get;set;}


        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm_PW {get;set;}
    }
}