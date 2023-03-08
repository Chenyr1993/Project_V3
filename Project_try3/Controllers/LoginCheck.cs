using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_try3.Controllers
{
    public class LoginCheck:ActionFilterAttribute
    {
        void LoginState(HttpContext context) 
        {
            if (context.Session["user"] == null) 
            {
                context.Response.Redirect("/Login/Login");
            }
        }
        //執行動作之前會呼叫這個方法
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            HttpContext context=HttpContext.Current;
            LoginState(context);
        }

    }
}