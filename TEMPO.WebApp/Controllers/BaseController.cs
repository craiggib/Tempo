using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.Data;

namespace TEMPO.WebApp.Controllers
{
    public class BaseController : Controller
    {
        private const string USERID_COOKIE_NAME = "Tempo.UserId";

        protected int GetUserID()
        {
            HttpCookie myCookie = Request.Cookies[USERID_COOKIE_NAME];
            if(myCookie != null)
            {
                return int.Parse(myCookie.Value);
            }
            else
            {
                return -1;
            }
        }

        protected void SetUserId(Employee employee)
        {
            HttpCookie userIdCookie = new HttpCookie(USERID_COOKIE_NAME);
            userIdCookie.Value = employee.empid.ToString();
            userIdCookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(userIdCookie);
        }
    }
}