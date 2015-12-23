using AuthenticationApp03.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthenticationApp03.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;
        public ToDoController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            try
            {
                return View(db.ToDoes.ToList().Where(todo => todo.User.Id == currentUser.Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ToDo todo)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                try
                {
                    todo.User = currentUser;
                    db.ToDoes.Add(todo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return View();
                }
            }
            return View(todo);
        }
    }
}