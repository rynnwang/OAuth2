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
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the cellphone number.
        /// </summary>
        /// <value>
        /// The cellphone number.
        /// </value>
        public CellphoneNumber CellphoneNumber { get; set; }
    }
}