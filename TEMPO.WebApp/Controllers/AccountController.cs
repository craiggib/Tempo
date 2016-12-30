using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TEMPO.BusinessLayer;
using TEMPO.Data;
using TEMPO.WebApp.Models.Account;

namespace TEMPO.WebApp.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            AccountManager account = new AccountManager();
            Employee employee = account.Login(user.EmployeeName, user.Password);
            if(employee != null)
            {
                
                FormsAuthentication.SetAuthCookie(employee.employeename, true);
                SetUserId(employee);
                return RedirectToAction("Index", "Home");
            }
            else
            {                
                ModelState.AddModelError("", "UserName or Password is wrong");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}