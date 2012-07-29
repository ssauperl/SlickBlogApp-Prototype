using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlickBlogApp.Models
{
    public class UserInfo
    {
        public int UserInfoId { get; set; }
        public string Username { get; set; }
        public Guid UserGuid { get; set; }
        public virtual ICollection<Blog> AuthBlogs { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}