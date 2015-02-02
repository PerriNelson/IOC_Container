using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class MyController : Controller
    {
        private Interface1 _interface1;
        public MyController(Interface1 interface1)
        {
            _interface1 = interface1;
        }

        public ActionResult Index()
        {
            return new ViewResult()
            {
                ViewName = _interface1.getViewName()
            };
        }

    }
}
