using Newtonsoft.Json;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public class OAuth2Request
    {
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.Scope)]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [JsonProperty(PropertyName = Constants.RFCKeys.State)]
        public string State { get; set; }
    }
}