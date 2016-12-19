using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// Project Type DALC.
	/// </summary>
	public class ProjectTypeDALC:BaseDALC  {
		
		public ProjectTypeDALC() {}

		/// <summary>
		/// Build a Project Typed Dataset that contains one set of project information
		/// </summary>
		public ProjectTypeDS GetProjectTypes() {
			string sql;
			
			// create the empty entity
			ProjectTypeDS ds = new ProjectTypeDS();

			// then build the Project Type Table
			sql = "select * from ProjectType";
			string[] tables = new string[] {"ProjectType"};

			// Build all the paramaters
			SqlParameter[] sqlparams = new SqlParameter[2];
			
			SqlParameter temp = new SqlParameter("@ProjectTypeID", SqlDbType.Int, 4, "ProjectTypeID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@ProjectTypeDesc", SqlDbType.VarChar, 50, "ProjectTypeDesc");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			// fill the project type portion of the dataset
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);

			
			// return the populated dataset
			return ds;
		}
	}
}
