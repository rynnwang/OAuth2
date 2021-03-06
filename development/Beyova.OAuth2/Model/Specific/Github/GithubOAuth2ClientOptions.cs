﻿using Newtonsoft.Json;

namespace Beyova.OAuth2
{
    /// <summary>
    /// </summary>
    public class GithubOAuth2ClientOptions : OAuth2ClientOptions
    {
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow sign up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow sign up]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty(PropertyName = "allowSignUp")]
        public bool AllowSignUp { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        [JsonProperty(PropertyName = "appName")]
        public string AppName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubOAuth2ClientOptions"/> class.
        /// </summary>
        public GithubOAuth2ClientOptions() : base() { }
    }
}