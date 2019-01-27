namespace Beyova.OAuth2
{
    /// <summary>
    /// See: https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent
    /// </summary>
    public static class MicrosoftOAuth2RespondModes
    {
        /// <summary>
        /// The query
        /// </summary>
        public const string Query = "query";

        /// <summary>
        /// The fragment
        /// </summary>
        public const string Fragment = "fragment";

        /// <summary>
        /// The form post
        /// </summary>
        public const string FormPost = "form_post";
    }
}