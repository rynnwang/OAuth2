using Newtonsoft.Json;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public class AuthenticationResult : AuthenticationResult<OAuth2AuthenticationResult, OAuth2AuthenticateErrorMessage>
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class AuthenticationResult<TResult, TErrorObject>
        where TResult : OAuth2AuthenticationResult
        where TErrorObject : OAuth2AuthenticateErrorMessage
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [JsonProperty(PropertyName = "result")]
        public TResult Result { get; set; }

        /// <summary>
        /// Gets or sets the error object.
        /// </summary>
        /// <value>
        /// The error object.
        /// </value>
        [JsonProperty(PropertyName = "errorObject")]
        public TErrorObject ErrorObject { get; set; }
    }
}