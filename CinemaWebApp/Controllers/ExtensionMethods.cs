using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Controllers
{
    public static class ExtensionMethods
    {
        private const string fstr = "FriendlyString";
        public const string UserCookie = "UserCookie";
        public static void GetViewData(this Controller controller)
        {
            controller.ViewData[fstr] = controller.HttpContext.Session.GetString(fstr);
            controller.ViewData["IsAdmin"] = controller.HttpContext.Session.GetInt32("IsAdmin") == 1;
        }
        public static void SetViewData(this Controller controller, User user)
        {
            controller.HttpContext.Session.SetString(fstr, $"Hi, {user.FirstName}");
            controller.HttpContext.Session.SetInt32("UserID", user.UserID);
            controller.GetViewData();
        }
        public static void SetUserCookie(this Controller controller, string item, IDataProtector protector)
        {
            controller.Response.Cookies.Append(UserCookie, protector.Protect(item),
                new CookieOptions { Expires = new DateTimeOffset(DateTime.Now.AddDays(14)) });
        }
        public static IDataProtector SetCookieProtector(this IDataProtectionProvider protectionProvider)
        {
            return protectionProvider.CreateProtector(UserCookie);
        }
        public static bool IsAdmin(this Controller controller, DataContext context)
        {
            int? is_admin = controller.HttpContext.Session.GetInt32("IsAdmin");

            if (is_admin == 1)
            {
                controller.GetViewData();
                return true;
            }

            int userid = controller.HttpContext.Session.GetInt32("UserID") ?? default;

            if (userid == default)
                return false;

            Admin admin = context.Admins.Find(userid);

            if (admin is null)
                return false;

            controller.HttpContext.Session.SetInt32("IsAdmin", 1);
            controller.GetViewData();
            return true;
        }
    }
}
