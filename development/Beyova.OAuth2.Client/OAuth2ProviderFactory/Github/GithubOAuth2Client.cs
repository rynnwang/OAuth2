using Beyova.Diagnostic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using static Beyova.OAuth2.Constants;

namespace Beyova.OAuth2
{
    /// <summary>
    /// See: https://developer.github.com/apps/building-oauth-apps/authorizing-oauth-apps/
    /// </summary>
    public class GithubOAuth2Client : OAuth2Client<GithubOAuth2ClientOptions, OAuth2Request, GithubOAuth2ErrorMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Client" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public GithubOAuth2Client(GithubOAuth2ClientOptions options) : base(RefineOptions(options))
        {
        }

        /// <summary>
        /// Refines the options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        static GithubOAuth2ClientOptions RefineOptions(GithubOAuth2ClientOptions options)
        {
            if (options != null && options.ProviderOptions == null)
            {
                options.ProviderOptions = OAuth2ProviderFactory.GetGithubProviderOptions();
            }

            return options;
        }

        /// <summary>
        /// Converts the o auth2 authentication result.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        protected override OAuth2AuthenticationResult ConvertOAuth2AuthenticationResult(string body)
        {
            var webDictionary = body.ParseToDictonary('&', StringComparer.OrdinalIgnoreCase);
            var errorObject = webDictionary.ContainsKey(RFCKeys.AccessToken) ? JToken.FromObject(webDictionary) : null;
            return errorObject?.ToObject<OAuth2AuthenticationResult>();
        }

        /// <summary>
        /// Converts the error object.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        protected override GithubOAuth2ErrorMessage ConvertErrorObject(string body)
        {
            var webDictionary = body.ParseToDictonary('&');
            var errorObject = webDictionary.ContainsKey(RFCKeys.Error) ? JToken.FromObject(webDictionary) : null;
            return errorObject?.ToObject<GithubOAuth2ErrorMessage>();
        }

        /// <summary>
        /// Fills the authentication.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="oauthResult">The oauth result.</param>
        protected override void FillAuthentication(HttpWebRequest httpRequest, OAuth2AuthenticationResult oauthResult)
        {
            if (httpRequest != null && oauthResult != null && !string.IsNullOrWhiteSpace(oauthResult.AccessToken))
            {
                httpRequest.Headers.Set(HttpRequestHeader.Authorization, string.Format("{0} {1}", "token", oauthResult.AccessToken));
                httpRequest.UserAgent = this._options.AppName;
            }
        }

        /// <summary>
        /// Creates the redirect query string parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected override Dictionary<string, string> CreateRedirectQueryStringParameters(OAuth2Request request)
        {
            var result = base.CreateRedirectQueryStringParameters(request);

            return result;
        }

        /// <summary>
        /// Creates the redirect URI.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected override string CreateRedirectUri(OAuth2Request request)
        {
            return _options.ProviderOptions.AuthenticationUri;
        }

        /// <summary>
        /// Deserializes the o auth2 user profile.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        protected override OAuth2UserProfile DeserializeOAuth2UserProfile(JToken json)
        {
            if (json != null)
            {
                return new OAuth2UserProfile
                {
                    Email = json.Value<string>("mail"),
                    Id = json.Value<string>("id"),
                    Name = json.Value<string>("displayName"),
                    JobTitle = json.Value<string>("jobTitle"),
                    UserPrincipalName = json.Value<string>("userPrincipalName"),
                    CellphoneNumber = json.Value<string>("mobilePhone")
                };
            }

            return null;
        }
    }
}