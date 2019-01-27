using Newtonsoft.Json;
using System;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public class OAuth2AuthenticationResult
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.AccessToken)]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.TokenType)]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.RefreshToken)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>
        /// The expires.
        /// </value>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.State)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the identifier token.
        /// </summary>
        /// <value>
        /// The identifier token.
        /// </value>
        [JsonProperty(PropertyName = Constants.Fields.IdToken)]
        public string IdToken { get; set; }
    }
}