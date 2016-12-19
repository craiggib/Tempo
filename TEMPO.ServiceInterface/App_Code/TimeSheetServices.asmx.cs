using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using TEMPO.BusinessEntity;
using TEMPO.DALC;
using TEMPO.BusinessLayer;
using TEMPO.Authorization;

namespace TEMPO.ServiceInterface {
	/// <summary>
	/// TimeSheet Service Interface
	/// </summary>
	public class TimeSheetServices : System.Web.Services.WebService {
		
		public TimeSheetServices()
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


		[WebMethod]
		public PeriodEndingDS GetPeriodEndingByRange(DateTime from, DateTime to, int employeeid) {
			PeriodEndingDALC dalc = new PeriodEndingDALC();
			return (dalc.GetInCompletePeriodEndingByRange(from, to, employeeid));
		}

		[WebMethod]
		public void CreateTimeSheet(TimeSheetDS ds) {
			// update the timesheet row with the default status
			TimeSheet ts = new TimeSheet();
			ts.SubmitTimeSheet(ds);			
		}

		[WebMethod]
		public TimeSheetDS GetSavedRejectedTimeSheets(int employeeid) {
			TimeSheetDALC dalc = new TimeSheetDALC();
			return dalc.GetSavedRejectedTimeSheets(employeeid);
		}

		[WebMethod]
		public void UpdateTimeSheet(TimeSheetDS timesheet) {
			TimeSheetDALC dalc = new TimeSheetDALC();
			dalc.Update(timesheet);
		}

		[WebMethod]
		public void DeleteTimeSheet(TimeSheetDS timesheet) {
			TimeSheetDALC dalc = new TimeSheetDALC();
			dalc.Delete(timesheet);
		}

		[WebMethod]
		public TimeSheetDS GetSubmittedTimeSheets() {
			TimeSheetDALC dalc = new TimeSheetDALC();
			return (dalc.GetSubmittedTimeSheets());
		}

		[WebMethod]
		public void ApproveTimeSheets(TimeSheetDS timesheet) {
			TimeSheetDALC dalc = new TimeSheetDALC();
			dalc.UpdateTimeSheet_CopyTimeEntryToMMT(timesheet);
		}

		/// <summary>
		/// Get all the Approved TimeSheets filtered by date range and employee
		/// </summary>
		[WebMethod]
		public TimeSheetDS GetApprovedTimeSheets(int employeeid, DateTime from, DateTime to) {
			TimeSheetDALC dalc = new TimeSheetDALC();
			return (dalc.GetApprovedTimeSheetsByDate(employeeid,from,to));
		}

		/// <summary>
		/// Get all the Projects for timesheet entry
		/// </summary>
		[WebMethod]
		public ProjectDS GetProjectList() {
			ProjectDALC dalc = new ProjectDALC();
			return (dalc.GetProjectList());
		}

		/// <summary>
		/// Get the possible new status states for a timesheet
		/// </summary>
		/// <param name="timesheet">the timesheet dataset to use</param>
		/// <param name="timesheetindex">the index of the timesheet to check</param>
		[WebMethod]
		public StatusDS GetStatusStates(TimeSheetDS timesheet, int index, int callingempid) {
			// build the business rule object we need
			TimeSheet ts = new TimeSheet();
			// build a status object that represents our data
			StatusDS ds = new StatusDALC().GetStatusByID(timesheet.TimeSheet[index].StatusID);
			// make a new prinicipal object
			TEMPOPrincipal callinguser = new TEMPOPrincipal(new TEMPOIdentity(callingempid));
			// make the call to the business rule
			return (ts.GetPossibleStatusStates(ds.Status[0], callinguser) );            
		}


	}
}
