using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SlickBlogApp.Models;
using SlickBlogApp.ViewModels;

namespace SlickBlogApp.Helpers
{
    public static class PaginationHelper
    {
        public static MvcHtmlString PostPaging(this HtmlHelper helper, CpPosts cpost)
        {
            StringBuilder sb = new StringBuilder();

            String path;
            if(cpost.Scope.Equals("Index")){
                path = VirtualPathUtility.ToAbsolute("~/Blog/" + cpost.Address + "/Index/");
            }
            else{
                path = VirtualPathUtility.ToAbsolute("~/Blog/" + cpost.Address + "/Posts/" + cpost.Scope +"/");
            }

            if (cpost.CurrentPage == 0)
                cpost.CurrentPage = 1;
            sb.Append("<div class=\"pagination\">\n");
            sb.Append("<ul>\n");

            sb.Append("<li");
            if (cpost.CurrentPage <= 1)
            {
                sb.Append(" class=\"disabled\" ><a href=\"#\">Prev</a></li>\n");
            }
            else
                sb.Append("><a href=\"" + path + (cpost.CurrentPage - 1) + "\">Prev</a></li>\n");

            for (int i = 1; i <= cpost.Pages; i++)
            {
                if (cpost.CurrentPage == i)
                {
                    sb.Append("<li class=\"active\"><a href=\""+path+i+"\">" + i + "</a></li>\n");
                }
                else
                {
                    sb.Append("<li><a href=\"" + path + i + "\">" + i + "</a></li>\n");
                }
                
            }


            sb.Append("<li");
            if (cpost.CurrentPage >= cpost.Pages)
            {
                sb.Append(" class=\"disabled\" ><a href=\"#\">Next</a></li>\n");
            }
            else
                sb.Append("><a href=\"" + path + (cpost.CurrentPage + 1) + "\">Next</a></li>\n");
            

            sb.Append("</ul>\n");
            sb.Append("</div>\n");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static IEnumerable<Post> getPagedPosts(Blog blog, int skip, int take, string scope){
            bool publish;
            if (skip != 0)
            {
                skip--;
            }
            switch (scope)
            {
                case "published":
                    {
                        publish = true;
                        break;
                    }
                case "draft":
                    {
                        publish = false;
                        break;
                    }
                default:
                    {
                        return blog.Posts.OrderByDescending(p => p.PostDate).Skip(skip * take).Take(take);
                    }
            }
            return blog.Posts.Where(p => p.Published == publish).OrderByDescending(p => p.PostDate).Skip(skip * take).Take(take);
        }
        public static MvcHtmlString PostCount(this HtmlHelper helper, int blogid)
        {
            SlickBlogAppContext _db = new SlickBlogAppContext();
            Blog blog = _db.Blogs.Find(blogid);
            int count = 0;
            if (blog.Posts != null)
            {
                count = blog.Posts.Count();
            }
            return MvcHtmlString.Create(count.ToString());
        }
        public static MvcHtmlString TotalCommentCount(this HtmlHelper helper, int blogid)
        {
            SlickBlogAppContext _db = new SlickBlogAppContext();
            //Blog blog = _db.Blogs.Find(blogid);
            _db.Comments.Where(c=>c.Post.Blog.BlogId==blogid).Count();
            int count = 0;
            //if (blog.Posts != null)
            //{
            count = _db.Comments.Where(c => c.Post.Blog.BlogId == blogid).Count();
            //}
            return MvcHtmlString.Create(count.ToString());
        }
        public static MvcHtmlString PostCommentCount(this HtmlHelper helper, int postid)
        {
            SlickBlogAppContext _db = new SlickBlogAppContext();
            //Blog blog = _db.Blogs.Find(blogid);
            _db.Comments.Where(c => c.Post.PostId == postid).Count();
            int count = 0;
            //if (blog.Posts != null)
            //{
            count = _db.Comments.Where(c => c.Post.Blog.BlogId == postid).Count();
            //}
            return MvcHtmlString.Create(count.ToString());
        }
        //public static MvcHtmlString PostPaging(this HtmlHelper helper, CpPosts cpost)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    String path;
        //    if(cpost.Scope.Equals("Index")){
        //        path = VirtualPathUtility.ToAbsolute("~/Blog/" + cpost.Address + "/Index/");
        //    }
        //    else{
        //        path = VirtualPathUtility.ToAbsolute("~/Blog/" + cpost.Address + "/Posts/" + cpost.Scope +"/");
        //    }

        //    if (cpost.CurrentPage == 0)
        //        cpost.CurrentPage = 1;
        //    sb.Append("<div class=\"pagination\">\n");
        //    sb.Append("<ul>\n");

        //    sb.Append("<li");
        //    if (cpost.CurrentPage <= 1)
        //    {
        //        sb.Append(" class=\"disabled\" ><a href=\"#\">Prev</a></li>\n");
        //    }
        //    else
        //        sb.Append("><a href=\"" + path + (cpost.CurrentPage - 1) + "\">Prev</a></li>\n");

        //    for (int i = 1; i <= cpost.Pages; i++)
        //    {
        //        if (cpost.CurrentPage == i)
        //        {
        //            sb.Append("<li class=\"active\"><a href=\""+path+i+"\">" + i + "</a></li>\n");
        //        }
        //        else
        //        {
        //            sb.Append("<li><a href=\"" + path + i + "\">" + i + "</a></li>\n");
        //        }
                
        //    }


        //    sb.Append("<li");
        //    if (cpost.CurrentPage >= cpost.Pages)
        //    {
        //        sb.Append(" class=\"disabled\" ><a href=\"#\">Next</a></li>\n");
        //    }
        //    else
        //        sb.Append("><a href=\"" + path + (cpost.CurrentPage + 1) + "\">Next</a></li>\n");
            

        //    sb.Append("</ul>\n");
        //    sb.Append("</div>\n");
        //    return MvcHtmlString.Create(sb.ToString());
        //}
        public static MvcHtmlString CommentPaging(this HtmlHelper helper, CpComments ccomm)
        {
            StringBuilder sb = new StringBuilder();

            String path;
            
            path = VirtualPathUtility.ToAbsolute("~/Blog/" + ccomm.Address + "/Comments/");


            if (ccomm.CurrentPage == 0)
                ccomm.CurrentPage = 1;
            sb.Append("<div class=\"pagination\">\n");
            sb.Append("<ul>\n");

            sb.Append("<li");
            if (ccomm.CurrentPage <= 1)
            {
                sb.Append(" class=\"disabled\" ><a href=\"#\">Prev</a></li>\n");
            }
            else
                sb.Append("><a href=\"" + path + (ccomm.CurrentPage - 1) + "\">Prev</a></li>\n");

            for (int i = 1; i <= ccomm.Pages; i++)
            {
                if (ccomm.CurrentPage == i)
                {
                    sb.Append("<li class=\"active\"><a href=\"" + path + i + "\">" + i + "</a></li>\n");
                }
                else
                {
                    sb.Append("<li><a href=\"" + path + i + "\">" + i + "</a></li>\n");
                }

            }


            sb.Append("<li");
            if (ccomm.CurrentPage >= ccomm.Pages)
            {
                sb.Append(" class=\"disabled\" ><a href=\"#\">Next</a></li>\n");
            }
            else
                sb.Append("><a href=\"" + path + (ccomm.CurrentPage + 1) + "\">Next</a></li>\n");


            sb.Append("</ul>\n");
            sb.Append("</div>\n");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static IEnumerable<Comment> getPagedComments(Blog blog, int skip, int take)
        {
            if (skip != 0)
            {
                skip--;
            }
            SlickBlogAppContext _db = new SlickBlogAppContext();
            IEnumerable<Comment> trl =  _db.Comments.Include("Author").Include("Post").Where(c => c.Post.Blog.Address.Equals(blog.Address)).OrderByDescending(c => c.PostDate).Skip(skip * take).Take(take);
            return trl;
        }
    }
    
}