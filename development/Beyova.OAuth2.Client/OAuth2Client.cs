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
    public class OAuth2Client : OAuth2Client<OAuth2ClientOptions, OAuth2Request>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Client"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public OAuth2Client(OAuth2ClientOptions options) : base(options)
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class OAuth2Client<TOption, TRequest>
        where TOption : OAuth2ClientOptions, new()
        where TRequest : OAuth2Request
    {
        /// <summary>
        /// The options
        /// </summary>
        protected TOption _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Client"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public OAuth2Client(TOption options)
        {
            ValidateOptions(options);
            _options = options;
        }

        /// <summary>
        /// Validates the options.
        /// </summary>
        /// <param name="options">The options.</param>
        protected virtual void ValidateOptions(TOption options)
        {
            options.CheckNullObject(nameof(options));
            options.ClientId.CheckEmptyString(nameof(options.ClientId));
            options.ClientSecret.CheckEmptyString(nameof(options.ClientSecret));
            options.RedirectUri.CheckEmptyString(nameof(options.RedirectUri));

            options.ProviderOptions.CheckNullObject(nameof(options.ProviderOptions));
            options.ProviderOptions.AccessTokenUri.CheckEmptyString(nameof(options.ProviderOptions.AccessTokenUri));
            options.ProviderOptions.AuthenticationUri.CheckEmptyString(nameof(options.ProviderOptions.AuthenticationUri));

            if (string.IsNullOrWhiteSpace(options.ProviderOptions.AuthenticationHttpMethod))
            {
                options.ProviderOptions.AuthenticationHttpMethod = HttpConstants.HttpMethod.Post;
            }
        }

        /// <summary>
        /// Creates the redirect query string parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> CreateRedirectQueryStringParameters(OAuth2Request request)
        {
            var result = new Dictionary<string, string> {
                {Constants.RFCKeys.ClientId, this._options.ClientId},
                {Constants.RFCKeys.RedirectUri, this._options.RedirectUri},
                {Constants.RFCKeys.ResponseType,   _options.ProviderOptions.AuthenticationByCodeResponseType.SafeToString(Constants.GrantTypes.AuthorizationCode)}
            };

            result.AddIfBothNotNullOrEmpty(Constants.RFCKeys.Scope, request?.Scope);
            result.AddIfBothNotNullOrEmpty(Constants.RFCKeys.State, request?.State);

            return result;
        }

        /// <summary>
        /// Creates the redirect URI.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual string CreateRedirectUri(OAuth2Request request)
        {
            return this._options.ProviderOptions.AuthenticationUri;
        }

        /// <summary>
        /// Creates the authenticate URI.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual string CreateAuthenticateUri(OAuth2AuthenticationRequest request)
        {
            return this._options.ProviderOptions.AccessTokenUri;
        }

        /// <summary>
        /// Deserializes the o auth2 user profile.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        protected virtual OAuth2UserProfile DeserializeOAuth2UserProfile(JToken json)
        {
            throw new NotImplementedException();
        }

        #region Public method

        /// <summary>
        /// Creates the redirect.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string CreateRedirect(OAuth2Request request = null)
        {
            try
            {
                var parameters = CreateRedirectQueryStringParameters(request);
                var uri = CreateRedirectUri(request);
                var requestUrl = CombineRequest(uri, parameters);

                return requestUrl;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { request });
            }
        }

        /// <summary>
        /// Authenticates the by code.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Beyova.Diagnostic.UnsupportedException">AuthenticationHttpMethod</exception>
        public OAuth2AuthenticationResult AuthenticateByCode(OAuth2AuthenticationRequest request)
        {
            return Authenticate(request, CreateAuthenticateByCodeParameters);
        }

        /// <summary>
        /// Authenticates the by token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public OAuth2AuthenticationResult AuthenticateByToken(OAuth2AuthenticationRequest request)
        {
            return Authenticate(request, CreateAuthenticateByTokenParameters);
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="authenticationResult">The authentication result.</param>
        /// <returns></returns>
        /// <exception cref="UnsupportedException">OAuth2UserProfile</exception>
        public OAuth2UserProfile GetUserProfile(OAuth2AuthenticationResult authenticationResult)
        {
            try
            {
                authenticationResult.CheckNullObject(nameof(authenticationResult));

                if (string.IsNullOrWhiteSpace(_options.ProviderOptions.UserProfileUri))
                {
                    throw new UnsupportedException(nameof(OAuth2UserProfile));
                }

                var httpReqeust = _options.ProviderOptions.UserProfileUri.CreateHttpWebRequest();
                httpReqeust.FillAuthentication(authenticationResult);

                return DeserializeOAuth2UserProfile(httpReqeust.ReadResponseAsObject<JToken>().Body);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { authenticationResult });
            }
        }

        #endregion Public method

        /// <summary>
        /// Creates the authenticate by code parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> CreateAuthenticateByCodeParameters(OAuth2AuthenticationRequest request)
        {
            var result = new Dictionary<string, string> {
                {Constants.RFCKeys.ClientId, this._options.ClientId},
                {Constants.RFCKeys.ClientSecret, this._options.ClientSecret},
                {Constants.RFCKeys.RedirectUri, this._options.RedirectUri},
                {Constants.RFCKeys.Code, request.Token},
                {Constants.RFCKeys.GrantType, Constants.GrantTypes.AuthorizationCode}
            };

            result.AddIfBothNotNullOrEmpty(Constants.RFCKeys.Scope, request?.Scope);
            result.AddIfBothNotNullOrEmpty(Constants.RFCKeys.State, request?.State);

            return result;
        }

        /// <summary>
        /// Creates the authenticate by token parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> CreateAuthenticateByTokenParameters(OAuth2AuthenticationRequest request)
        {
            var result = new Dictionary<string, string> {
                {Constants.RFCKeys.ClientId, this._options.ClientId},
                {Constants.RFCKeys.ClientSecret, this._options.ClientSecret},
                {Constants.RFCKeys.RefreshToken, request.Token},
                {Constants.RFCKeys.GrantType, Constants.GrantTypes.RefreshToken}
            };

            return result;
        }

        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="parameterCreator">The parameter creator.</param>
        /// <returns></returns>
        /// <exception cref="UnsupportedException">AuthenticationHttpMethod</exception>
        private OAuth2AuthenticationResult Authenticate(OAuth2AuthenticationRequest request, Func<OAuth2AuthenticationRequest, Dictionary<string, string>> parameterCreator)
        {
            try
            {
                parameterCreator.CheckNullObject(nameof(parameterCreator));
                request.CheckNullObject(nameof(request));
                request.Token.CheckEmptyString(nameof(request.Token));

                var parameters = parameterCreator(request);
                var uri = CreateAuthenticateUri(request);
                HttpWebRequest httpReqeust = null;

                if (this._options.ProviderOptions.AuthenticationHttpMethod.Equals(HttpConstants.HttpMethod.Post, StringComparison.OrdinalIgnoreCase))
                {
                    httpReqeust = uri.CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpReqeust.FillData(parameters, Encoding.UTF8);
                }
                else if (this._options.ProviderOptions.AuthenticationHttpMethod.Equals(HttpConstants.HttpMethod.Get, StringComparison.OrdinalIgnoreCase))
                {
                    httpReqeust = CombineRequest(uri, parameters).CreateHttpWebRequest();
                }
                else
                {
                    throw new UnsupportedException(nameof(this._options.ProviderOptions.AuthenticationHttpMethod), data: new { this._options.ProviderOptions.AuthenticationHttpMethod });
                }

                var response = httpReqeust.ReadResponseAsText(Encoding.UTF8);
                return DeserializeResponse(response.Body);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { request });
            }
        }

        /// <summary>
        /// Deserializes the response.
        /// </summary>
        /// <param name="responseString">The response string.</param>
        /// <returns></returns>
        private static OAuth2AuthenticationResult DeserializeResponse(string responseString)
        {
            try
            {
                responseString.CheckEmptyString(nameof(responseString));

                var json = JObject.Parse(responseString);

                if (json != null)
                {
                    OAuth2AuthenticationResult result = json.ToObject<OAuth2AuthenticationResult>();
                    result.Expires = DateTime.UtcNow.AddSeconds(json.Value<int>(Constants.RFCKeys.ExpiresIn));
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { responseString });
            }
        }

        /// <summary>
        /// Combines the request.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="queryStringParameters">The query string parameters.</param>
        /// <returns></returns>
        protected string CombineRequest(string uri, Dictionary<string, string> queryStringParameters)
        {
            try
            {
                uri.CheckEmptyString(nameof(uri));
                queryStringParameters.CheckNullObject(nameof(queryStringParameters));

                var queryString = queryStringParameters.ToKeyValuePairString(encodeKeyValue: true);

                var requestUrl = string.Format("{0}{1}{2}",
                    uri,
                    uri.Contains('?') ? "&" : "?",
                    queryString);

                return requestUrl;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { uri, queryStringParameters });
            }
        }
    }
}