using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SlickBlogApp.Models;

namespace SlickBlogApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BlogPgs",
                "blog/{address}/{action}/{id}/{page}",
                new { controller = "Blog", page = UrlParameter.Optional });

            routes.MapRoute("Blog",
                "blog/{address}/{action}/{id}",
                new { controller = "Blog", action = "Index" ,id = UrlParameter.Optional });



            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Database.SetInitializer<SlickBlogAppContext>(new CreateDatabaseIfNotExists<SlickBlogAppContext>());
            //// Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory = new SqlConnectionFactory("Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            //System.Data.Entity.Database.SetInitializer<SlickBlogAppContext>(new SlickBlogAppInitializer());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
    //public class SlickBlogAppInitializer : CreateDatabaseIfNotExists<SlickBlogAppContext>
    //{
    //    protected override void Seed(SlickBlogAppContext context)
    //    {
    //        context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX__Address ON Blogs (Address)");
    //    }
    //}
}