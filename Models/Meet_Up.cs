using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Final_Exam.Models
{
    public class Meet_Up
    {
        [Key]
        public int Meet_UpId {get;set;}

        [Required]
        [MinLength(2)]
        public string Title {get;set;}

        [Required]
        [FutureDateCheck]
        public DateTime Date_and_Time {get;set;}

        [Required]
        public int Duration {get;set;}
        public string Duration_frame {get;set;}

        [Required]
        public string Description {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;




        // User Creating the Wedding (ex Id #1)
        public int UserId {get;set;}

        // The User Creatiing the Wedding (bookmark to User Info)
        public User Event_Coordinator {get;set;}
        
        // Connect to Many to Many
        public List<Friend> Users_Already_Joined {get;set;}
    }

    public class FutureDateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime minDate = (DateTime)value;
            return minDate <= DateTime.Now ? new ValidationResult("Can't go back in time! Please schedule a Meet Up for a Future Date and Time.") : ValidationResult.Success;
        }
    }
}