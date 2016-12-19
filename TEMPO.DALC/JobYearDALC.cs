using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// JobYear DALC
	/// </summary>
	public class JobYearDALC : BaseDALC  {
		
		public JobYearDALC(){}
		
		/// <summary>
		/// Build a Project Typed Dataset that contains one set of project information
		/// </summary>
		public JobYearDS GetJobYears() {
			
			// create the empty entity
			JobYearDS ds = new JobYearDS();
			
			// build the table list
			string[] tables = new string[] {"JobYear"};

			// fill the project type portion of the dataset
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.StoredProcedure, "GetJobYears", ds, tables);

			// return the populated dataset
			return ds;
		}

	}
}
