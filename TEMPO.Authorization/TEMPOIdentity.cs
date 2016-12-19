using System;
using System.Security.Principal;
using System.Configuration;

namespace TEMPO.Authorization {
	/// <summary>
	/// Custom Security Identity for Authorization
	/// </summary>
	public class TEMPOIdentity:IIdentity {
		
		#region Member Declarations 

		private bool _isauthenticated;
		private string _username;
		private string _password;
		private int _userid;

		#endregion

		#region Public Initalization

		public TEMPOIdentity(string username, string password){
			AuthFramework.AuthorizationServices authorization = new TEMPO.Authorization.AuthFramework.AuthorizationServices();
			authorization.Url = System.Configuration.ConfigurationManager.AppSettings["TEMPO.Authorization.AuthFramework.AuthorizationServices"];

			_userid = authorization.AuthenticateUser(username, password);
			if ( _userid != -1 ) {
				_isauthenticated = true;
				_username = username;
				_password = password;
			}
			else 
				_isauthenticated = false;
		}

		public TEMPOIdentity(int userid){
			_userid = userid;
			_isauthenticated = true;
		}

		#endregion

		#region IIdentity Members

		public bool IsAuthenticated {
			get {return _isauthenticated; }
		}

		public string Name {
			get { return _username; }
		}

		public string AuthenticationType {
			get { return "TEMPO.AuthorizationFramework"; }
		}

		#endregion

		#region Properties

		public int UserID {
			get { return _userid; }
		}

		public string Password {
			get { return _password; }
		}

		#endregion
	}
}
