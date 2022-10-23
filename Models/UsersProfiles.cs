using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class UsersProfiles
    {
        [Key]
        public long Usersid { get; set; }
        public string Description { get; set; }
    }

}
