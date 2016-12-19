using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client
{
	/// <summary>
	/// Approval Time Entry Line item Panel
	/// </summary>
	public class AP_LineItemPanel: SubPanel {

		#region Member Declaration

		private ComboBox _project, _worktype;
		private Label mon,tue,wed,thu,fri,sat,sun,_client;
		private Label lbl_total;
		private TimeSheetDS.TimeEntryRow _timeentryrow;
		//private ClientDS _clientdata;
		private WorkTypeDS _worktypedata;
		private ProjectDS _projectdata;

		#endregion	

		#region Public Initialization

		public AP_LineItemPanel(): base (new Point(0,0), new Size(724,28), "LineItem", false) {

			// get the client lookup datea
			//_clientdata = TEMPOServerProxy.Instance.GetAllClients();
			// get the project lookup datea
			_projectdata = TEMPOServerProxy.Instance.GetProjectsList();
			// get all the worktype data
			_worktypedata = TEMPOServerProxy.Instance.GetAllWorkTypes();

			// initalize the user interface
			UserInterfaceInitalization();

		}
		# endregion

		#region Private UI Helper Methods 


		/// <summary>
		/// Determine the total value of the weeks worth of entered values
		/// </summary>
		/// <returns></returns>
		private decimal WeekTotal () {
			decimal total = 0;
			// use the is null property to make sure we aren't trying to add up nulls
			// for each on that isn't null, add it to the running total
			total += _timeentryrow.Sunday;
			total += _timeentryrow.Monday;
			total += _timeentryrow.Tuesday;
			total += _timeentryrow.Wednesday;
			total += _timeentryrow.Thursday;
			total += _timeentryrow.Friday;
			total += _timeentryrow.Saturday;
			return total;
		}

		/// <summary>
		/// Build the User Interface
		/// </summary>
		private void UserInterfaceInitalization() {
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.lineitementrypanel.png"));
		
			// client combo box
			//_client = new Label();
			//_client.Size = new System.Drawing.Size(130, 21);
			//this.addControl(new Point(162,2), _client,false,false,SubPanel.std_fontsize);
			
			// project combo box
			_project = new ComboBox();
			_project.Size = new System.Drawing.Size(130, 21);
			_project.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(32,2), _project,false,false,SubPanel.std_fontsize);
			_project.Width= 240;

			//worktype combo box
			_worktype = new ComboBox();
			_worktype.Size = new System.Drawing.Size(70, 21);
			_worktype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(292,2),_worktype,false,false,SubPanel.std_fontsize);

			// total label
			lbl_total = new Label();
			lbl_total.BackColor = System.Drawing.Color.FromArgb(199,184,173);
			this.addLabel(new Point(680, 7), lbl_total, false, false, SubPanel.std_fontsize);

			// days
			sun = new Label();
			sun.Size = new Size(40,21);
			sun.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(365, 7), sun, false,false,SubPanel.std_fontsize);
			
			mon = new Label();
			mon.Size = new Size(40,21);
			mon.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(409, 7), mon, false,false,SubPanel.std_fontsize);

			tue = new Label();
			tue.Size = new Size(40,21);
			tue.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(455, 7), tue, false,false,SubPanel.std_fontsize);

			wed = new Label();
			wed.Size = new Size(40,21);
			wed.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(504, 7), wed, false,false,SubPanel.std_fontsize);

			thu = new Label();
			thu.Size = new Size(40,21);
			thu.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(548, 7), thu, false,false,SubPanel.std_fontsize);

			fri = new Label();
			fri.Size = new Size(40,21);
			fri.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(592, 7), fri, false,false,SubPanel.std_fontsize);

			sat = new Label();
			sat.Size = new Size(40,21);
			sat.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			this.addLabel(new Point(636, 7), sat, false,false,SubPanel.std_fontsize);
		}

		#endregion

		#region Public UI Methods

		/// <summary>
		/// Populate the line item based on the reference to the time entry data
		/// </summary>
		public void PopulateLineItem(TimeSheetDS.TimeEntryRow row) {
			// store a local reference to the data
			_timeentryrow = row;

			// bind to the project time entry data
			_project.DataSource = _projectdata.Project;
			_project.DisplayMember = "ProjectName";
			_project.ValueMember = "ProjectID";	
			Binding bind = new Binding("SelectedValue", _timeentryrow, "ProjectID");
			_project.DataBindings.Add(bind);
			
			// bind to the worktype entry data
			_worktype.DataSource = _worktypedata.WorkType;
			_worktype.DisplayMember = "WorkTypeName";
			_worktype.ValueMember = "WorkTypeID";
			bind = new Binding("SelectedValue", _timeentryrow, "WorkTypeID");
			_worktype.DataBindings.Add(bind);
			
			// bind the time entry data to the days of the week
			sun.DataBindings.Add("Text", _timeentryrow, "Sunday");
			sat.DataBindings.Add("Text", _timeentryrow, "Saturday");
			fri.DataBindings.Add("Text", _timeentryrow, "Friday");
			thu.DataBindings.Add("Text", _timeentryrow, "Thursday");
			wed.DataBindings.Add("Text", _timeentryrow, "Wednesday");
			tue.DataBindings.Add("Text", _timeentryrow, "Tuesday");
			mon.DataBindings.Add("Text", _timeentryrow, "Monday");

			// update the display with the appropriate project item
			if (_timeentryrow.ProjectID != -1) _project.SelectedIndex = _project.FindStringExact(_projectdata.Project.FindByProjectID(_timeentryrow.ProjectID).ProjectName);
			// update the work type with the appropriate item
			if (_timeentryrow.WorkTypeID != -1) _worktype.SelectedIndex = _worktype.FindStringExact(_worktypedata.WorkType.FindByWorkTypeID(_timeentryrow.WorkTypeID).WorkTypeName);
			// upate the client name
			//if (_timeentryrow.ClientID != -1) _client.Text = _clientdata.Client.FindByClientID(_timeentryrow.ClientID).ClientName;

			// update the total
			lbl_total.Text = WeekTotal().ToString();
		}
		#endregion

	}
}
