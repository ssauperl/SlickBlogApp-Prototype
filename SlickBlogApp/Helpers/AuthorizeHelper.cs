using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SlickBlogApp.Models;
namespace SlickBlogApp.Helpers
{
    public class AuthorizeHelper
    {
        public static bool Authorize(SlickBlogAppContext _db, Blog blog)
        {
            Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
            UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
            if (blog.Owner.Equals(userInfo))
            {
                return true;
            }
            return false;
        }
    }
}