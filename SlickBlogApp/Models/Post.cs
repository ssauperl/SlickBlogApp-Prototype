using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlickBlogApp.Models
{
    public class Post
    {
        public int PostId { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public bool Published { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual UserInfo Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        //public int Views { get; set; }
    }
}