using System;
using System.Collections.Generic;
using System.Text;

namespace SnapCarHire
{
	public class GoogleConstants
	{
		public static string AppName = "Jax Mobile app";

		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "453794223987-7iqid06hph885kqvj2pkha06jed55k20.apps.googleusercontent.com";
		public static string AndroidClientId = "453794223987-c9vel7ddri0ftaipr12tv3d01h6t2e14.apps.googleusercontent.com";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "com.googleusercontent.apps.453794223987-7iqid06hph885kqvj2pkha06jed55k20:/oauth2redirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.453794223987-c9vel7ddri0ftaipr12tv3d01h6t2e14:/oauth2redirect";
	}
}
