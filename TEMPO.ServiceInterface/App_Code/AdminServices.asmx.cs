using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using TEMPO.BusinessEntity;
using TEMPO.DALC;

namespace TEMPO.ServiceInterface {
	/// <summary>
	/// Summary description for AdminServices.
	/// </summary>
	public class AdminServices : System.Web.Services.WebService
	{
		public AdminServices() {
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

		/// <summary>
		/// Get all Employees
		/// </summary>
		/// <returns>A strongly typed dataset</returns>
		[WebMethod]
		public EmployeeDS GetAllEmployees() { 
			EmployeeDALC edalc = new EmployeeDALC();
			return ( edalc.GetAllEmployees() );
		}

		/// <summary>
		/// Update a disconnected Employee dataset
		/// </summary>
		[WebMethod]
		public void UpdateEmployees(EmployeeDS ds, int[] roles, int empid) {
			EmployeeDALC edalc = new EmployeeDALC();
			edalc.Update(ds);

			AuthorizationDALC adalc = new AuthorizationDALC();
			adalc.UpdateRoles(roles, empid);

		}

		/// <summary>
		/// Create an Employee
		/// </summary>
		[WebMethod]
		public void CreateEmployee(EmployeeDS ds, int[] roles) {
			EmployeeDALC edalc = new EmployeeDALC();
			edalc.Update(ds);

			AuthorizationDALC adalc = new AuthorizationDALC();
			adalc.UpdateRoles(roles, ds.Employee[0].EmpID);
		}

		/// <summary>
		/// Get all the Clients in the system
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public ClientDS GetAllClients() {
			ClientDALC cdalc = new ClientDALC();
			return cdalc.GetAllClients();
		}

		/// <summary>
		/// Update a Client
		/// </summary>
		/// <param name="ds">the client to update</param>
		[WebMethod]
		public void UpdateClients(ClientDS ds) {
			ClientDALC cdalc = new ClientDALC();
			cdalc.Update(ds);
		}

		/// <summary>
		/// Get all the Projects by a certain Project ID
		/// </summary>
		/// <param name="projectid">the project id of the project to get</param>
		/// <returns></returns>
		[WebMethod]
		public ProjectDS GetProject(int projectid) {
			ProjectDALC dalc = new ProjectDALC();
			return dalc.GetProject(projectid);
		}

		/// <summary>
		/// Get all the projects in the system
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public ProjectDS GetAllProjects() {
			ProjectDALC dalc = new ProjectDALC();
			return dalc.GetAllProjects();
		}

		/// <summary>
		/// Update a project data set
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public void UpdateProjects(ProjectDS ds) {
			ProjectDALC dalc = new ProjectDALC();
			dalc.Update(ds);
		}

		/// <summary>
		/// Get a Project Type Dataset
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public ProjectTypeDS GetProjectTypes() {
			ProjectTypeDALC dalc = new ProjectTypeDALC();
			return dalc.GetProjectTypes();
		}

		/// <summary>
		/// Get all the Job Years
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public JobYearDS GetJobYears() {
			JobYearDALC dalc = new JobYearDALC();
			return dalc.GetJobYears();
		}

		/// <summary>
		/// Get all the WorkTypes
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public WorkTypeDS GetWorkTypes() {
			WorkTypeDALC dalc = new WorkTypeDALC();
			return dalc.GetAllWorkTypes();
		}

		/// <summary>
		/// Get all the WorkTypes
		/// </summary>
		/// <returns></returns>
		[WebMethod]
		public void UpdateWorkTypes(WorkTypeDS ds) {
			WorkTypeDALC dalc = new WorkTypeDALC();
			dalc.Update(ds);
		}
		[WebMethod]
		public ProjectDS GetProjectsByClient(int clientid) {
			ProjectDALC dalc = new ProjectDALC();
			return (dalc.GetProjectsByClient(clientid));
		}
	}
}
