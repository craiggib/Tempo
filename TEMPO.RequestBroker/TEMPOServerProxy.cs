using System;
using System.Net;
using System.Web.Services.Protocols;
using System.Threading;
using System.IO;
using TEMPO.BusinessEntity;
using TEMPO.Authorization;
using System.Configuration;
using TEMPO.RequestBroker.ReportingServices;


namespace TEMPO.RequestBroker {
	/// <summary>
	/// Implements Singleton pattern to achieve proxy based access to server
	/// </summary>
	public class TEMPOServerProxy {
	
		#region Member Declaration

		// internal instance declaration
		private static TEMPOServerProxy _instance;
		private static TSServices.TimeSheetServices _tsServices;
		private static AdminServices.AdminServices _adminServices;
        private static ReportingServices.ReportExecutionService _rsServices;
		private static TEMPO.Authorization.AuthFramework.AuthorizationServices _authservices;

		#endregion

		#region Private Initalization
		
		// private constructor
		private TEMPOServerProxy() {
			// create the proxies
			_adminServices = new AdminServices.AdminServices();
			_adminServices.Url = System.Configuration.ConfigurationManager.AppSettings["TEMPO.RequestBroker.AdminServices.AdminServices"];
			
			_tsServices = new TSServices.TimeSheetServices();
			_tsServices.Url = System.Configuration.ConfigurationManager.AppSettings["TEMPO.RequestBroker.TSServices.TimeSheetServices"];
			
			_rsServices = new ReportingServices.ReportExecutionService();
			_rsServices.Url = System.Configuration.ConfigurationManager.AppSettings["TEMPO.RequestBroker.ReportingServices.ReportingService"];
            _rsServices.Credentials = new System.Net.NetworkCredential(
                System.Configuration.ConfigurationManager.AppSettings["TEMPO.Authorization.NetworkCredential.RS.UserName"],
                System.Configuration.ConfigurationManager.AppSettings["TEMPO.Authorization.NetworkCredential.RS.Password"],
                System.Configuration.ConfigurationManager.AppSettings["TEMPO.Authorization.NetworkCredential.RS.Domain"]);
			
			// authorization services
			_authservices = new TEMPO.Authorization.AuthFramework.AuthorizationServices();
			_authservices.Url = System.Configuration.ConfigurationManager.AppSettings["TEMPO.Authorization.AuthFramework.AuthorizationServices"];
	
			// Create a new instance of CredentialCache.
			CredentialCache credentialCache = new CredentialCache();

			// Create a new instance of NetworkCredential using the client
			NetworkCredential credentials = new NetworkCredential(Thread.CurrentPrincipal.Identity.Name,((TEMPOIdentity)Thread.CurrentPrincipal.Identity).Password);
			
			// Add the NetworkCredential to the CredentialCache for both proxies
			credentialCache.Add(new Uri(_adminServices.Url), "Basic", credentials);
			credentialCache.Add(new Uri(_tsServices.Url), "Basic", credentials);

			// Add the CredentialCache to the proxy class credentials.
			_tsServices.Credentials = credentialCache;
			_adminServices.Credentials = credentialCache;
		}	
		#endregion

		#region Public Properties

		
		// instance get
		public static TEMPOServerProxy Instance {
			get {
				if (_instance == null) _instance = new TEMPOServerProxy();
				return _instance;
			}
		}
		#endregion

		# region Administrator Proxy Calls

		public EmployeeDS GetAllEmployees () {
			EmployeeDS returnval = new EmployeeDS();
			returnval.Merge(_adminServices.GetAllEmployees());
			return ( returnval );
		}

		public void UpdateEmployees(EmployeeDS eds, int[] roles, int empid) {
			RequestBroker.AdminServices.EmployeeDS reds = new RequestBroker.AdminServices.EmployeeDS();
			reds.Merge(eds);
			_adminServices.UpdateEmployees(reds, roles, empid);
		}

		public void CreateEmployee(EmployeeDS eds, int[] roles) {
			RequestBroker.AdminServices.EmployeeDS reds = new RequestBroker.AdminServices.EmployeeDS();
			reds.Merge(eds);
			_adminServices.CreateEmployee(reds, roles);
		}

