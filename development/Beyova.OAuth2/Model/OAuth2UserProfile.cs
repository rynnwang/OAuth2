using Newtonsoft.Json;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public class OAuth2UserProfile
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        [JsonProperty(PropertyName = "jobTitle")]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name of the user principal.
        /// </summary>
        /// <value>
        /// The name of the user principal.
        /// </value>
        [JsonProperty(PropertyName = "userPrincipalName")]
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets the cellphone number.
        /// </summary>
        /// <value>
        /// The cellphone number.
        /// </value>
        [JsonProperty(PropertyName = "cellphoneNumber")]
        public CellphoneNumber CellphoneNumber { get; set; }
    }
}