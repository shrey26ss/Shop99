using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string name { get; set; }
        public Role role { get; set; }
    }
    public class UserDetails: UserInfo
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
