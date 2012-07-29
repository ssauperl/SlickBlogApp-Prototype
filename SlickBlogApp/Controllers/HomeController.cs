using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using SlickBlogApp.Models;
using SlickBlogApp.ViewModels;

namespace SlickBlogApp.Controllers
{
    public class HomeController : Controller
    {

        SlickBlogAppContext _db = new SlickBlogAppContext();

        public ActionResult Index()
        {
            ViewBag.Message = "My blogs";
            return View();
        }
        public ActionResult UserBlogs()
        {
            IEnumerable<Blog> blogs = new List<Blog>();
            if (User.Identity.IsAuthenticated)
            {
                Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                blogs = _db.Blogs.Where(b => b.Owner.UserInfoId == userInfo.UserInfoId);
            }
            return PartialView("_UserBlogs", blogs);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateBlog(CreateBlogModel cblog)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<CreateBlogModel, Blog>();
                Blog blog = Mapper.Map<CreateBlogModel, Blog>(cblog);

                //check if blog adress is unique
                if (_db.Blogs.Any(b => b.Address == blog.Address))
                {
                    return Content("Address not unique");
                }
                else
                {

                    Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                    UserInfo userInfo = _db.UserInfo.Single(ui => ui.UserGuid == userGuid);
                    blog.Owner = userInfo;
                    List<UserInfo> authors = new List<UserInfo>();
                    authors.Add(userInfo);
                    blog.Authors = authors;
                    _db.Blogs.Add(blog);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(cblog);
        }

    }
}
