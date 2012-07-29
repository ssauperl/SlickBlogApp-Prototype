using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SlickBlogApp.Models;
using SlickBlogApp.Helpers;
using SlickBlogApp.ViewModels;
using AutoMapper;
using System.Data;
//using Microsoft.Security.Application;

namespace SlickBlogApp.Controllers
{
    public class BlogController : Controller
    {
        SlickBlogAppContext _db = new SlickBlogAppContext();

        public ActionResult Details(String address, int id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            Mapper.CreateMap<Blog, DisPost>();
            DisPost dpost = Mapper.Map<Blog, DisPost>(blog);
            dpost.Post = blog.Posts.Single(p => p.PostId == id);
            return View(dpost);
        }
        public PartialViewResult MoreComments(int id, int page)
        {
            Post post = _db.Posts.Find(id);
            NewComment comment = new NewComment();
            comment.PostId = post.PostId;
            comment.Address = post.Blog.Address;
            DisComments disComments = new DisComments();
            disComments.NewComment = comment;
            disComments.Comments = post.Comments.OrderByDescending(c => c.PostDate).Skip(page*5).Take(5);
            return PartialView("_CommentsMore", disComments);
        }
        public PartialViewResult PostComments(int id)
        {
            Post post = _db.Posts.Find(id);
            NewComment comment = new NewComment();
            comment.PostId = post.PostId;
            comment.Address = post.Blog.Address;
            DisComments disComments = new DisComments();
            disComments.NewComment = comment;
            disComments.Comments = post.Comments.OrderByDescending(c => c.PostDate).Take(5);
            return PartialView("_Comments", disComments);
        }
        [Authorize]
        [HttpPost]
        public ActionResult PublishComment(NewComment c)
        {
            if (ModelState.IsValid)
            {
                Comment comment = new Comment();
                Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                comment.Author = userInfo;
                comment.Post = _db.Posts.Find(c.PostId);
                comment.Text = c.Text;
                comment.PostDate = DateTime.Now;
                _db.Comments.Add(comment);
                _db.SaveChanges();
            }
            Post post = _db.Posts.Find(c.PostId);
            NewComment newComment = new NewComment();
            newComment.PostId = post.PostId;
            newComment.Address = post.Blog.Address;
            DisComments disComments = new DisComments();
            disComments.NewComment = newComment;
            disComments.Comments = post.Comments.OrderByDescending(x => x.PostDate).Take(5);
            return PartialView("_Comments", disComments);
        }
        public ActionResult Index(String address, int? id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            int pageSize = 3;
            
                int pages = blog.Posts.Where(p => p.Published == true).Count() / pageSize;
                        
                Mapper.CreateMap<Blog, CpPosts>().ForMember(m=>m.Posts, opt=>opt.Ignore());
                CpPosts cposts = Mapper.Map<Blog, CpPosts>(blog);
                cposts.CurrentPage = id ?? 0;
                cposts.Pages = pages+1;
                cposts.Scope = "Index";
                cposts.Posts = PaginationHelper.getPagedPosts(blog, id ?? 0, pageSize, "published"); 
                return View(cposts);
        }
        [Authorize]
        public ActionResult Home(String address)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            if(AuthorizeHelper.Authorize(_db, blog)){
            return View(blog);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public ActionResult Settings(String address)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            if (AuthorizeHelper.Authorize(_db, blog))
            {
                return View(blog);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //[Authorize]
        //public ActionResult PostEditor(String address)
        //{
        //    Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
        //    if (AuthorizeHelper.Authorize(_db, blog))
        //    {
        //        EditPost post = new EditPost();
        //        post.BlogId = blog.BlogId;
        //        post.Address = blog.Address;
        //        return View(post);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        [Authorize]
        public ActionResult PostEditor(String address, int? id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            if (AuthorizeHelper.Authorize(_db, blog))
            {
                if (id != null)
                {
                    Post post = blog.Posts.Single(p => p.PostId == id);//needs check if post exists
                    Mapper.CreateMap<Post, EditPost>();
                    EditPost ep = Mapper.Map<Post, EditPost>(post);
                    ep.PostId = post.PostId;
                    ep.BlogId = blog.BlogId;
                    ep.Address = address;
                    return View(ep);
                }
                else
                {
                    EditPost post = new EditPost();
                    post.BlogId = blog.BlogId;
                    post.Address = blog.Address;
                    return View(post);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public ActionResult Posts(String address, String id, int? page)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            int pageSize = 3;
            if (AuthorizeHelper.Authorize(_db, blog))
            {
                int pages;
                switch (id)
                {
                    case "published":
                        {
                            pages = blog.Posts.Where(p => p.Published == true).Count() / pageSize;
                            break;
                        }
                    case "draft":
                        {
                            pages = blog.Posts.Where(p => p.Published == true).Count() / pageSize;
                            break;
                        }
                    default:
                        {
                            pages = blog.Posts.Count() / pageSize;
                            break;
                        }
                }
                Mapper.CreateMap<Blog, CpPosts>().ForMember(m=>m.Posts, opt=>opt.Ignore());
                CpPosts cposts = Mapper.Map<Blog, CpPosts>(blog);
                cposts.CurrentPage = page ?? 0;
                cposts.Scope = id;
                cposts.Pages = pages+1;
                cposts.Posts = PaginationHelper.getPagedPosts(blog, page ?? 0, pageSize, id); 
                return View(cposts);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //[Authorize]
        //public ActionResult ShortPosts(String address)
        //{
        //    Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
        //    if (AuthorizeHelper.Authorize(_db, blog))
        //    {
        //        IEnumerable<Post> posts = blog.Posts.OrderByDescending(p=>p.PostDate);
        //        return PartialView("_Posts", posts);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        //[Authorize]
        //public ActionResult CreatePost(String address)
        //{
        //    Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
        //    if (AuthorizeHelper.Authorize(_db, blog))
        //    {
        //        EditPost post = new EditPost();

        //        post.BlogId = blog.BlogId;
        //        post.Address = blog.Address;

        //        return PartialView("_CreatePost", post);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
            
        //}
        [Authorize]
        [HttpPost]
        public ActionResult PublishPost(EditPost p)
        {
            Blog blog = _db.Blogs.Find(p.BlogId);
            if (AuthorizeHelper.Authorize(_db, blog))
            {
                if (ModelState.IsValid)
                {
                    //anti xss
                    //p.Content = Sanitizer.GetSafeHtmlFragment(p.Content);
                    Mapper.CreateMap<EditPost, Post>();
                    Post post = Mapper.Map<EditPost, Post>(p);
                    Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                    UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                    post.Author = userInfo;
                    post.Blog = _db.Blogs.Find(p.BlogId);
                    post.Published = true;
                    post.PostDate = DateTime.Now;
                    if (p.PostId != 0)
                    {
                        _db.Entry(post).State = EntityState.Modified;
                    }
                    else
                    {
                        _db.Posts.Add(post);
                    }
                    _db.SaveChanges();
                    return RedirectToAction("/Details/" + post.PostId);
                }

                return RedirectToAction("/Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult SavePost(EditPost p)
        {
            Blog blog = _db.Blogs.Find(p.BlogId);
            if (AuthorizeHelper.Authorize(_db, blog))
            {
                if (ModelState.IsValid)
                {
                    //anti xss
                    //p.Content = Sanitizer.GetSafeHtmlFragment(p.Content);
                    Mapper.CreateMap<EditPost, Post>();
                    Post post = Mapper.Map<EditPost, Post>(p);
                    Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                    UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                    post.Author = userInfo;
                    post.Blog = _db.Blogs.Find(p.BlogId);
                    post.Published = false;
                    post.PostDate = DateTime.Now;
                    if (p.PostId != 0)
                    {
                        _db.Entry(post).State = EntityState.Modified;
                    }
                    else
                    {
                        _db.Posts.Add(post);
                    }
                    _db.SaveChanges();
                    //Mapper.CreateMap<Post, EditPost>();
                    //EditPost ep = Mapper.Map<Post, EditPost>(post);
                    //ep.PostId = post.PostId;
                    //ep.BlogId = p.BlogId;
                    //ep.Address = p.Address;
                    return Content(post.PostId.ToString()); 
                }
                return Content("0"); 
                
            }
            return Content("0"); 
        }
        [Authorize]
        public ActionResult DeletePost(String address, int id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);
            if (AuthorizeHelper.Authorize(_db, blog))
            {
                Post post = blog.Posts.Single(p => p.PostId == id);
                IEnumerable<Comment> comments = _db.Comments.Where(c => c.Post.PostId == id);//not the best way
                foreach (var item in comments)
                {
                    _db.Comments.Remove(item);
                }
                _db.Posts.Remove(post);
                _db.SaveChanges();
                return RedirectToAction("/Posts/all");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
