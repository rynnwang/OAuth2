using System;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public class MicrosoftOAuth2ErrorMessage : OAuth2AuthenticateErrorMessage
    {
        /// <summary>
        /// Gets or sets the error codes.
        /// </summary>
        /// <value>
        /// The error codes.
        /// </value>
        [Newtonsoft.Json.JsonProperty(PropertyName = "error_codes")]
        public int[] ErrorCodes { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        [Newtonsoft.Json.JsonProperty(PropertyName = "timestamp")]
        public DateTime? TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>
        /// The trace identifier.
        /// </value>
        [Newtonsoft.Json.JsonProperty(PropertyName = "trace_id")]
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        [Newtonsoft.Json.JsonProperty(PropertyName = "correlation_id")]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftOAuth2ErrorMessage"/> class.
        /// </summary>
        public MicrosoftOAuth2ErrorMessage() : base()
        {
        }
    }
}