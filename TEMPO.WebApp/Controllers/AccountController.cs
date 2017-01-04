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
        private const string ROLE_SEPARATOR = ",";

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

                List<string> roles = employee.modules.Select(i => i.modulename).ToList();

                FormsAuthentication.SetAuthCookie(employee.employeename, false);

                SetAuthenticationCookie(employee, roles);
                SetUserIdCookie(employee);

                return RedirectToAction("Index", "Home");
            }
            else
            {                
                ModelState.AddModelError("", "UserName or Password is wrong");
            }
            return View();
        }

        private void SetAuthenticationCookie(Employee employee, List<string> roles)
        {
            var authTicket = new FormsAuthenticationTicket(
                                1,
                                employee.employeename,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(20),
                                false,
                                string.Join(ROLE_SEPARATOR, roles));

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
        }

        private void SetUserIdCookie(Employee employee)
        {
            HttpCookie userIdCookie = new HttpCookie(USERID_COOKIE_NAME);
            userIdCookie.Value = employee.empid.ToString();
            userIdCookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(userIdCookie);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}