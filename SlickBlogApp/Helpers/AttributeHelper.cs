using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SlickBlogApp.Models;
namespace SlickBlogApp.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAddress : ValidationAttribute
    {
        SlickBlogAppContext _db = new SlickBlogAppContext();
        public override Boolean IsValid(Object value)
        {
            // constraint implemented on database
            string testValue = value.ToString();
            if (_db.Blogs.Any(b => b.Address == testValue))
            {
                return false;
            }
            return true;
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext,
    string actionName, MethodInfo methodInfo)
        {
            bool isValidName = false;
            string keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);
            if (value != null)
            {
                value = new ValueProviderResult(Argument, Argument, null);
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
}