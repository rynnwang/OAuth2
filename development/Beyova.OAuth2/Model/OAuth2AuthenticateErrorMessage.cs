using Newtonsoft.Json;

namespace Beyova.OAuth2
{
    /// <summary>
    /// </summary>
    public class OAuth2AuthenticateErrorMessage
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.Error)]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// The error description.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.ErrorDescription)]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets the error URI.
        /// </summary>
        /// <value>
        /// The error URI.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.ErrorUri)]
        public string ErrorUri { get; set; }
    }
}