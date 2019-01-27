namespace Beyova.OAuth2
{
    /// <summary>
    /// See https://tools.ietf.org/html/rfc6749
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///
        /// </summary>
        public static class RFCKeys
        {
            /// <summary>
            /// The client identifier
            /// </summary>
            public const string ClientId = "client_id";

            /// <summary>
            /// The redirect URI
            /// </summary>
            public const string RedirectUri = "redirect_uri";

            /// <summary>
            /// The response type
            /// </summary>
            public const string ResponseType = "response_type";

            /// <summary>
            /// The client secret
            /// </summary>
            public const string ClientSecret = "client_secret";

            /// <summary>
            /// The scope
            /// </summary>
            public const string Scope = "scope";

            /// <summary>
            /// The state
            /// </summary>
            public const string State = "state";

            /// <summary>
            /// The error
            /// </summary>
            public const string Error = "error";

            /// <summary>
            /// The error description
            /// </summary>
            public const string ErrorDescription = "error_description";

            /// <summary>
            /// The error URI
            /// </summary>
            public const string ErrorUri = "error_uri";

            /// <summary>
            /// The grant type
            /// </summary>
            public const string GrantType = "grant_type";

            /// <summary>
            /// The code
            /// </summary>
            public const string Code = "code";

            /// <summary>
            /// The access token
            /// </summary>
            public const string AccessToken = "access_token";

            /// <summary>
            /// The token type
            /// </summary>
            public const string TokenType = "token_type";

            /// <summary>
            /// The expires in
            /// </summary>
            public const string ExpiresIn = "expires_in";

            /// <summary>
            /// The user name
            /// </summary>
            public const string UserName = "username";

            /// <summary>
            /// The password
            /// </summary>
            public const string Password = "password";

            /// <summary>
            /// The refresh token
            /// </summary>
            public const string RefreshToken = "refresh_token";
        }

        /// <summary>
        ///
        /// </summary>
        public static class GrantTypes
        {
            /// <summary>
            /// The refresh token
            /// </summary>
            public const string RefreshToken = "refresh_token";

            /// <summary>
            /// The authorization code
            /// </summary>
            public const string AuthorizationCode = "authorization_code";

            /// <summary>
            /// The code
            /// </summary>
            public const string Code = "code";
        }

        /// <summary>
        ///
        /// </summary>
        public static class Fields
        {
            /// <summary>
            /// The identifier token
            /// </summary>
            public const string IdToken = "id_token";
        }
    }
}