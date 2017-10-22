using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class AppDataContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string password { get; set; }
        public string Email { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}