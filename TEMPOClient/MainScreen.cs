using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using TEMPO.Client.UIElements;
using System.Configuration;

namespace TEMPO.Client {
	/// <summary>
	/// TEMPO Main Form
	/// </summary>
	public class MainScreen : System.Windows.Forms.Form {
		
		#region Member Declaration
		
		private System.ComponentModel.Container components = null;
		public LoginPanel Login;
		public NavPanel Navigation;
		public ApproveTimeSheetSearch ApprovedSearch;
		public EmployeeCRUDPanel EmployeeCRUD;
		public ClientCRUDPanel ClientCRUD;
		public ProjectCRUDPanel ProjectCRUD;
		public TypeCRUDPanel TypeCRUD;
		public TimeSheetsPanel TimeSheets;
		public ApproveTimeSheets ApproveTS;
		public RPT_EmployeeTimeSummary EmpSummaryReport;
		public RPT_ProjectTimeSummary ProjectSummaryReport;
		private SubPanel currentpanel = null;

		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		
		#endregion
			
		#region Public Initialization

		public MainScreen() {
			InitializeComponent();

            // main screen re-size
            this.ClientSize = new System.Drawing.Size(850, 600);

			// asssign the background image
			this.BackgroundImage = new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.mainpanel.png");

			// assign the form icon
			this.Icon = new Icon(typeof(TEMPO.Client.MainScreen), "Resources.trayicon.ico");

			// build and display the login panel
			Login = new LoginPanel();			
			this.Controls.Add(Login);

			//build and display the navigation panel
			Navigation = new NavPanel();
			this.Controls.Add(Navigation);		
	
			// show the version text
			UpdateStatusBar("Version: " + System.Configuration.ConfigurationManager.AppSettings["TEMPO.ApplicationVersion"]);

		}
		
		#endregion

		#region Public Enumeration

		public enum ModuleType {
			TimeSheets = 0,
			ApprovedSearch = 1,
			EmployeeCRUD = 2,
			ProjectCRUD = 3,
			ClientCRUD = 4,
			TypeCRUD = 5,
			ApproveTimeSheets = 6,
			EmployeeSummaryReport = 7,
			ProjectSummaryReport = 8
		}
		#endregion
		
		#region Public UI Helper Methods

		public void UpdateStatusBar(string text) {
			statusBar.Panels[1].Text = text;
		}
	
		public void hideCurrentPanel() {
			if (currentpanel != null) currentpanel.Visible = false;
		}

		/// <summary>
		/// Display a module given a command from the Navigation Panel
		/// </summary>
		/// <param name="module">The enumeration of the module to display</param>
		public void showModule(ModuleType module) {
			// remove the current panel
			this.Controls.Remove(currentpanel);
			// then show the one we care about
			switch (module) {
				case MainScreen.ModuleType.TimeSheets:
					TimeSheets = new TimeSheetsPanel();
					currentpanel = TimeSheets;
					this.Controls.Add(TimeSheets);
					TimeSheets.Visible = true;
					break;
				case MainScreen.ModuleType.ApprovedSearch:
					ApprovedSearch = new ApproveTimeSheetSearch();
					this.Controls.Add(ApprovedSearch);
					currentpanel = ApprovedSearch;
					ApprovedSearch.Visible = true;
					break;
				case MainScreen.ModuleType.EmployeeCRUD:
					EmployeeCRUD = new EmployeeCRUDPanel();
					this.Controls.Add(EmployeeCRUD);
					currentpanel = EmployeeCRUD;
					EmployeeCRUD.Visible = true;
					break;
				case MainScreen.ModuleType.ProjectCRUD:
					ProjectCRUD = new ProjectCRUDPanel();
					this.Controls.Add(ProjectCRUD);
					currentpanel = ProjectCRUD;
					ProjectCRUD.Visible = true;
					break;
				case MainScreen.ModuleType.ClientCRUD:
					ClientCRUD = new ClientCRUDPanel();
					this.Controls.Add(ClientCRUD);
					currentpanel = ClientCRUD;
					ClientCRUD.Visible = true;
					break;
				case MainScreen.ModuleType.TypeCRUD:
					TypeCRUD = new TypeCRUDPanel();
					this.Controls.Add(TypeCRUD);
					currentpanel = TypeCRUD;
					TypeCRUD.Visible = true;
					break;
				case MainScreen.ModuleType.ApproveTimeSheets:
					ApproveTS = new ApproveTimeSheets();
					this.Controls.Add(ApproveTS);
					currentpanel = ApproveTS;
					ApproveTS.Visible = true;
					break;
				case MainScreen.ModuleType.EmployeeSummaryReport:
					EmpSummaryReport = new RPT_EmployeeTimeSummary();
					this.Controls.Add(EmpSummaryReport);
					currentpanel = EmpSummaryReport;
					EmpSummaryReport.Visible = true;
					break;
				case MainScreen.ModuleType.ProjectSummaryReport:
					ProjectSummaryReport = new RPT_ProjectTimeSummary();
					this.Controls.Add(ProjectSummaryReport);
					currentpanel = ProjectSummaryReport;
					ProjectSummaryReport.Visible = true;
					break;
			}
		}
		#endregion 

		#region Windows Standard Designer and Startup Code

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 709);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(1085, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 0;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 760;
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.Name = "statusBarPanel2";
            // 
            // MainScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(850, 583);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainScreen";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEMPO Time Sheet Management";
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new MainScreen());
		}

		#endregion
	
	}
}
