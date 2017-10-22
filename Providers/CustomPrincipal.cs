using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MvcApplication.Providers
{
    public class CustomPrincipal : IPrincipal
    {
        private IIdentity _identity;
        private AppDataContext _context;
        private string _role;

        public CustomPrincipal(IIdentity identity)
        {
            _identity = identity;
            _context = new AppDataContext();
        }

        public CustomPrincipal(IIdentity identity, string role) : this(identity)
        {
            this._role = role;
        }

        public User User { get; set; }

        public IIdentity Identity => _identity;
        public bool IsInRole(string role)
        {
            return !String.IsNullOrEmpty(_role)
                ? _role == role
                :_context.Users.Include("Role").FirstOrDefault(u => u.Username == User.Username)?.Role.RoleName == role;
        }
    }
}