namespace Beyova.OAuth2
{
    /// <summary>
    /// See: https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-protocols-oauth-code
    /// </summary>
    public class MicrosoftOAuth2ClientOptions : OAuth2ClientOptions
    {
        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the respond mode.
        /// </summary>
        /// <value>
        /// The respond mode.
        /// </value>
        public string RespondMode { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftOAuth2ClientOptions"/> class.
        /// </summary>
        public MicrosoftOAuth2ClientOptions() : base()
        {
            TenantId = "common";
        }
    }
}