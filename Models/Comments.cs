using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wall.Models
{
    public class Comments
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string comment { get; set; }
        [Required]
        public DateTime updated_at { get; set; }
        [Required]
        public DateTime created_at { get; set; }
        public int usersid { get; set; }
        public Users Users { get; set; }
        public int messagesid { get; set; }
        public Messages Messages { get; set; }
    }
}