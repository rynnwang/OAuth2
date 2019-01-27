using System.Net;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public static class OAuth2Extension
    {
        /// <summary>
        /// Fills the authentication.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="oauthResult">The oauth result.</param>
        public static void FillAuthentication(this HttpWebRequest httpRequest, OAuth2AuthenticationResult oauthResult)
        {
            if (httpRequest != null && oauthResult != null && !string.IsNullOrWhiteSpace(oauthResult.AccessToken))
            {
                httpRequest.Headers.Set(HttpRequestHeader.Authorization, string.Format("{0} {1}", oauthResult.TokenType, oauthResult.AccessToken));
            }
        }
    }
}