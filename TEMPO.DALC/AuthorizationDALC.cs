using System;
using System.Data.SqlClient;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// Authorization Data Access Logic
	/// </summary>
	public class AuthorizationDALC : BaseDALC
	{
		public AuthorizationDALC() {}

		/// <summary>
		/// Authenticate a user and return their ID
		/// </summary>
		/// <param name="username">the username of the user to authenticate</param>
		/// <param name="password">the password of the user to authenticate</param>
		/// <returns></returns>
		public int AuthenticateUser(string username, string password) {
            DbCommand command = tempodb.GetStoredProcCommand("GetEmployee");
            // assign the paramater
            tempodb.AddInParameter(command, "@employeename", DbType.String, username);
            tempodb.AddInParameter(command, "@password", DbType.String, password);
            // build the empty data set
            EmployeeDS employeeds = new EmployeeDS();
            // populate the dataset
            tempodb.LoadDataSet(command, employeeds, new string[] { "Employee" });
            if (employeeds.Employee.Count != 0)
                return employeeds.Employee[0].EmpID;
            else
                return -1;
		}

		/// <summary>
		/// Get the User Roles
		/// </summary>
		/// <returns></returns>
		public AuthorizationDS GetUserRoles(int userid) {
            DbCommand command = tempodb.GetStoredProcCommand("GetUserRoles");
            // assign the paramater
            tempodb.AddInParameter(command, "@empid", DbType.Int32, userid);
            // build the empty data set
            AuthorizationDS authds = new AuthorizationDS();
            // populate the dataset
            tempodb.LoadDataSet(command, authds, new string[] { "Module", "Authorization"});
			return authds;
		}

		/// <summary>
		/// Get all the roles
		/// </summary>
		/// <returns></returns>
		public AuthorizationDS GetAllRoles() {
            DbCommand command = tempodb.GetStoredProcCommand("GetAllRoles");
            // build the empty data set
            AuthorizationDS authds = new AuthorizationDS();
            // populate the dataset
            tempodb.LoadDataSet(command, authds, new string[] { "Module" });
            return authds; 
		}

		/// <summary>
		/// Update a users roles
		/// </summary>
		/// <returns></returns>
		public void UpdateRoles(int[] roles, int empid) {
            // delete all the roles associated with the user
            DbCommand command = tempodb.GetStoredProcCommand("DeleteUserRoles");
            tempodb.AddInParameter(command, "@empid", DbType.Int32, empid);
            tempodb.ExecuteNonQuery(command);
            // then add in all the new ones
            
            for (int i=0; i< roles.Length; i++) {
                DbCommand insertcommand = tempodb.GetStoredProcCommand("InsertUserIntoRole");
                tempodb.AddInParameter(insertcommand, "@empid", DbType.Int32, empid);
                tempodb.AddInParameter(insertcommand, "@moduleid", DbType.Int32, roles[i]);
                tempodb.ExecuteNonQuery(command);				
			}
		}
	}
}
