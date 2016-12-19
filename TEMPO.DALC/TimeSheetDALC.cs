using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// TimeSheet DALC
	/// </summary>
	public class TimeSheetDALC: BaseDALC  {
		
		public TimeSheetDALC() {}

		/// <summary>
		/// Create a New Timesheet
		/// This method will only update the TimeSheet Table
		/// </summary>
		/// <param name="ds"></param>
		public void CreateTimeSheet(TimeSheetDS ds) {

			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);

			sql = "insert into TimeSheet (PEID, EmpID, StatusID, Notes) values (@PEID, @EmpID, @StatusID, @Notes)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@PEID", System.Data.SqlDbType.Int, 4, "PEID");
			insertCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");
			insertCommand.Parameters.Add("@StatusID", System.Data.SqlDbType.Int, 4, "StatusID");
			insertCommand.Parameters.Add("@Notes", System.Data.SqlDbType.VarChar, 500, "Notes");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update TimeSheet set TID = @TID Where TID = @TID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");

			sql = "delete from TimeSheet Where TID = @TID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");

			SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"TimeSheet");

		}

		/// <summary>
		/// Get all the saved timesheets
		/// Two part data query (timesheets and timeentry information)
		/// </summary>
		/// <param name="ds"></param>
		public TimeSheetDS GetSavedRejectedTimeSheets(int employeeid) {

			TimeSheetDS ds = new TimeSheetDS();
			string[] tables = new string[] {"TimeSheet", "TimeEntry"};

			SqlParameter[] sqlparams = new SqlParameter[1];

			// Input Paramaters
			SqlParameter temp = new SqlParameter("@EmployeeID", SqlDbType.Int, 4, "EmployeeID");
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = employeeid;
			sqlparams[0] = temp;

			// use the same input Paramaters
			ds.EnforceConstraints = false;
			SqlHelper.FillDataset(this.DBConnectionString,CommandType.StoredProcedure,"GetSavedRejectedTimeSheets", ds, tables, sqlparams);
			//ds.EnforceConstraints = true;

			return ds;
		}

		/// <summary>
		/// Get all the approved timesheets for a given employee
		/// Two part data query (timesheets and timeentry information)
		/// </summary>
		/// <param name="ds"></param>
		public TimeSheetDS GetApprovedTimeSheetsByDate(int employeeid, System.DateTime RangeFrom, System.DateTime RangeTo) {
			string sql;
			sql = "select tid,timesheet.peid,timesheet.empid,timesheet.statusid,notes,endingdate,statusname,employeename ";
			sql += "from timesheet, status, periodending,employee ";
			sql += "where timesheet.statusid = status.statusid and timesheet.peid = periodending.peid ";
			sql += "and employee.empid = timesheet.empid ";
			sql += "and status.statusname = 'Approved' and timesheet.empid = @EmployeeID ";
			sql += "and periodending.endingdate between @RangeFrom and @RangeTo";

			// assign the table name we are looking for
			TimeSheetDS ds = new TimeSheetDS();
			string[] tables = new string[] {"TimeSheet"};

			SqlParameter[] sqlparams = new SqlParameter[3];
			SqlParameter temp;

			// Build the input Paramaters
			// Employee ID
			temp = new SqlParameter("@EmployeeID", SqlDbType.Int, 4, "EmployeeID");
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = employeeid;
			sqlparams[0] = temp;

			// Date Range From
			temp = new SqlParameter("@RangeFrom", SqlDbType.DateTime, 10, "RangeFrom");
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = RangeFrom;
			sqlparams[1] = temp;

			// Date Range To
			temp = new SqlParameter("@RangeTo", SqlDbType.DateTime, 10, "RangeTo");
			temp.Direction = System.Data.ParameterDirection.Input;
			temp.Value = RangeTo;
			sqlparams[2] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
			
			// Build the SQL query to get the timesheet details
			sql = "select entryid, timeentry.tid, sunday,monday,tuesday,wednesday,thursday,friday,saturday, ";
			sql += "worktypeid,timeentry.projectid,client.clientid ";
			sql += "from timesheet, status, employee, timeentry, client, project,periodending ";
			sql += "where timesheet.statusid = status.statusid ";
			sql += "and employee.empid = timesheet.empid ";
			sql += "and status.statusname = 'Approved' ";
			sql += "and timesheet.empid = @EmployeeID ";
			sql += "and timesheet.tid = timeentry.tid ";
			sql += "and project.projectid = timeentry.projectid ";
			sql += "and project.clientid = client.clientid ";
			sql += "and timesheet.peid = periodending.peid ";
			sql += "and periodending.endingdate between @RangeFrom and @RangeTo";
			
			tables = new string[] {"TimeEntry"};

			// use the same input Paramaters
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);

			return ds;
		}

		/// <summary>
		/// Update a Timesheet
		/// This method will update the Timeentry table as well
		/// </summary>
		/// <param name="ds">The Typed Data set for the timesheet</param>
		public void Update(TimeSheetDS ds) {
			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);
			TimeSheetDS entry_copy = (TimeSheetDS)ds.Copy();
			
			sql = "insert into TimeSheet (PEID, EmpID, StatusID, Notes) values (@PEID, @EmpID, @StatusID, @Notes)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@PEID", System.Data.SqlDbType.Int, 4, "PEID");
			insertCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");
			insertCommand.Parameters.Add("@StatusID", System.Data.SqlDbType.Int, 4, "StatusID");
			insertCommand.Parameters.Add("@Notes", System.Data.SqlDbType.VarChar, 500, "Notes");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update TimeSheet set PEID = @PEID, EmpID = @EmpID, StatusID = @StatusID, Notes = @Notes where TID = @TID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");
			updateCommand.Parameters.Add("@PEID", System.Data.SqlDbType.Int, 4, "PEID");
			updateCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");
			updateCommand.Parameters.Add("@StatusID", System.Data.SqlDbType.Int, 4, "StatusID");
			updateCommand.Parameters.Add("@Notes", System.Data.SqlDbType.VarChar, 500, "Notes");

			sql = "delete from TimeSheet Where TID = @TID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");

			// update the timesheet table
			SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"TimeSheet");
			

			// update the Timeentry table
			sql = "insert into TimeEntry (TID,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,WorktypeID, ProjectID) values ";
			sql += "(@TID,@Sunday,@Monday,@Tuesday,@Wednesday,@Thursday,@Friday,@Saturday,@WorktypeID, @ProjectID)";
			insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");
			insertCommand.Parameters.Add("@Sunday", System.Data.SqlDbType.Decimal, 5, "Sunday");
			insertCommand.Parameters.Add("@Monday", System.Data.SqlDbType.Decimal, 5, "Monday");
			insertCommand.Parameters.Add("@Tuesday", System.Data.SqlDbType.Decimal, 5, "Tuesday");
			insertCommand.Parameters.Add("@Wednesday", System.Data.SqlDbType.Decimal, 5, "Wednesday");
			insertCommand.Parameters.Add("@Thursday", System.Data.SqlDbType.Decimal, 5, "Thursday");
			insertCommand.Parameters.Add("@Friday", System.Data.SqlDbType.Decimal, 5, "Friday");
			insertCommand.Parameters.Add("@Saturday", System.Data.SqlDbType.Decimal, 5, "Saturday");
			insertCommand.Parameters.Add("@WorktypeID", System.Data.SqlDbType.Int, 4, "WorktypeID");
			insertCommand.Parameters.Add("@ProjectID", System.Data.SqlDbType.Int, 4, "ProjectID");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update TimeEntry set TID = @TID,Sunday = @Sunday,Monday=@Monday,Tuesday=@Tuesday,Wednesday=@Wednesday,";
			sql += "Thursday=@Thursday,Friday=@Friday,Saturday=@Saturday,WorktypeID=@WorkTypeID, ProjectID=@ProjectID where EntryID = @EntryID";
			updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");
			updateCommand.Parameters.Add("@Sunday", System.Data.SqlDbType.Decimal, 5, "Sunday");
			updateCommand.Parameters.Add("@Monday", System.Data.SqlDbType.Decimal, 5, "Monday");
			updateCommand.Parameters.Add("@Tuesday", System.Data.SqlDbType.Decimal, 5, "Tuesday");
			updateCommand.Parameters.Add("@Wednesday", System.Data.SqlDbType.Decimal, 5, "Wednesday");
			updateCommand.Parameters.Add("@Thursday", System.Data.SqlDbType.Decimal, 5, "Thursday");
			updateCommand.Parameters.Add("@Friday", System.Data.SqlDbType.Decimal, 5, "Friday");
			updateCommand.Parameters.Add("@Saturday", System.Data.SqlDbType.Decimal, 5, "Saturday");
			updateCommand.Parameters.Add("@WorktypeID", System.Data.SqlDbType.Int, 4, "WorktypeID");
			updateCommand.Parameters.Add("@ProjectID", System.Data.SqlDbType.Int, 4, "ProjectID");
			updateCommand.Parameters.Add("@EntryID", System.Data.SqlDbType.Int, 4, "EntryID");			
			
			sql = "delete from TimeEntry Where EntryID = @EntryID";
			deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@EntryID", System.Data.SqlDbType.Int, 4, "EntryID");

			// update the TimeEntry Table
			SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,entry_copy,"TimeEntry");
		}

		/// <summary>
		/// Update a Timesheet but make a copy of all the changes to the entries in the MoveMiscTime table
		/// This method will NOT update the Timeentry table
		/// Uses the MMT Dataset to update the MMT table
		/// </summary>
		/// <param name="ds">The Typed Data set for the timesheet</param>
		public void UpdateTimeSheet_CopyTimeEntryToMMT(TimeSheetDS ds) {
			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);
			TimeSheetDS entry_copy = (TimeSheetDS)ds.Copy();
			
			sql = "insert into TimeSheet (PEID, EmpID, StatusID, Notes) values (@PEID, @EmpID, @StatusID, @Notes)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@PEID", System.Data.SqlDbType.Int, 4, "PEID");
			insertCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");
			insertCommand.Parameters.Add("@StatusID", System.Data.SqlDbType.Int, 4, "StatusID");
			insertCommand.Parameters.Add("@Notes", System.Data.SqlDbType.VarChar, 500, "Notes");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update TimeSheet set PEID = @PEID, EmpID = @EmpID, StatusID = @StatusID, Notes = @Notes where TID = @TID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");
			updateCommand.Parameters.Add("@PEID", System.Data.SqlDbType.Int, 4, "PEID");
			updateCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");
			updateCommand.Parameters.Add("@StatusID", System.Data.SqlDbType.Int, 4, "StatusID");
			updateCommand.Parameters.Add("@Notes", System.Data.SqlDbType.VarChar, 500, "Notes");

			sql = "delete from TimeSheet Where TID = @TID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");

			// update the timesheet table
			SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"TimeSheet");		

			// update the MMT table if there were changes			
			if ( entry_copy.TimeEntry.GetChanges() != null) {

				// build the move misc time dataset
				MMTDS mmtds = new MMTDS();
				// assign the changes to a local var for easier access
				TimeSheetDS.TimeEntryDataTable tedt = (TimeSheetDS.TimeEntryDataTable)entry_copy.TimeEntry.GetChanges();
				for (int i=0; i<tedt.Count;i++)
					mmtds.MMT.AddMMTRow(tedt[i].EntryID, tedt[i].ProjectID, tedt[i].WorkTypeID);

				sql = "insert into MMT (EntryID,WorkTypeID,ProjectID) values (@EntryID,@WorkTypeID,@ProjectID)";
				insertCommand = new SqlCommand(sql, conn);
				insertCommand.Parameters.Add("@EntryID", System.Data.SqlDbType.Int, 4, "EntryID");
				insertCommand.Parameters.Add("@WorktypeID", System.Data.SqlDbType.Int, 4, "WorktypeID");
				insertCommand.Parameters.Add("@ProjectID", System.Data.SqlDbType.Int, 4, "ProjectID");
				insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

				sql = "update MMT set EntryID = @EntryID,WorktypeID = @WorktypeID,ProjectID=@ProjectID where MMTID = @MMTID";
				updateCommand = new SqlCommand(sql,conn);
				updateCommand.Parameters.Add("@MMTID", System.Data.SqlDbType.Int, 4, "MMTID");
				updateCommand.Parameters.Add("@EntryID", System.Data.SqlDbType.Int, 4, "EntryID");
				updateCommand.Parameters.Add("@WorktypeID", System.Data.SqlDbType.Int, 4, "WorktypeID");
				updateCommand.Parameters.Add("@ProjectID", System.Data.SqlDbType.Int, 4, "ProjectID");			
			
				sql = "delete from MMT where MMTID = @MMTID";
				deleteCommand = new SqlCommand(sql, conn);
				deleteCommand.Parameters.Add("@MMTID", System.Data.SqlDbType.Int, 4, "MMTID");

				// update the TimeEntry Table
				SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,mmtds,"MMT");
			}
		}


		/// <summary>
		/// Delete a Timesheet
		/// Cascade keys / contraints in database will auto delete neccessary child tables
		/// </summary>
		public void Delete (TimeSheetDS ds) {
			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);

			sql = "insert into TimeSheet (PEID, EmpID, StatusID, Notes) values (@PEID, @EmpID, @StatusID, @Notes)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@PEID", System.Data.SqlDbType.Int, 4, "PEID");
			insertCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");
			insertCommand.Parameters.Add("@StatusID", System.Data.SqlDbType.Int, 4, "StatusID");
			insertCommand.Parameters.Add("@Notes", System.Data.SqlDbType.VarChar, 500, "Notes");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update TimeSheet set TID = @TID Where TID = @TID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");

			sql = "delete from TimeSheet Where TID = @TID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@TID", System.Data.SqlDbType.Int, 4, "TID");

			SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"TimeSheet");
		}

		/// <summary>
		/// Get all the submiteed timesheets
		/// Two part data query (timesheets and timeentry information)
		/// </summary>
		/// <param name="ds"></param>
		public TimeSheetDS GetSubmittedTimeSheets() {
			
			TimeSheetDS ds = new TimeSheetDS();
			string[] tables = new string[] {"TimeSheet","TimeEntry"};

			// use the same input Paramaters
			ds.EnforceConstraints = false;
			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.StoredProcedure, "GetSubmittedTimeSheets", ds, tables);
			ds.EnforceConstraints = true;

			return ds;
		}


	}
}
