using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salestech.Exam.WebMVC.Models
{
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var users = TG.Exam.WebMVC.Models.User.GetAll()
                .Select(u => new UserViewModel
                {
                    Age = u.Age,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Method = "Sync"
                });
            return View(users);
        }

        [HttpGet]
        public ActionResult IndexUsers()
        {
            var users = TG.Exam.WebMVC.Models.User.GetAll()
                .Select(u => new UserViewModel
                {
                    Age = u.Age + 10,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Method = "Async"
                }).ToList();
            return View("Index", users);
        }
    }
}