		public ClientDS GetAllClients() {
			ClientDS returnval = new ClientDS();
			returnval.Merge(_adminServices.GetAllClients());
			return ( returnval );
		}

		public void UpdateClients(ClientDS cds) {
			RequestBroker.AdminServices.ClientDS r_ds = new RequestBroker.AdminServices.ClientDS();
			r_ds.Merge(cds);
			_adminServices.UpdateClients(r_ds);
		}

		public ProjectDS GetAllProjects() {
			ProjectDS returnval = new ProjectDS();
			returnval.Merge(_adminServices.GetAllProjects());
			return ( returnval );			
		}


		
		public ProjectDS GetProjectsByClient(int clientid) {
			ProjectDS returnval = new ProjectDS();
			returnval.Merge(_adminServices.GetProjectsByClient(clientid));
			return ( returnval );
			
		}

		public void UpdateProjects(ProjectDS pds) {
			RequestBroker.AdminServices.ProjectDS send_ds = new RequestBroker.AdminServices.ProjectDS();
			send_ds.Merge(pds);
			//send_ds.WriteXml("test.xml");
			_adminServices.UpdateProjects(send_ds);			
		}

		public ProjectTypeDS GetProjectTypes() {
			ProjectTypeDS returnval = new ProjectTypeDS();
			returnval.Merge(_adminServices.GetProjectTypes());
			return ( returnval );		
		}

		public JobYearDS GetJobYears() {
			JobYearDS returnval = new JobYearDS();
			returnval.Merge(_adminServices.GetJobYears());
			return ( returnval );		
		}

		public WorkTypeDS GetAllWorkTypes() {
			WorkTypeDS returnval = new WorkTypeDS();
			returnval.Merge(_adminServices.GetWorkTypes());
			return ( returnval );
		}

		public void UpdateWorkTypes(WorkTypeDS ds) {
			RequestBroker.AdminServices.WorkTypeDS send_ds = new RequestBroker.AdminServices.WorkTypeDS();
			send_ds.Merge(ds);
			_adminServices.UpdateWorkTypes(send_ds);
		}
		#endregion 

		# region TimeSheet Proxy Calls

		public PeriodEndingDS GetMonthPeriodEndingNotCompleted(int employeeid) {
			PeriodEndingDS returnval = new PeriodEndingDS();
            returnval.Merge(_tsServices.GetPeriodEndingByRange(System.DateTime.Now.AddMonths(-1), System.DateTime.Now.AddMonths(1), employeeid));
			return ( returnval );
		}

		public void CreateTimeSheet(TimeSheetDS ds) {
			RequestBroker.TSServices.TimeSheetDS send_ds = new RequestBroker.TSServices.TimeSheetDS();
			send_ds.Merge(ds);
			_tsServices.CreateTimeSheet(send_ds);
		}

		public TimeSheetDS GetSavedRejectedTimeSheets(int employeeid) {
			TimeSheetDS returnval = new TimeSheetDS();
			returnval.Merge(_tsServices.GetSavedRejectedTimeSheets(employeeid));
			return ( returnval );
		}

		public void UpdateTimeSheet(TimeSheetDS timesheet) {
			RequestBroker.TSServices.TimeSheetDS send_ds = new RequestBroker.TSServices.TimeSheetDS();
			send_ds.Merge(timesheet);
			_tsServices.UpdateTimeSheet(send_ds);
		}

		public void DeleteTimeSheet(TimeSheetDS timesheet) {
			RequestBroker.TSServices.TimeSheetDS send_ds = new RequestBroker.TSServices.TimeSheetDS();
			send_ds.Merge(timesheet);
			_tsServices.DeleteTimeSheet(send_ds);
		}

