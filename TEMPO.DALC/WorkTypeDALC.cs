using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// Work Type DALC
	/// </summary>
	public class WorkTypeDALC: BaseDALC {

		public WorkTypeDALC(){}	
		
		/// <summary>
		/// Get all the WorkTypes
		/// </summary>
		/// <returns>a strongly typed data set</returns>
		public WorkTypeDS GetAllWorkTypes() {
			string sql;
			sql = "select * from WorkType;";

			WorkTypeDS ds = new WorkTypeDS();
			string[] tables = new string[] {"WorkType"};

			SqlParameter[] sqlparams = new SqlParameter[2];

			SqlParameter temp = new SqlParameter("@WorkTypeID", SqlDbType.Int, 4, "WorkTypeID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@WorkTypeName", SqlDbType.VarChar, 4, "WorkTypeName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
			return ds;
		}

		/// <summary>
		/// Update a WorkType
		/// </summary>
		/// <param name="ds"></param>
		public void Update (WorkTypeDS ds) {
			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);

			sql = "insert into WorkType (WorkTypeName) values (@WorkTypeName)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@WorkTypeName", System.Data.SqlDbType.VarChar, 255, "WorkTypeName");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update WorkType set WorkTypeName = @WorkTypeName Where WorkTypeID = @WorkTypeID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@WorkTypeName", System.Data.SqlDbType.VarChar, 50, "WorkTypeName");
			updateCommand.Parameters.Add("@WorkTypeID", System.Data.SqlDbType.Int, 4, "WorkTypeID");

			sql = "delete from WorkType Where WorkTypeID= @WorkTypeID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@WorkTypeID", System.Data.SqlDbType.Int, 4, "WorkTypeID");

			try {
				SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"WorkType");
			}
			catch {
				throw new Exception();
			}
		}
	
	}
}
