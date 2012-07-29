using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlickBlogApp.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string TagName { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}