using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// Client Access Logic for Project Entity
	/// </summary>
	public class ProjectDALC : BaseDALC {
		
		public ProjectDALC() { }

		/// <summary>
		/// Build a Project Typed Dataset that contains one set of project information
		/// </summary>
		public ProjectDS GetProject(int projectid) {
			string sql;
			
			// create the empty entity
			ProjectDS ds = new ProjectDS();
			
			// first build the project table
			string[] tables = new string[] {"Project"};
			sql = "select * from Project, Client where Project.ClientID = Client.ClientID and ProjectID = @ProjectID";

			// Build all the paramaters
			SqlParameter[] sqlparams = new SqlParameter[6];
			// these are added to the paramater collection and passed into the sql helper function			
			SqlParameter temp = new SqlParameter("@ProjectID", SqlDbType.Int, 4, "ProjectID");
			temp.Value = projectid;
			temp.Direction = System.Data.ParameterDirection.Input;
			sqlparams[0] = temp;

			temp = new SqlParameter("@ClientID", SqlDbType.Int, 4, "ClientID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			temp = new SqlParameter("@ClientName", SqlDbType.VarChar, 255, "ClientName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[2] = temp;

			temp = new SqlParameter("@JobNumYear", SqlDbType.VarChar, 10, "JobNumYear");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[3] = temp;

			temp = new SqlParameter("@RefJobNum", SqlDbType.VarChar, 50, "RefJobNum");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[4] = temp;

			temp = new SqlParameter("@ProjectTypeID", SqlDbType.Int, 4, "ProjectTypeID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[5] = temp;

			// fill the dataset
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
		
			// return the populated dataset
			return ds;
		}

		/// <summary>
		/// Build a Project Typed Dataset that contains one set of project information
		/// </summary>
		public ProjectDS GetProjectsByClient(int clientid) {
			string sql;
			
			// create the empty entity
			ProjectDS ds = new ProjectDS();
			
			// first build the project table
			string[] tables = new string[] {"Project"};
			sql = "select * from Project, Client where Project.ClientID = Client.ClientID and Client.ClientID = @ClientID";

			// Build all the paramaters
			SqlParameter[] sqlparams = new SqlParameter[7];
			// these are added to the paramater collection and passed into the sql helper function			
			SqlParameter temp = new SqlParameter("@ClientID", SqlDbType.Int, 4, "ClientID");
			temp.Value = clientid;
			temp.Direction = System.Data.ParameterDirection.Input;
			sqlparams[0] = temp;

			temp = new SqlParameter("@ProjectID", SqlDbType.Int, 4, "ProjectID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			temp = new SqlParameter("@ClientName", SqlDbType.VarChar, 255, "ClientName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[2] = temp;

			temp = new SqlParameter("@JobNumYear", SqlDbType.Int, 4, "JobNumYear");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[3] = temp;

			temp = new SqlParameter("@RefJobNum", SqlDbType.VarChar, 50, "RefJobNum");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[4] = temp;

			temp = new SqlParameter("@ProjectTypeID", SqlDbType.Int, 4, "ProjectTypeID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[5] = temp;

			temp = new SqlParameter("@JobNum", SqlDbType.VarChar, 10, "JobNum");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[6] = temp;

			// fill the dataset
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
		
			// return the populated dataset
			return ds;
		}

		/// <summary>
		/// Get all the Projects
		/// </summary>
		/// <returns>a project data set</returns>
		public ProjectDS GetAllProjects() {
			string sql;
			
			// create the empty entity
			ProjectDS ds = new ProjectDS();			
			
			// first build the project table
			string[] tables = new string[] {"Project"};
			sql = "select projectid, project.clientid, client.clientname,jobnumyear,jobnum,refjobnum,projecttypeid from Project, Client where Project.ClientID = Client.ClientID";

			// Build all the paramaters
			SqlParameter[] sqlparams = new SqlParameter[7];
			// these are added to the paramater collection and passed into the sql helper function			
			SqlParameter temp = new SqlParameter("@ProjectID", SqlDbType.Int, 4, "ProjectID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@ClientID", SqlDbType.Int, 4, "ClientID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			temp = new SqlParameter("@ClientName", SqlDbType.VarChar, 255, "ClientName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[2] = temp;

			temp = new SqlParameter("@JobNumYear", SqlDbType.Int, 4, "JobNumYear");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[3] = temp;

			temp = new SqlParameter("@JobNum", SqlDbType.VarChar, 10, "JobNum");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[4] = temp;

			temp = new SqlParameter("@RefJobNum", SqlDbType.VarChar, 50, "RefJobNum");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[6] = temp;

			temp = new SqlParameter("@ProjectTypeID", SqlDbType.Int, 4, "ProjectTypeID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[6] = temp;

			// fill this part of the dataset
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
		
			// return the populated dataset
			return ds;
		}

		/// <summary>
		/// Get all the Projects
		/// </summary>
		/// <returns>a project data set</returns>
		public ProjectDS GetProjectList() {
			string sql;
			
			// create the empty entity
			ProjectDS ds = new ProjectDS();			
			
			// first build the project table
			string[] tables = new string[] {"Project"};
            sql = "select * from ProjectList order by ProjectName desc";
		

			// fill this part of the dataset
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables);
		
			// return the populated dataset
			return ds;
		}


		/// <summary>
		/// Update a Project
		/// This method will only updat the Project Table
		/// </summary>
		/// <param name="ds"></param>
		public void Update (ProjectDS ds) {

			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);

			sql = "insert into Project (ClientID, JobNumYear, JobNum, RefJobNum,ProjectTypeID, Description) values (@ClientID, @JobNumYear, @JobNum, @RefJobNum,@ProjectTypeID, @Description)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@ClientID", System.Data.SqlDbType.Int, 4, "ClientID");
			insertCommand.Parameters.Add("@JobNumYear", System.Data.SqlDbType.Int, 4, "JobNumYear");
			insertCommand.Parameters.Add("@JobNum", System.Data.SqlDbType.VarChar, 10, "JobNum");
			insertCommand.Parameters.Add("@RefJobNum", System.Data.SqlDbType.VarChar, 50, "RefJobNum");
			insertCommand.Parameters.Add("@ProjectTypeID", System.Data.SqlDbType.Int, 4, "ProjectTypeID");
			insertCommand.Parameters.Add("@Description", System.Data.SqlDbType.VarChar, 30, "Description");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update Project set ClientID = @ClientID, JobNumYear=@JobNumYear,JobNum=@JobNum, RefJobNum=@RefJobNum,ProjectTypeID=@ProjectTypeID,Description=@Description Where ProjectID = @ProjectID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@ClientID", System.Data.SqlDbType.Int, 4, "ClientID");
			updateCommand.Parameters.Add("@JobNumYear", System.Data.SqlDbType.Int, 4, "JobNumYear");
			updateCommand.Parameters.Add("@JobNum", System.Data.SqlDbType.VarChar, 10, "JobNum");
			updateCommand.Parameters.Add("@RefJobNum", System.Data.SqlDbType.VarChar, 50, "RefJobNum");
			updateCommand.Parameters.Add("@ProjectTypeID", System.Data.SqlDbType.Int, 4, "ProjectTypeID");
			updateCommand.Parameters.Add("@ProjectID", System.Data.SqlDbType.Int, 4, "ProjectID");
			updateCommand.Parameters.Add("@Description", System.Data.SqlDbType.VarChar, 30, "Description");

			sql = "delete from Project Where ProjectID = @ProjectID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@ProjectID", System.Data.SqlDbType.Int, 4, "ProjectID");

			try {
				SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"Project");
			}
			catch {
				throw new Exception();
			}
		}


	}
}
