using System;
using TEMPO.BusinessEntity;
using TEMPO.DALC;
using TEMPO.Authorization;


namespace TEMPO.BusinessLayer
{
	/// <summary>
	/// Contains all the business logic and rules for processing time sheets
	/// </summary>
	public class TimeSheet {
		public TimeSheet() { }

		#region Business Rules for Timesheets 

		/// <summary>
		/// Submits a new timesheet to the database after attatching the default status
		/// Assumes that the timesheet row is in the first row of the timesheet dataset
		/// </summary>
		/// <param name="ds">the timesheet to submit</param>
		public void SubmitTimeSheet(TimeSheetDS timsheet) {
			// assign the default status to the first row of the time sheet
			timsheet.TimeSheet[0].StatusID = 1;
			// then use the DACL to save the timesheet to the database
			TimeSheetDALC dalc = new TimeSheetDALC();
			dalc.CreateTimeSheet(timsheet);
		}

		/// <summary>
		/// Get the possible 'to-states' for a timesheet status based on a user and a current status
		/// </summary>
		/// <param name="user">the user requesting it with certain permissions</param>
		/// <param name="currentstatus">the current status of the timesheet</param>
		/// <returns></returns>
		public StatusDS GetPossibleStatusStates(StatusDS.StatusRow currentstatus, TEMPOPrincipal user) {
			string[] states;

			// there are four possible states
			states = new string[2];
			states[0] = "XX";
			states[1] = "XX";
			
			// make the decision on the current state
			switch ( currentstatus.StatusName.Trim() ) {
				case "Saved":
					// a user can move it from saved to submitted only
					states[0] = "Saved";
					states[1] = "Submitted";
					break;
				case "Submitted":
					// if it has been submitted then they have to be an admin to move it along or reject it
					if (user.IsInRole("ADMIN")) {
						states[0] = "Approved"; 
						states[1] = "Rejected";
					}
					break;
				case "Approved":
					// only an admin can un-approve things back to saved
					if (user.IsInRole("ADMIN")) {
						states[0] = "Saved";
					}
					break;
				case "Rejected":
					// a user can move a rejected timesheet to a saved or submitted state
					states[0] = "Saved";
					states[1] = "Submitted";
					break;
			}
			
			// trim the excess state information
//			states = new string[index];
//			for (int i=0; i< index; i++) {
//				states[i] = tempstates[i];
//			}
			// now that we have the possible states make the query into the database
			return (new StatusDALC().GetStatusByName(states));			
		}

		#endregion

	}
}
