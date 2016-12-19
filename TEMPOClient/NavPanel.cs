using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using TEMPO.Authorization;
using TEMPO.Client.UIElements;


namespace TEMPO.Client {
	/// <summary>
	/// Provides the UI for the application navigation buttons 
	/// Will check the threads current principal to verify authentication / authorization
	/// </summary>
	
	public class NavPanel : SubPanel {
		
		#region Member Declaration

		// main nav
		private GraphicButton timesheets, reports, admin, exit;
		// sub nav
		private GraphicButton ts_current, ts_approved;
		private GraphicButton admin_clients, admin_projects, admin_types, admin_employees, admin_mmt, admin_approve;
		private GraphicButton reports_employee, reports_project;

		#endregion

		#region Enumeration for Sub Menu Display

		private enum SubNavigationMenuType {
			TimeSheets = 0,
			Reports = 1,
			Admin = 2,
			Exit = 3
		}
		
		#endregion
	
		# region Public Initalization

		public NavPanel() : base(new Point(20,70), new Size(820,68), "Navigation", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.navpanel.png"));
			
			// main nav
			timesheets = new GraphicButton(new Point(11,11), new Size(209,24));
			timesheets.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesheetsbutton.png"));
			timesheets.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesheetsbutton-over.png"));
			timesheets.Tag = SubNavigationMenuType.TimeSheets;
			timesheets.Click += new System.EventHandler(displaySubNavPanel);
			this.Controls.Add(timesheets);

			reports = new GraphicButton(new Point(224,11), new Size(209,24));
			reports.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.reportsbutton.png"));
			reports.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.reportsbutton-over.png"));
			reports.Tag = SubNavigationMenuType.Reports;
			reports.Click += new System.EventHandler(displaySubNavPanel);
			this.Controls.Add(reports);

			admin = new GraphicButton(new Point(437,11), new Size(209,24));
			admin.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.adminbutton.png"));
			admin.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.adminbutton-over.png"));
			admin.Tag = SubNavigationMenuType.Admin;
			admin.Click += new System.EventHandler(displaySubNavPanel);
			this.Controls.Add(admin);

			exit = new GraphicButton(new Point(650,11), new Size(162,24));
			exit.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.exitbutton.png"));
			exit.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.exitbutton-over.png"));
			exit.Click += new System.EventHandler(exitApplication);
			this.Controls.Add(exit);

			// always enable the exit button
			exit.Visible = true;
			exit.Enabled = true;
			

			// TimeSheet Sub Panel
			ts_current = new GraphicButton(new Point(11,40), new Size(180,23));
			ts_current.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.currentbutton.png"));
			ts_current.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.currentbutton-over.png"));
			ts_current.Visible = false;
			ts_current.Click += new System.EventHandler(openTimeSheets);
			this.Controls.Add(ts_current);
			
			ts_approved = new GraphicButton(new Point(196,40), new Size(180,23));
			ts_approved.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.approvedbutton.png"));
			ts_approved.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.approvedbutton-over.png"));
			ts_approved.Visible = false;
			ts_approved.Click += new System.EventHandler(openApprovedSearch);
			this.Controls.Add(ts_approved);

			// Admin Sub Panel
			admin_clients = new GraphicButton(new Point(11,40), new Size(106,23));
			admin_clients.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.clientsbutton.png"));
			admin_clients.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.clientsbutton-over.png"));
			admin_clients.Visible = false;
			admin_clients.Click += new System.EventHandler(openClientCRUD);
			this.Controls.Add(admin_clients);

			admin_projects = new GraphicButton(new Point(120,40), new Size(106,23));
			admin_projects.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.projectsbutton.png"));
			admin_projects.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.projectsbutton-over.png"));			
			admin_projects.Visible = false;
			admin_projects.Click += new System.EventHandler(openProjectCRUD);
			this.Controls.Add(admin_projects);

			admin_employees = new GraphicButton(new Point(229,40), new Size(106,23));
			admin_employees.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.employeesbutton.png"));
			admin_employees.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.employeesbutton-over.png"));	
			admin_employees.Visible = false;
			admin_employees.Click += new System.EventHandler(openEmployeeCRUD);
			this.Controls.Add(admin_employees);

			admin_types = new GraphicButton(new Point(338,40), new Size(106,23));
			admin_types.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.typesbutton.png"));
			admin_types.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.typesbutton-over.png"));	
			admin_types.Visible = false;
			admin_types.Click += new System.EventHandler(openTypeCRUD);
			this.Controls.Add(admin_types);

//			admin_mmt = new GraphicButton(new Point(448,40), new Size(180,23));
//			admin_mmt.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.mmtbutton.png"));
//			admin_mmt.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.mmtbutton-over.png"));	
//			admin_mmt.Visible = false;
//			this.Controls.Add(admin_mmt);

			admin_approve = new GraphicButton(new Point(448,40), new Size(180,23));
			admin_approve.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.approvebutton.png"));
			admin_approve.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.approvebutton-over.png"));	
			admin_approve.Visible = false;
			admin_approve.Click += new System.EventHandler(openApproveTimeSheets);
			this.Controls.Add(admin_approve);
			
			// Reports Sub Panel

			reports_employee = new GraphicButton(new Point(224,40), new Size(209,23));
			reports_employee.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.byemployeebutton.png"));
			reports_employee.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.byemployeebutton-over.png"));	
			reports_employee.Visible = false;
			reports_employee.Click += new System.EventHandler(openReportsEmployee);
			this.Controls.Add(reports_employee);

			reports_project = new GraphicButton(new Point(438,40), new Size(209,23));
			reports_project.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.byprojectbutton.png"));
			reports_project.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.byprojectbutton-over.png"));	
			reports_project.Visible = false;
			reports_project.Click += new System.EventHandler(openReportsProject);
			this.Controls.Add(reports_project);

			hideAllNavigation();
		}

