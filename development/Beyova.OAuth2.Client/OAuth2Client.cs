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
    public abstract class OAuth2Client : OAuth2Client<OAuth2ClientOptions, OAuth2Request, OAuth2AuthenticateErrorMessage>
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
    public abstract class OAuth2Client<TOption, TRequest, TErrorObject> : IOAuth2Client
        where TOption : OAuth2ClientOptions, new()
        where TRequest : OAuth2Request
        where TErrorObject : OAuth2AuthenticateErrorMessage
    {
        /// <summary>
        /// The options
        /// </summary>
        protected TOption _options;

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public abstract string Domain { get; }

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
        /// Converts the error object.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        protected virtual TErrorObject ConvertErrorObject(string body)
        {
            return body.TryParseToJToken()?.ToObject<TErrorObject>();
        }

        /// <summary>
        /// Converts the o auth2 authentication result.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        protected virtual OAuth2AuthenticationResult ConvertOAuth2AuthenticationResult(string body)
        {
            try
            {
                body.CheckEmptyString(nameof(body));

                var json = JObject.Parse(body);

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
                throw ex.Handle(new { body });
            }
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
                {Constants.RFCKeys.ClientId, _options.ClientId},
                {Constants.RFCKeys.RedirectUri, _options.RedirectUri},
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
            return _options.ProviderOptions.AuthenticationUri;
        }

        /// <summary>
        /// Creates the authenticate URI.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual string CreateAuthenticateUri(OAuth2AuthenticationRequest request)
        {
            return _options.ProviderOptions.AccessTokenUri;
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
        public AuthenticationResult AuthenticateByCode(OAuth2AuthenticationRequest request)
        {
            return Authenticate(request, CreateAuthenticateByCodeParameters);
        }

        /// <summary>
        /// Authenticates the by token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public AuthenticationResult AuthenticateByToken(OAuth2AuthenticationRequest request)
        {
            return Authenticate(request, CreateAuthenticateByTokenParameters);
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="authenticationResult">The authentication result.</param>
        /// <returns></returns>
        /// <exception cref="UnsupportedException">OAuth2UserProfile</exception>
        public virtual OAuth2UserProfile GetUserProfile(OAuth2AuthenticationResult authenticationResult)
        {
            try
            {
                authenticationResult.CheckNullObject(nameof(authenticationResult));

                if (string.IsNullOrWhiteSpace(_options.ProviderOptions.UserProfileUri))
                {
                    throw new UnsupportedException(nameof(OAuth2UserProfile));
                }

                var httpRequest = _options.ProviderOptions.UserProfileUri.CreateHttpWebRequest();
                FillAuthentication(httpRequest, authenticationResult);

                return DeserializeOAuth2UserProfile(httpRequest.ReadResponseAsObject<JToken>().Body);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { authenticationResult });
            }
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        /// <exception cref="UnsupportedException">OAuth2UserProfile</exception>
        public virtual OAuth2UserProfile GetUserProfile(string accessToken)
        {
            try
            {
                accessToken.CheckEmptyString(nameof(accessToken));

                if (string.IsNullOrWhiteSpace(_options.ProviderOptions.UserProfileUri))
                {
                    throw new UnsupportedException(nameof(OAuth2UserProfile));
                }

                var httpRequest = _options.ProviderOptions.UserProfileUri.CreateHttpWebRequest();
                FillAuthentication(httpRequest, accessToken);

                return DeserializeOAuth2UserProfile(httpRequest.ReadResponseAsObject<JToken>().Body);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { accessToken });
            }
        }

        #endregion Public method

        /// <summary>
        /// Fills the authentication.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="oauthResult">The oauth result.</param>
        protected virtual void FillAuthentication(HttpWebRequest httpRequest, OAuth2AuthenticationResult oauthResult)
        {
            FillAuthentication(httpRequest, oauthResult.AccessToken, oauthResult.TokenType);
        }

        /// <summary>
        /// Fills the authentication.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="tokenType">Type of the token.</param>
        protected virtual void FillAuthentication(HttpWebRequest httpRequest, string accessToken, string tokenType = null)
        {
            if (httpRequest != null && !string.IsNullOrWhiteSpace(accessToken))
            {
                httpRequest.Headers.Set(HttpRequestHeader.Authorization, string.Format("{0} {1}", tokenType.SafeToString("Bearer"), accessToken));
            }
        }

        /// <summary>
        /// Creates the authenticate by code parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> CreateAuthenticateByCodeParameters(OAuth2AuthenticationRequest request)
        {
            var result = new Dictionary<string, string> {
                {Constants.RFCKeys.ClientId, _options.ClientId},
                {Constants.RFCKeys.ClientSecret, _options.ClientSecret},
                {Constants.RFCKeys.RedirectUri, _options.RedirectUri},
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
                {Constants.RFCKeys.ClientId, _options.ClientId},
                {Constants.RFCKeys.ClientSecret, _options.ClientSecret},
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
        private AuthenticationResult Authenticate(OAuth2AuthenticationRequest request, Func<OAuth2AuthenticationRequest, Dictionary<string, string>> parameterCreator)
        {
            TErrorObject errorObject = default(TErrorObject);
            try
            {
                parameterCreator.CheckNullObject(nameof(parameterCreator));
                request.CheckNullObject(nameof(request));
                request.Token.CheckEmptyString(nameof(request.Token));

                var parameters = parameterCreator(request);
                var uri = CreateAuthenticateUri(request);
                HttpWebRequest httpReqeust = null;

                if (_options.ProviderOptions.AuthenticationHttpMethod.Equals(HttpConstants.HttpMethod.Post, StringComparison.OrdinalIgnoreCase))
                {
                    httpReqeust = uri.CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpReqeust.FillData(parameters, Encoding.UTF8);
                }
                else if (_options.ProviderOptions.AuthenticationHttpMethod.Equals(HttpConstants.HttpMethod.Get, StringComparison.OrdinalIgnoreCase))
                {
                    httpReqeust = CombineRequest(uri, parameters).CreateHttpWebRequest();
                }
                else
                {
                    throw new UnsupportedException(nameof(_options.ProviderOptions.AuthenticationHttpMethod), data: new { _options.ProviderOptions.AuthenticationHttpMethod });
                }

                var response = httpReqeust.ReadResponseAsText(Encoding.UTF8);

                // Some OAUTH provider would return 200 even if error occurred.
                errorObject = ConvertErrorObject(response.Body);
                if (errorObject == null)
                {
                    return new AuthenticationResult
                    {
                        Result = ConvertOAuth2AuthenticationResult(response.Body)
                    };
                }
            }
            catch (Exception ex)
            {
                errorObject = GetErrorObject(ex);

                if (errorObject == null)
                {
                    throw ex.Handle(new { request });
                }
            }

            return new AuthenticationResult
            {
                ErrorObject = errorObject
            };
        }

        /// <summary>
        /// Gets the error object.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        protected virtual TErrorObject GetErrorObject(Exception exception)
        {
            HttpOperationException httpException = exception as HttpOperationException;
            if (httpException != null && httpException.ExceptionReference != null && !string.IsNullOrWhiteSpace(httpException.ExceptionReference.ResponseText))
            {
                return ConvertErrorObject(httpException.ExceptionReference.ResponseText);
            }

            return null;
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