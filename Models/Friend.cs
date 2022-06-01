using System;
using System.ComponentModel.DataAnnotations;

namespace Final_Exam.Models
{
    public class Friend
    {
        [Key]
        public int FriendId {get;set;}
        public int UserId {get;set;}
        public int Meet_UpId {get;set;}

        public User User {get;set;}
        public Meet_Up Meet_Up {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}