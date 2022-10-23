using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class Users
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Title is required.")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Title is required.")]

        public string Email { get; set; }
        public int Profile { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }

        public List<UsersProfiles> listProfiles { get; set; }
        public bool Confirm { get; set; }
        public string msgError { get; set; }
        public bool ErrorExists { get; set; }

        public List<Users> UserLists { get; set; }

    }


}
