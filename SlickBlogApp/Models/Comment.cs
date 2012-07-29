using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlickBlogApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        public virtual Post Post { get; set; }
        public virtual UserInfo Author { get; set; }
        
    }
}