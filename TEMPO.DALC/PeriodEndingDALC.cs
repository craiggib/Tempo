using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// Period Ending Data Access Logic Component
	/// </summary>
	public class PeriodEndingDALC : BaseDALC {
		
		public PeriodEndingDALC() {}	

		/// <summary>
		/// Get all the PeriodEndings in a given time range
		/// </summary>
		/// <returns>a strongly typed data set</returns>
		public PeriodEndingDS GetInCompletePeriodEndingByRange(DateTime from, DateTime to, int employeeid) {
			
			PeriodEndingDS ds = new PeriodEndingDS();
			string[] tables = new string[] {"PeriodEnding"};

			SqlParameter[] sqlparams = new SqlParameter[3];

			SqlParameter temp = new SqlParameter("@To", SqlDbType.DateTime, 4);
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = to;
			sqlparams[0] = temp;

			temp = new SqlParameter("@From", SqlDbType.DateTime, 4);
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = from;
			sqlparams[1] = temp;

			temp = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = employeeid;
			sqlparams[2] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.StoredProcedure, "GetInCompletePeriodEndingByRange", ds, tables, sqlparams);
			return ds;
		}
	}

}