		#endregion

		#region Context Sensitive Menu Build Function

		/// <summary>
		/// Will enable the navigation based on the threads current authenticated user
		/// </summary>
		public void EnableNavigation() {
			// first hide all the sub navigation buttons
			HideAllSubNavButtons();
		
			// check that we are dealing with the appropriately authenticated prinicpal
			if (Thread.CurrentPrincipal.GetType() == typeof(TEMPOPrincipal)) {
				// verify the thread is TEMPO authenticated
				TEMPOPrincipal currentuser = (TEMPOPrincipal) Thread.CurrentPrincipal;
				if (currentuser.Identity.IsAuthenticated) {
					// show all the buttons
					admin.Visible=true;
					timesheets.Visible = true;
					reports.Visible = true;
					// then enable the buttons the authenticated user has access too
					// is the user an administrator?
					if (currentuser.IsInRole("ADMIN")) admin.Enabled = true;
					// is the user able to view timesheets?
					if (currentuser.IsInRole("TIMESHEETS")) timesheets.Enabled = true;
					// is the user able to view reports?
					if (currentuser.IsInRole("REPORTS")) reports.Enabled = true;
				}				
				else {
					// the thread isn't authenticated, so don't display any navigation
					hideAllNavigation();
				}
			}
			else hideAllNavigation();
		}
		#endregion

		# region UI Helper Functions

		/// <summary>
		///  Display the appropriate Sub Navigation menu
		/// </summary>
		/// <param name="submenu">the submen to display</param>
		private void ShowSubNavigationSet(SubNavigationMenuType submenu) {
			HideAllSubNavButtons();
			switch (submenu) {
				case SubNavigationMenuType.TimeSheets:
					ts_current.Visible = true;
					ts_approved.Visible = true;
					break;
				case SubNavigationMenuType.Admin:
					admin_clients.Visible = true;
					admin_projects.Visible = true;
					admin_types.Visible = true;
					admin_employees.Visible = true;
//					admin_mmt.Visible = true;
					admin_approve.Visible = true;
					break;
				case SubNavigationMenuType.Reports:
					reports_employee.Visible = true;
					reports_project.Visible = true;
					break;
			}
		}

		/// <summary>
		///  Hide all the sub navigation buttons
		/// </summary>
		private void HideAllSubNavButtons() {
			ts_current.Visible = false;
			ts_approved.Visible = false;
			admin_clients.Visible = false;
			admin_projects.Visible = false; 
			admin_types.Visible = false;
			admin_employees.Visible = false;
//			admin_mmt.Visible = false;
			admin_approve.Visible = false;
			reports_employee.Visible = false;
			reports_project.Visible = false;
		}


		public void hideAllNavigation() {
			timesheets.Visible = false;
			timesheets.Enabled = false;
			admin.Visible=false;
			admin.Enabled = false;
			reports.Visible = false;
			reports.Enabled = false;
			HideAllSubNavButtons();
		}

		/// <summary>
		/// Displays the Navigation Buttons but they are all disabled
		/// </summary>
		public void showNavigation() {
			timesheets.Visible = true;
			timesheets.Enabled = false;
			
			admin.Visible = true;
			admin.Enabled = false;

			reports.Visible = true;
			reports.Enabled = false;
		}

		#endregion

		#region UI Event Handlers

		/// <summary>
		/// Event handler for sub navigation panel switching
		/// </summary>
		private void displaySubNavPanel( object sender, EventArgs e) {
			// the sender to this will be the graphic button
			GraphicButton buttonsender = (GraphicButton)sender;
			// cast the tag data as the sub nav enumerator and load the appropriate panel
			ShowSubNavigationSet((SubNavigationMenuType)buttonsender.Tag);
		}

		private void openTimeSheets(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.TimeSheets);
		}

		private void openApprovedSearch(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.ApprovedSearch);
		}

		private void openEmployeeCRUD(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.EmployeeCRUD);
		}

		private void openProjectCRUD(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.ProjectCRUD);
		}

		private void openClientCRUD(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.ClientCRUD);
		}

		private void openTypeCRUD(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.TypeCRUD);
		}

		private void openApproveTimeSheets(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.ApproveTimeSheets);
		}

		private void openReportsEmployee(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.EmployeeSummaryReport);
		}

		private void openReportsProject(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.showModule(MainScreen.ModuleType.ProjectSummaryReport);
		}
		
		/// <summary>
		/// Close the Application
		/// </summary>
		private void exitApplication(object sender, EventArgs e) {
			MainScreen ms = (MainScreen) this.Parent;
			ms.Close();
		}

		#endregion

	}
}
