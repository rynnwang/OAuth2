using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Beyova.OAuth2
{
    /// <summary>
    /// See: https://docs.microsoft.com/en-us/graph/auth-v2-user,
    /// https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-graph-api
    /// </summary>
    public class MicrosoftOAuth2Client : OAuth2Client<MicrosoftOAuth2ClientOptions, OAuth2Request, MicrosoftOAuth2ErrorMessage>
    {
        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public override string Domain { get { return "microsoftonline.com"; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Client" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MicrosoftOAuth2Client(MicrosoftOAuth2ClientOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets the default options.
        /// </summary>
        /// <returns></returns>
        protected override OAuth2ProviderOptions GetDefaultOptions()
        {
            return OAuth2ProviderFactory.GetMicrosoftAzureGlobalProviderOptionsV2();
        }

        /// <summary>
        /// Creates the redirect query string parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected override Dictionary<string, string> CreateRedirectQueryStringParameters(OAuth2Request request)
        {
            var result = base.CreateRedirectQueryStringParameters(request);
            result.AddIfBothNotNullOrEmpty("response_mode", _options.RespondMode);
            return result;
        }

        /// <summary>
        /// Creates the redirect URI.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected override string CreateRedirectUri(OAuth2Request request)
        {
            return string.Format(_options.ProviderOptions.AuthenticationUri, _options.TenantId);
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