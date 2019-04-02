namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public static class OAuth2ProviderFactory
    {
        /// <summary>
        /// Initializes the <see cref="OAuth2ProviderFactory"/> class.
        /// </summary>
        static OAuth2ProviderFactory() { }

        /// <summary>
        /// Gets the facebook provider options.
        /// </summary>
        /// <value>
        /// The facebook provider options.
        /// </value>
        public static OAuth2ProviderOptions GetFacebookProviderOptions()
        {
            return new OAuth2ProviderOptions
            {
                AuthenticationUri = "https://graph.facebook.com/oauth/authorize",
                AccessTokenUri = "https://graph.facebook.com/oauth/access_token",
                AuthenticationHttpMethod = HttpConstants.HttpMethod.Get
            };
        }

        /// <summary>
        /// Gets the microsoft azure global provider options.
        /// </summary>
        /// <value>
        /// The microsoft azure global provider options.
        /// </value>
        public static OAuth2ProviderOptions GetMicrosoftAzureGlobalProviderOptionsV1(string tenantId = null)
        {
            tenantId = string.IsNullOrWhiteSpace(tenantId) ? "common" : tenantId.Trim();

            return new OAuth2ProviderOptions
            {
                AuthenticationUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/authorize", tenantId),
                AccessTokenUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/token", tenantId),
                AuthenticationHttpMethod = HttpConstants.HttpMethod.Post,
                UserProfileUri = "https://graph.windows.net/me?api-version=1.6",
                AuthenticationByCodeResponseType = Constants.GrantTypes.AuthorizationCode
            };
        }

        /// <summary>
        /// Gets the microsoft azure global provider options v2.
        /// </summary>
        /// <value>
        /// The microsoft azure global provider options v2.
        /// </value>
        public static OAuth2ProviderOptions GetMicrosoftAzureGlobalProviderOptionsV2(string tenantId = null)
        {
            tenantId = string.IsNullOrWhiteSpace(tenantId) ? "common" : tenantId.Trim();
            return new OAuth2ProviderOptions
            {
                AuthenticationUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize", tenantId),
                AccessTokenUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", tenantId),
                AuthenticationHttpMethod = HttpConstants.HttpMethod.Post,
                UserProfileUri = "https://graph.microsoft.com/v1.0/me",
                AuthenticationByCodeResponseType = Constants.GrantTypes.Code
            };
        }

        /// <summary>
        /// Gets the google provider options.
        /// </summary>
        /// <value>
        /// The google provider options.
        /// </value>
        public static OAuth2ProviderOptions GetGoogleProviderOptions()
        {
            return new OAuth2ProviderOptions
            {
                AuthenticationUri = "https://accounts.google.com/o/oauth2/v2/auth",
                AccessTokenUri = "https://www.googleapis.com/oauth2/v4/token",
                AuthenticationHttpMethod = HttpConstants.HttpMethod.Post
            };
        }
    }
}