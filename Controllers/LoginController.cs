using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication.Controllers
{
    public class LoginController : Controller
    {
        AppDataContext _context;

        public LoginController()
        {
            _context = new AppDataContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Exclude = "Email,Id")]User model)
        {
            if (!ModelState.IsValid) return View(model);

            // on recupere depuis le DbContext l'utilisateur ayant le username et password qui coincide a ceux envoyer 
            var user = _context.Users.Include("Role").FirstOrDefault(u => u.Username == model.Username && u.password == model.password);
            // si on a un resultat 
            if (user != null)
            {
                // on cree un ticket d'authentification Forms 
                // Note: sur le fichier web.config sur la partie <authentication mode="Forms"/>
                // sert a specifier le type d'authentification a utiliser
                FormsAuthenticationTicket ticket = 
                    new FormsAuthenticationTicket(version: 1, name: model.Username, 
                                        issueDate: DateTime.Now, expiration: DateTime.Now.AddMinutes(20), 
                                        isPersistent: false, 
                                        userData: String.Format("{0};{1}",user.Username, user.Role.RoleName));
                // on crypte le ticket 
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                // et on en cree un cookie
                HttpCookie authCookie =
                             new HttpCookie(FormsAuthentication.FormsCookieName,
                                            encryptedTicket);
                // puis on l'ajoute a la liste de cookie 
                Response.Cookies.Add(authCookie);
                // le reste de la logique se deroule sur le fichier Global.asax.cs
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Username", "Authentication Failed");
            return View(model);
        }
    }
}