using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_try3.Controllers
{
    public class LoginCheck:ActionFilterAttribute
    {
        public bool flag { get; set; }
        public short id = 2;
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
            if (flag)//到需要為flase的action上寫[LoginCheck(flag=false)]
            {
                //base.OnActionExecuting(filterContext);
                HttpContext context = HttpContext.Current;
                LoginState(context);
            }
        }

    }
}