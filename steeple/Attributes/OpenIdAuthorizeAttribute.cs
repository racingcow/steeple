using System.Web.Mvc;
using System.Web;

namespace steeple.Attributes
{
    public class OpenIdAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

#if (!DEBUG)
{	
            if (!httpContext.User.Identity.IsAuthenticated)
            {                
                httpContext.Response.Redirect(string.Format("~/User/Login?ReturnUrl=~{0}", httpContext.Request.Url.PathAndQuery));
                return false;
            }
}
#endif

            return true;
        }
    }
}