using System.Web.Mvc;

namespace Beyova.OAuth2.TestSite.Controllers
{
    public class OAuth2Controller : Controller
    {
        private static Cache.MemoryCacheContainer<string, OAuth2AuthenticationResult> authenticationResultCache = new Cache.MemoryCacheContainer<string, OAuth2AuthenticationResult>(new Cache.MemoryCacheContainerOptions<string> { Capacity = 500, ExpirationInSecond = 3600 * 12 });

        private static MicrosoftOAuth2Client client = new MicrosoftOAuth2Client(new MicrosoftOAuth2ClientOptions
        {
            ClientId = "0aaaf25a-dd86-44c2-96fc-e0b8afa638dc",
            ClientSecret = "ftrMZPQY566=?gtzgAY92(;",
            RedirectUri = "http://localhost:64963/oauth2/authorize/",
            TenantId = "common",
            ProviderOptions = OAuth2ProviderFactory.MicrosoftAzureGlobalProviderOptionsV2,
            RespondMode = "query"
        });

        // GET: OAuth2
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirects this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Redirect()
        {
            return Redirect(client.CreateRedirect(new OAuth2Request { Scope = "User.Read" }));
        }

        /// <summary>
        /// Authorizes the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="error">The error.</param>
        /// <param name="error_description">The error description.</param>
        /// <returns></returns>
        public ActionResult Authorize(string code = null, string error = null, string error_description = null)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                var result = authenticationResultCache.Get(code);
                if (result == null)
                {
                    result = client.AuthenticateByCode(new OAuth2AuthenticationRequest { Token = code });
                    authenticationResultCache.Update(code, result);
                }

                var profile = client.GetUserProfile(result);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (!string.IsNullOrWhiteSpace(error))
            {
                return Json(new { error, error_description }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}