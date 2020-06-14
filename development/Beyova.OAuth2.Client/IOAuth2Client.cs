using Beyova.Diagnostic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Beyova.OAuth2
{
    /// <summary>
    ///
    /// </summary>
    public interface IOAuth2Client
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        string Domain { get; }

        #region Public method

        /// <summary>
        /// Creates the redirect.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        string CreateRedirect(OAuth2Request request);

        /// <summary>
        /// Authenticates the by code.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Beyova.Diagnostic.UnsupportedException">AuthenticationHttpMethod</exception>
        AuthenticationResult AuthenticateByCode(OAuth2AuthenticationRequest request);

        /// <summary>
        /// Authenticates the by token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        AuthenticationResult AuthenticateByToken(OAuth2AuthenticationRequest request);

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="authenticationResult">The authentication result.</param>
        /// <returns></returns>
        /// <exception cref="UnsupportedException">OAuth2UserProfile</exception>
        OAuth2UserProfile GetUserProfile(OAuth2AuthenticationResult authenticationResult);

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        OAuth2UserProfile GetUserProfile(string accessToken);

        #endregion Public method
    }
}