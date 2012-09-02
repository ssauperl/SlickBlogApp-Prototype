using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickBlogApp.Helpers;
using SlickBlogApp.Models;

namespace SlickBlogApp.ViewModels
{
    public class CreateBlogModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [StringLength(100, MinimumLength=3)]
        public string Title { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [UniqueAddress]
        [Remote("IsAddress_Available", "Validation")]
        //[RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed.")]
        [RegularExpression(@"^(?:\w+\s?)+\w+$", ErrorMessage = "Address can contain only letters and numbers.")]
        [StringLength(30, MinimumLength = 3)]
        public String Address { get; set; }

    }
    public class EditPost
    {
        [HiddenInput]
        public int PostId { get; set; }
        [HiddenInput]
        public int BlogId { get; set; }
        [HiddenInput]
        public String Address { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
        public HttpPostedFileBase file { get; set; }
        //public byte[] FileData { get; set; }
        public string FileName { get; set; }
        //public string FileContentType { get; set; }
        public string Tags { get; set; }
    }
    public class CpPosts
    {
        [HiddenInput]
        public int BlogId { get; set; }
        [HiddenInput]
        public String Address { get; set; }
        [HiddenInput]
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string Scope { get; set; }
        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
    public class CpComments
    {
        [HiddenInput]
        public int BlogId { get; set; }
        [HiddenInput]
        public String Address { get; set; }
        [HiddenInput]
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string Scope { get; set; }
        public string Title { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
    public class NewComment
    {
        [HiddenInput]
        public int BlogId { get; set; }
        [HiddenInput]
        public String Address { get; set; }
        [HiddenInput]
        public int PostId { get; set; }
        [Required]
        public string Text { get; set; }
    }
    public class DisComments
    {
        public NewComment NewComment { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
    public class DisPost
    {
        [HiddenInput]
        public int BlogId { get; set; }
        [HiddenInput]
        public String Address { get; set; }
        [HiddenInput]
        public string Title { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}