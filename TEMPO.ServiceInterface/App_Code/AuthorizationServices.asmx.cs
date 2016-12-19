using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using TEMPO.DALC;
using TEMPO.BusinessEntity;

namespace TEMPO.ServiceInterface
{
	/// <summary>
	/// Summary description for AuthorizationServices.
	/// </summary>
	public class AuthorizationServices : System.Web.Services.WebService
	{
		public AuthorizationServices()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion


		/// <summary>
		/// Authenticates a user and returns their employee id or -1 if not authenticated
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		[WebMethod]
		public int AuthenticateUser(string username, string password) {
			AuthorizationDALC adalc = new AuthorizationDALC();
			return adalc.AuthenticateUser(username, password);
		}

		/// <summary>
		/// Gets the list of user roles for a given user
		/// </summary>
		/// <returns>an array of strings for the representing the 'inRole' list</returns>
		[WebMethod]
		public AuthorizationDS GetUserRoles(int userid) {
			AuthorizationDALC adalc = new AuthorizationDALC();
			return ( adalc.GetUserRoles(userid) ) ;
		}

		/// <summary>
		/// Update a user with a list of role ids
		/// </summary>
		[WebMethod]
		public void UpdateAuthorization(int[] roles, int empid) {
			AuthorizationDALC adalc = new AuthorizationDALC();
			adalc.UpdateRoles(roles,empid);			
		}

		/// <summary>
		/// Gets all the possible system roles
		/// </summary>
		[WebMethod]
		public AuthorizationDS GetAllRoles() {
			AuthorizationDALC adalc = new AuthorizationDALC();
			return adalc.GetAllRoles();
		}


	}
}
