A simple C# OAuth2 client library

```
PM> Install-Package Beyova.OAuth2.Client
```

## How to Create the Redirect

First, you need to setup an OAUTH2.0 client. 
(Regarding client is thread safe and with static options, suggest to declare it as static to hold somewhere for reuse.)

```csharp
OAuth2Client client = new OAuth2Client(new OAuth2ClientOptions
{
	ClientId = "my-client-id",
	ClientSecret = "my-client-secret",
	// RedirectUri is the URI which OAUTH server would redirect to.
	// It MUST be exactly same as which is configured in OAUTH server.
	RedirectUri = "https://awesome-domain.com/my-awesome-oauth-callback/",
	ProviderOptions = new OAuth2ProviderOptions
	{
		AuthenticationUri = "https://graph.facebook.com/oauth/authorize",
		AccessTokenUri = "https://graph.facebook.com/oauth/access_token",
		AuthenticationHttpMethod = "POST",
		UserProfileUri = "https://graph.facebook.com/me",
		AuthenticationByCodeResponseType = "code"
	},
	RespondMode = "query"
});

```
Then you can use this client to create redirect URL.

```csharp
var redirectUri = client.CreateRedirect(new OAuth2Request { Scope = "User.Read" });
```

This will give you a URI you can just forward the user to. They will sign in and give the app access, which will redirect back to: `https://awesome-domain.com/my-awesome-oauth-callback?code=XXX`

## How to Authenticate by Code

Still use the declared client instance.

```csharp
var result = client.AuthenticateByCode(new OAuth2AuthenticationRequest 
{ 
	Token = "code from callback from provider" 
});

```

## How to Authenticate by Token

If you need to refresh the token by valid access token in hands.

```csharp
var result = client.AuthenticateByToken(new OAuth2AuthenticationRequest { Token = "Valid token" });

```

NOTES:
- I registered a test OAuth2.0 app in microsoft, and put a test ASP.NET MVC site in source code. 
- I tried to built-in several common OAUTH provider. Right now, I just tested Microsoft OAUTH 2.0 (Gragh 2.0). Might update and set Google and Facebook later.

