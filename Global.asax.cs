using MvcApplication.Models;
using MvcApplication.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MvcApplication
{
    public class Application : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthenticateRequest += MvcApplication_AuthenticateRequest;
        }

        // ici on pointe sur l'evenement d'authentification de la requette 
        private void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // on recupere vle nom du cookie d'authentication forms
            string cookieName = FormsAuthentication.FormsCookieName;
            // on cherche ca presence dans la collection des cockie de la requete 
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {
                // indisponible
                return;
            }

            FormsAuthenticationTicket authTicket = null;
            try
            {
                // on essaye de decrypter le contenu cookie
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                // partie pour la journalisation
                return;
            }

            if (null == authTicket)
            {
                return;
            }
            string username = authTicket.UserData.Split(';')[0]; // on recupre le nom d'utilisateur precedemment stocker comme userData lors du login
            string role = authTicket.UserData.Split(';')[1]; // et puis on recupere le role 

            FormsIdentity id = new FormsIdentity(authTicket); // on cree une identite forms a partir des infos qu'on a 

            // puis on cree un principal object qui va represente notre identite
            CustomPrincipal principal = new CustomPrincipal(id, role); // a jetter un oeuil sur cette classe 
            principal.User = new User() { Username = username };
            // associer la principal a l'utilisateur de context courant
            Context.User = principal;
        }
    }
}
