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
using System.IO;
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
            disComments.Comments = post.Comments.OrderByDescending(c => c.PostDate).Skip(page * 5).Take(5);
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

        public ActionResult DeleteComment(String address, int id)
        {
            Comment comment = _db.Comments.Find(id);
            String username = User.Identity.Name;
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            if (username.Equals(comment.Author.Username) || username.Equals(blog.Owner.Username))
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [AuthorizeUser]
        public ActionResult Comments(String address, int? id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            int pageSize = 3;

            int pages = 1;

            int commCount = _db.Comments.Include("Author").Include("Post").Where(c => c.Post.Blog.Address.Equals(address)).Count();
            pages = commCount / pageSize;
            Mapper.CreateMap<Blog, CpComments>().ForMember(m => m.Comments, opt => opt.Ignore());
            CpComments cposts = Mapper.Map<Blog, CpComments>(blog);
            cposts.CurrentPage = id ?? 0;
            cposts.Pages = pages;
            //cposts.Comments = comments;
            cposts.Comments = PaginationHelper.getPagedComments(blog, id ?? 0, pageSize);
            return View(cposts);

        }
        public ActionResult Index(String address, int? id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            int pageSize = 3;

            int pages = blog.Posts.Where(p => p.Published == true).Count() / pageSize;

            Mapper.CreateMap<Blog, CpPosts>().ForMember(m => m.Posts, opt => opt.Ignore());
            CpPosts cposts = Mapper.Map<Blog, CpPosts>(blog);
            cposts.CurrentPage = id ?? 0;
            cposts.Pages = pages + 1;
            cposts.Scope = "Index";
            cposts.Posts = PaginationHelper.getPagedPosts(blog, id ?? 0, pageSize, "published");
            return View(cposts);
        }
        [AuthorizeUser]
        public ActionResult Home(String address)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists

            return View(blog);

        }
        [AuthorizeUser]
        public ActionResult Settings(String address)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists

            return View(blog);

        }

        [AuthorizeUser]
        public ActionResult PostEditor(String address, int? id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists

            if (id != null)
            {
                Post post = blog.Posts.Single(p => p.PostId == id);//needs check if post exists
                Mapper.CreateMap<Post, EditPost>().ForMember(f => f.file, opt => opt.Ignore()); ;
                EditPost ep = Mapper.Map<Post, EditPost>(post);
                //byte[] fileData;               
                //string contentType;

                string fileName;

                if (post.File != null)
                {
                    //fileData = post.File;                  
                    //contentType = post.FileContentType;

                    fileName = post.FileName;

                    //ep.FileData = fileData;
                    //ep.FileContentType = fileName;

                    ep.FileName = fileName;
                }
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
        [AuthorizeUser]
        public ActionResult Posts(String address, String id, int? page)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);//needs check if blog exists
            int pageSize = 3;

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
            pages++;
            Mapper.CreateMap<Blog, CpPosts>().ForMember(m => m.Posts, opt => opt.Ignore());
            CpPosts cposts = Mapper.Map<Blog, CpPosts>(blog);
            cposts.CurrentPage = page ?? 0;
            cposts.Scope = id;
            cposts.Pages = pages; //dont know why there was +1
            cposts.Posts = PaginationHelper.getPagedPosts(blog, page ?? 0, pageSize, id);
            return View(cposts);

        }

        [AuthorizeUser]
        [HttpPost]
        public ActionResult PublishPost(EditPost p)
        {
            Blog blog = _db.Blogs.Find(p.BlogId);

            if (ModelState.IsValid)
            {
                //anti xss
                //p.Content = Sanitizer.GetSafeHtmlFragment(p.Content);
                
                byte[] fileData = null;
                string contentType = null;
                string fileName = null;

                if (p.PostId != 0)
                {
                    Post pst = _db.Posts.Find(p.PostId);
                    fileData = pst.File;
                    contentType = pst.FileContentType;
                    fileName = pst.FileName;
                    pst = null;
                    _db = new SlickBlogAppContext();
                }
                Mapper.CreateMap<EditPost, Post>().ForMember(f => f.File, opt => opt.Ignore());
                Post post = Mapper.Map<EditPost, Post>(p);
                
                Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                
                if (p.file != null)
                {
                    if (p.file.ContentLength > 0)
                    {
                        Stream s = p.file.InputStream;
                        byte[] appData = new byte[p.file.ContentLength + 1];
                        s.Read(appData, 0, p.file.ContentLength);
                        post.File = appData;
                        post.FileName = p.file.FileName;
                        post.FileContentType = p.file.ContentType;
                    }
                }
                else if (p.PostId!=0&&p.FileName.Equals(fileName))
                {
                    post.File = fileData;
                    post.FileName = fileName;
                    post.FileContentType = contentType;
                }
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
        [AuthorizeUser]
        [HttpPost]
        public ActionResult SavePost(EditPost p)
        {
            Blog blog = _db.Blogs.Find(p.BlogId);

            if (ModelState.IsValid)
            {
                //anti xss
                //p.Content = Sanitizer.GetSafeHtmlFragment(p.Content);
                byte[] fileData = null;
                string contentType = null;
                string fileName = null;

                if (p.PostId != 0)
                {
                    Post pst = _db.Posts.Find(p.PostId);
                    fileData = pst.File;
                    contentType = pst.FileContentType;
                    fileName = pst.FileName;
                    pst = null;
                    _db = new SlickBlogAppContext();
                }
                Mapper.CreateMap<EditPost, Post>().ForMember(f => f.File, opt => opt.Ignore());
                Post post = Mapper.Map<EditPost, Post>(p);
                Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                if (p.file != null)
                {
                    if (p.file.ContentLength > 0)
                    {
                        Stream s = p.file.InputStream;
                        byte[] appData = new byte[p.file.ContentLength + 1];
                        s.Read(appData, 0, p.file.ContentLength);
                        post.File = appData;
                        post.FileName = p.file.FileName;
                        post.FileContentType = p.file.ContentType;
                    }
                }
                else if (p.PostId != 0 && p.FileName.Equals(fileName))
                {
                    post.File = fileData;
                    post.FileName = fileName;
                    post.FileContentType = contentType;
                }
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
        public FileContentResult FileDownload(int id)
        {
            //declare byte array to get file content from database and string to store file name
            byte[] fileData;
            string fileName;
            string contentType;

            Post p = _db.Posts.Find(id);
            if (p.File != null)
            {
                fileData = p.File;
                fileName = p.FileName;
                contentType = p.FileContentType;
                //return file and provide byte file content and file name
                return File(fileData, contentType, fileName);
            }
            return null;
        }
        [AuthorizeUser]
        public ActionResult DeletePost(String address, int id)
        {
            Blog blog = _db.Blogs.Single(b => b.Address == address);

            Post post = blog.Posts.Single(p => p.PostId == id);
            IEnumerable<Comment> comments = _db.Comments.Where(c => c.Post.PostId == id);
            foreach (var item in comments)
            {
                _db.Comments.Remove(item);
            }
            _db.Posts.Remove(post);
            _db.SaveChanges();
            return RedirectToAction("/Posts/all");

        }
    }
}
