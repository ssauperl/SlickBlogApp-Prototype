using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using SlickBlogApp.Models;

namespace SlickBlogApp.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {

        SlickBlogAppContext _db = new SlickBlogAppContext();
        public JsonResult IsAddress_Available(string address)
        {

            if (!_db.Blogs.Any(b => b.Address == address))
                return Json(true, JsonRequestBehavior.AllowGet);

            string suggestedUID = String.Format(CultureInfo.InvariantCulture,
                "{0} is not available.", address);

            for (int i = 1; i < 100; i++)
            {
                string altCandidate = address + i.ToString();
                if (!_db.Blogs.Any(b => b.Address == altCandidate))
                {
                    suggestedUID = String.Format(CultureInfo.InvariantCulture,
                   "{0} is not available. Try {1}.", address, altCandidate);
                    break;
                }
            }
            return Json(suggestedUID, JsonRequestBehavior.AllowGet);
        }

    }
}
