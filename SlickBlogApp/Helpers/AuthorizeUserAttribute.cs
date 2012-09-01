using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SlickBlogApp.Models;

namespace SlickBlogApp.Helpers
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                // The user is not authorized => no need to continue
                return false;
            }

            // At this stage we know that the user is authorized => we can fetch
            // the username
            string username = httpContext.User.Identity.Name;

            // Now let's fetch the blog address from the request
            string address = httpContext.Request.QueryString.Get("address");
            string rawUrl = httpContext.Request.RawUrl;
            address = Regex.Split(rawUrl, "/")[2];

            // All that's left is to verify if the current user is the owner 
            // of the account
            return IsBlogOwner(username, address);
        }


        private bool IsBlogOwner(string username, string address)
        {
            // TODO: query the backend to perform the necessary verifications
            SlickBlogAppContext _db = new SlickBlogAppContext();
            try
            {
                Blog blog = _db.Blogs.Single(b => b.Address == address);
                if (blog.Owner.Username.Equals(username))
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            
            return false;

        }
    }
}