		/// <summary>
		/// Determine the status changes that this employee can do based on a certain status
		/// </summary>
		public StatusDS GetStatusChange(TimeSheetDS.TimeSheetRow timesheetrow) {
			// find the row index
			TimeSheetDS tdds = (TimeSheetDS) timesheetrow.Table.DataSet;
			int index = 0;
            for (int i=0; i<tdds.TimeSheet.Rows.Count; i++)
				if (tdds.TimeSheet[i] == timesheetrow) {
					index = i; 
					break; 
				}
            
			// set up the dataset to send
			RequestBroker.TSServices.TimeSheetDS send_ds = new RequestBroker.TSServices.TimeSheetDS();
			send_ds.Merge(tdds);
			
			// set up the data set to recieve
			StatusDS returnval = new StatusDS();
			returnval.Merge(_tsServices.GetStatusStates(send_ds,index,((TEMPOIdentity)Thread.CurrentPrincipal.Identity).UserID));
			return (returnval);			
		}

		public TimeSheetDS GetSubmittedTimeSheets() {
			TimeSheetDS returnval = new TimeSheetDS();
			returnval.Merge(_tsServices.GetSubmittedTimeSheets());
			return ( returnval );
		}

		public void ApprovalUpdate(TimeSheetDS timesheet) {
			RequestBroker.TSServices.TimeSheetDS send_ds = new RequestBroker.TSServices.TimeSheetDS();
			send_ds.Merge(timesheet);
			_tsServices.ApproveTimeSheets(send_ds);
		}

		/// <summary>
		/// Get the Approved TimeSheets filtered by date range and employee
		/// </summary>
		/// <returns></returns>
		public TimeSheetDS GetApprovedTimeSheets(DateTime from, DateTime to) {			
			TimeSheetDS returnval = new TimeSheetDS();
			// add the user id of the employee to the request
			returnval.Merge(_tsServices.GetApprovedTimeSheets(((TEMPOIdentity)Thread.CurrentPrincipal.Identity).UserID, from,to));
			return ( returnval );
		}

		/// <summary>
		/// Get the Project List for entering timesheets
		/// </summary>
		/// <returns></returns>
		public ProjectDS GetProjectsList() {
			ProjectDS returnval = new ProjectDS();
			returnval.Merge(_tsServices.GetProjectList());
			return ( returnval );			
		}

		#endregion

		#region Reporting Services

		public string GetTimeSheetReport(int reportid) {
			byte[] result = null;
			// Render arguments			
			string reportPath = "/TEMPO.Reports/EmployeeTimeSheet";
			string format = "PDF";
			string historyID = null;
			string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

			// Prepare report parameter.
			TEMPO.RequestBroker.ReportingServices.ParameterValue[] parameters = new ParameterValue[1];
            parameters[0] = new TEMPO.RequestBroker.ReportingServices.ParameterValue();
			parameters[0].Name = "tid";
			parameters[0].Value = reportid.ToString();
			
			DataSourceCredentials[] credentials = null;
			string showHideToggle = null;
			string encoding;
			string mimeType;
			Warning[] warnings = null;
			ParameterValue[] reportHistoryParameters = null;
			string[] streamIDs = null;
            string extension;

            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();
            _rsServices.ExecutionHeaderValue = execHeader;
            execInfo = _rsServices.LoadReport(reportPath, historyID);
            _rsServices.SetExecutionParameters(parameters, "en-us"); 


			try {
				// get the report stream
				result = _rsServices.Render(format, devInfo, out extension, out mimeType,out encoding,out warnings, out streamIDs);
			}
			catch (SoapException e) {
				// do something
			}
			// Write the contents of the report to a pdf file file.
			try {
				FileStream stream = File.Create( "report.pdf", result.Length );
				stream.Write( result, 0, result.Length );
				stream.Close();
			}
			catch ( Exception e ) {
				// do something again
			}
			return "report.pdf";
		}

