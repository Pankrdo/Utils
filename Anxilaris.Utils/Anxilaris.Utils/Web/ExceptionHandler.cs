namespace Anxilaris.Utils.Web.Mvc
{    
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class ExceptionHandler<T> where T:Controller,new()
    {
        public static void Process(HttpApplication context)
        {
            Exception exception = context.Server.GetLastError();

            HttpException httpException = exception as HttpException;
            if (httpException == null)
                httpException = new HttpException(500, "Internal Server Error", exception);

            context.Response.Clear();
            context.Server.ClearError();

            Controller controller = new T();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 403:
                    routeData.Values.Add("action", "AccessDenied");
                    break;

                case 404:
                    routeData.Values.Add("action", "NotFound");
                    break;

                case 500:
                    routeData.Values.Add("action", "ServerError");
                    break;

                default:
                    routeData.Values.Add("action", "OtherHttpStatusCode");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    break;
            }

            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(context.Context), routeData));
        }
    }
}