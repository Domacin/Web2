using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class User
    {
        public User() { }
        public enum TIP { ADMIN, CONSUMER, DELIVERER }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
        public TIP Tip { get; set; }
        public string Image { get; set; }
        public bool Verified { get; set; }
    }
}