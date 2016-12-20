using System;
using System.Security.Principal;

namespace TEMPO.Authorization
{
	/// <summary>
	/// Custom Security Framwork Principal for Authorization
	/// </summary>
	public class TEMPOPrincipal:IPrincipal {
		
		#region Member Declarations

		private TEMPOIdentity _identity;
		private string[] _roles;

		#endregion

		#region Public Initialization

		/// <summary>
		/// Create a new TEMPO Principal based on already establishd Identity
		/// </summary>
		/// <param name="identity">The TEMPOIdentity</param>
		public TEMPOPrincipal(TEMPOIdentity identity) {
			// assign to local var
			_identity = identity;
			// get the roles this user belongs too
			AuthFramework.AuthorizationServices authorization = new TEMPO.Authorization.AuthFramework.AuthorizationServices();
			authorization.Url = System.Configuration.ConfigurationManager.AppSettings["TEMPO.Authorization.AuthFramework.AuthorizationServices"];
			AuthFramework.AuthorizationDS authds = authorization.GetUserRoles(_identity.UserID);
			_roles = new string[authds.ModuleAuth.Count];
            for (int i = 0; i < authds.ModuleAuth.Count; i++) 
				_roles[i] = authds.Module.FindByModuleID(authds.ModuleAuth[i].moduleid).ModuleName;
		}

		#endregion

		#region IPrincipal Members

		public IIdentity Identity {
			get {return _identity; }
		}

		/// <summary>
		/// Verify if a user is in a given role. This method will first check authentication of the Identity.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role) {
			if (_identity.IsAuthenticated) {
				for (int i=0; i< _roles.Length; i++)
					if (role == _roles[i])
						return true;
				return false;
			}
			else return false;
		}
		
		#endregion

	}
}
