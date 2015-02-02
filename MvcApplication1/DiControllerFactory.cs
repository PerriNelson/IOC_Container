using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IOC_Container;

namespace MvcApplication1
{
    public class DiControllerFactory : DefaultControllerFactory
    {
        private InversionOfControlContainer iocContainer;
        public DiControllerFactory()
        {
            iocContainer = new InversionOfControlContainer();
            iocContainer.Register<Interface1, Class1>();
            iocContainer.Register<Controller, Controllers.MyController>();
        }
   
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (controllerName.Equals("My"))
                return iocContainer.Resolve<Controller>();

            return base.CreateController(requestContext, controllerName);
        }
    }
}