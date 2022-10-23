using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCrud.Models;

namespace TestCrud.Connection
{
    public class ConnSqlServer : DbContext
    {

        public ConnSqlServer(DbContextOptions<ConnSqlServer> options) : base(options) {

        }

        public DbSet<Users> Users { get; set; }

        public DbSet<UsersProfiles> Users_Profiles { get; set; }
    }
}
