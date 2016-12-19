using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;


namespace TEMPO.DALC
{
	/// <summary>
	/// Status Data Access Logic Component
	/// </summary>
	public class StatusDALC : BaseDALC {
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public StatusDALC() { }

		/// <summary>
		///  get all the status codes
		/// </summary>
		/// <returns></returns>		
		public StatusDS GetAllStatus() {
			string sql;
			sql = "select * from Status;";

			StatusDS ds = new StatusDS();
			string[] tables = new string[] {"Status"};

			SqlParameter[] sqlparams = new SqlParameter[2];

			SqlParameter temp = new SqlParameter("@StatusID", SqlDbType.Int, 4, "StatusID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@StatusName", SqlDbType.VarChar, 4, "StatusName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
			return ds;
		}

		/// <summary>
		/// Get a status dataset based on an ID
		/// </summary>
		/// <returns></returns>
		public StatusDS GetStatusByID(int statusid) {
			string sql;
			sql = "select * from Status where statusid = @StatusID;";

			StatusDS ds = new StatusDS();
			string[] tables = new string[] {"Status"};

			SqlParameter[] sqlparams = new SqlParameter[1];

			SqlParameter temp = new SqlParameter("@StatusID", SqlDbType.Int, 4, "StatusID");
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = statusid;
			sqlparams[0] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
			return ds;
		}

		/// <summary>
		/// Build a status dataset based on the named states
		/// </summary>
		/// <param name="names">the array of named status to query against</param>
		/// <returns>a status dataset whose rows contain the status information for each type requested</returns>
		public StatusDS GetStatusByName(string[] names) {
			string sql;			

			StatusDS ds = new StatusDS();
			string[] tables = new string[] {"Status"};

			SqlParameter[] sqlparams = new SqlParameter[3];

			SqlParameter temp = new SqlParameter("@StatusID", SqlDbType.Int, 4, "StatusID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@StatusName", SqlDbType.VarChar, 100, "StatusName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			// build the sql string
			sql = "select * from Status where StatusName in (";			
			for (int i=0; i< names.Length; i++) {
				sql += "'" + names[i] + "'" ;
				// if we aren't at the last one
				if ( (i+1) != names.Length) sql += ",";
			}
			sql += ")";
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
			return ds;
		}
	}
}
