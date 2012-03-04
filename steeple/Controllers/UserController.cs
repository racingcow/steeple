using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace prayerapp.Controllers
{
    public class UserController : Controller
    {

        private static readonly OpenIdRelyingParty m_openid = new OpenIdRelyingParty();

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Home");
        }

        public ActionResult Login()
        {
            // Stage 1: display login form to user
            return View("Login");
        }

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            var response = m_openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        IAuthenticationRequest request = m_openid.CreateRequest(Request.Form["openid_identifier"]);
                        request.AddExtension(new ClaimsRequest()
                        {
                            Email = DemandLevel.Require,
                            FullName = DemandLevel.Request
                        });                        

                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewData["Message"] = ex.Message;
                        return View("Login");
                    }
                }
                ViewData["Message"] = "Invalid identifier";
                return View("Login");
            }
            // Stage 3: OpenID Provider sending assertion response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    var claimsResponse = response.GetExtension<ClaimsResponse>();                    
                    Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;
                    //Session["FriendlyIdentifier"] = claimsResponse.Nickname ?? claimsResponse.Email;
                    FormsAuthentication.SetAuthCookie(response.ClaimedIdentifier, false);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                case AuthenticationStatus.Canceled:
                    ViewData["Message"] = "Canceled at provider";
                    return View("Login");
                case AuthenticationStatus.Failed:
                    ViewData["Message"] = response.Exception.Message;
                    return View("Login");
            }
            return new EmptyResult();
        }
    }
}
