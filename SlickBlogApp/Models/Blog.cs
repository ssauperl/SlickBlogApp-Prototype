using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlickBlogApp.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [StringLength(30, MinimumLength = 3)]
        public String Address { get; set; }
        //public virtual ICollection<Tag> Tags { get; set; }
        //[ForeignKey("Owner")]
        //public int Owner_UserInfoId { get; set; }
        [Required]
        public virtual UserInfo Owner { get; set; }
        public virtual ICollection<UserInfo> Authors { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        //[DataType(DataType.MultilineText)]
        //[AllowHtml]
        //public string CSS { get; set; }
        //[DataType(DataType.MultilineText)]
        //[AllowHtml]
        //public string HTML { get; set; }

    }
}