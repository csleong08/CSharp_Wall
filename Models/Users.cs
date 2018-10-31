using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wall.Models
{
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public Users()
        {
            Message = new List<Messages>();
            Comment = new List<Comments>();
        }
        public List<Messages> Message;
        public List<Comments> Comment;
    }
}