		public string GetEmployeeTimeSummaryReport(int employeeid, DateTime from, DateTime to) {
			byte[] result = null;
			// Render arguments			
			string reportPath = "/TEMPO.Reports/EmployeeTimeSummary";
			string format = "PDF";
			string historyID = null;
			string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

			// Prepare report parameter.
			ParameterValue[] parameters = new ParameterValue[3];
			parameters[0] = new ParameterValue();
			parameters[0].Name = "empid";
			parameters[0].Value = employeeid.ToString();
			parameters[1] = new ParameterValue();
			parameters[1].Name = "fromdate";
			parameters[1].Value = from.ToShortDateString();
			parameters[2] = new ParameterValue();
			parameters[2].Name = "enddate";
			parameters[2].Value = to.ToShortDateString();
			
			DataSourceCredentials[] credentials = null;
			string showHideToggle = null;
			string encoding;
			string mimeType;
			Warning[] warnings = null;
			ParameterValue[] reportHistoryParameters = null;
			string[] streamIDs = null;
            string extension;

            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();
            _rsServices.ExecutionHeaderValue = execHeader;
            execInfo = _rsServices.LoadReport(reportPath, historyID);
            _rsServices.SetExecutionParameters(parameters, "en-us"); 

			try {
				// get the report stream
                result = _rsServices.Render(format, devInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);
			}
			catch (SoapException e) {
				// do something
			}
			// Write the contents of the report to a pdf file file.
			try {
				FileStream stream = File.Create( "report2.pdf", result.Length );
				stream.Write( result, 0, result.Length );
				stream.Close();
			}
			catch ( Exception e ) {
				// do something again
			}
			return "report2.pdf";
		}

		public string GetProjectTimeSummaryReport(int projectid, DateTime from, DateTime to) {
			byte[] result = null;
			// Render arguments			
			string reportPath = "/TEMPO.Reports/ProjectTimeSummary";
			string format = "PDF";
			string historyID = null;
			string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

			// Prepare report parameter.
			ParameterValue[] parameters = new ParameterValue[3];
			parameters[0] = new ParameterValue();
			parameters[0].Name = "projectid";
			parameters[0].Value = projectid.ToString();
			parameters[1] = new ParameterValue();
			parameters[1].Name = "fromdate";
			parameters[1].Value = from.ToShortDateString();
			parameters[2] = new ParameterValue();
			parameters[2].Name = "todate";
			parameters[2].Value = to.ToShortDateString();
			
			DataSourceCredentials[] credentials = null;
			string showHideToggle = null;
			string encoding;
			string mimeType;
			Warning[] warnings = null;
			ParameterValue[] reportHistoryParameters = null;
			string[] streamIDs = null;
            string extension;

            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();
            _rsServices.ExecutionHeaderValue = execHeader;
            execInfo = _rsServices.LoadReport(reportPath, historyID);
            _rsServices.SetExecutionParameters(parameters, "en-us"); 

			try {
				// get the report stream
                result = _rsServices.Render(format, devInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);
			}
			catch (SoapException e) {
				// do something
			}
			// Write the contents of the report to a pdf file file.
			try {
				FileStream stream = File.Create( "report3.pdf", result.Length );
				stream.Write( result, 0, result.Length );
				stream.Close();
			}
			catch ( Exception e ) {
				// do something again
			}
			return "report3.pdf";
		}

		#endregion

		#region Authorization Serives Proxy Calls
		
		public AuthorizationDS GetAuthorization(int empid) {
			AuthorizationDS rval = new AuthorizationDS();
			TEMPO.Authorization.AuthFramework.AuthorizationDS getds = _authservices.GetUserRoles(empid);
			rval.Merge(getds);
			return (rval);
		}

		/// <summary>
		/// Update the roles for a user
		/// </summary>
		/// <param name="roles">an array of strings for the user to be assiged</param>
		/// <param name="empid">the key of the user</param>
		public void UpdateAuthorization(int[] roles, int empid) {
			_authservices.UpdateAuthorization(roles,empid);
		}

		/// <summary>
		/// Get all the possible authorization Roles
		/// </summary>
		/// <param name="roles">an array of strings for the user to be assiged</param>
		/// <param name="empid">the key of the user</param>
		public AuthorizationDS GetAllAuthorizationRoles() {
			AuthorizationDS rval = new AuthorizationDS();
			rval.Merge(_authservices.GetAllRoles());
			return rval;
		}

		#endregion
	}
}
