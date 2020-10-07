using Newtonsoft.Json;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public class OAuth2AuthenticationRequest : OAuth2Request
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}