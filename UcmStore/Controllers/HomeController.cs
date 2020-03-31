using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Configuration;
using DataLibrary;




namespace UcmStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
           //IF user logged in, redirect to shopping controller's index, ELSE return  view like normal
           //so..after logging in, you are not able to re log in
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            bool verifiedStatus = Users.login(email, password);
            if (verifiedStatus == true)
            {
                Session["email_ID"] = email;
                return RedirectToAction("Index", "Shopping");
            }
            else
            {
                return RedirectToAction("Login");
            }
            //return View(); //redirect to some index page
        }

        public ActionResult Logout()
        {
            //close session, cookies, etc
            return RedirectToAction("Shopping","Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "About page";

            return View();
        }

    }
}