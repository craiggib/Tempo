
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client {
	/// <summary>
	/// User Interface Form to permit line item entry
	/// </summary>
	/// <remarks>
	/// CHANGE HISTORY:
	/// ----------------------------------------------------------------------------------
	/// April 19, 2004 -- Removed Client Label and Extended size of input box
	/// </remarks>
	public class LineItem:SubPanel {
		
		#region Member Declaration

		private ComboBox _project, _worktype;
		private TextBox mon,tue,wed,thu,fri,sat,sun;
		private Label lbl_total; //,client;
		private GraphicButton removerow;
		private TimeSheetDS.TimeEntryRow _timeentryrow;
		//private ClientDS _clientdata;
		private WorkTypeDS _worktypedata;
		private ProjectDS _projectdata;

		#endregion

		#region Public Inialization

		public LineItem(): base (new Point(0,0), new Size(724,28), "LineItem", false) {
			// get the client lookup datea
			//_clientdata = TEMPOServerProxy.Instance.GetAllClients();
			// get the project lookup datea
			_projectdata = TEMPOServerProxy.Instance.GetProjectsList();
			// get all the worktype data
			_worktypedata = TEMPOServerProxy.Instance.GetAllWorkTypes();
			
			// initialize the user interface
			UserInterfaceInitialization();
		}
		#endregion

		#region Delegate Declarations

		// create the delegates and events to monitor this objects existance
		public delegate void LineItemRemoveDelegate(LineItem l);
		public event LineItemRemoveDelegate onLineItemRemove;

		// create the delegates and events to monitor this object
		public delegate void LineItemRowTotalChangedDelegate();
		public event LineItemRowTotalChangedDelegate onLineItemRowTotalChange;

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
			if (_timeentryrow.ProjectID != -1) _project.SelectedIndex = _project.FindStringExact(_projectdata.Project.FindByProjectID(_timeentryrow.ProjectID).RefJobNum);
			// update the work type with the appropriate item
			if (_timeentryrow.WorkTypeID != -1) _worktype.SelectedIndex = _worktype.FindStringExact(_worktypedata.WorkType.FindByWorkTypeID(_timeentryrow.WorkTypeID).WorkTypeName);
			// upate the client name
			//if (_timeentryrow.ClientID != -1) client.Text = _clientdata.Client.FindByClientID(_timeentryrow.ClientID).ClientName;

			// update the total
			lbl_total.Text = WeekTotal().ToString();			
		}

		#endregion

		#region Private UI Event Handlers

		/// <summary>
		/// Remove a line item 
		/// Mark the data for deletion and make a call to the delegate to update the UI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void removeRowClick(object sender, EventArgs e) {
			DialogResult dr = MessageBox.Show(this.getStringResource("22"), this.getStringResource("23"),MessageBoxButtons.OKCancel);
			if (dr == DialogResult.OK) {
				// mark the row for deletion and remove this line item from the UI
				_timeentryrow.Delete();
				// notify the subscribers so that the UI can get updated
				onLineItemRemove(this);
			}
		}

		/// <summary>
		/// Event Handler for Project Drop Down change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void projectChange (object sender, EventArgs e)  {
			// update the timeentry row with the new value
			if (_project.SelectedIndex != -1) {
				//client.Text = _clientdata.Client.FindByClientID(_projectdata.Project.FindByProjectID((int)_project.SelectedValue).ClientID).ClientName;
			}
			
		}

		
		/// <summary>
		/// Event handler for the hour inputs - validates correct input
		/// Triggers call to TimeEntryPanel
		/// </summary>
		private void timeEntryChange ( object sender, EventArgs e){
			// update the UI with the total
			lbl_total.Text = WeekTotal().ToString();	
			onLineItemRowTotalChange();
		}

		#endregion

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

		private void UserInterfaceInitialization() {

			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.lineitementrypanel.png"));
	
			//removerow button
			removerow = new GraphicButton(new Point(2,3), new Size(27,19));
			removerow.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.removerowbutton.png"));
			removerow.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.removerowbutton-over.png"));			
			removerow.Click += new System.EventHandler(removeRowClick);
			this.Controls.Add(removerow);

			// Client Label Removed April 19, 2004 as per Client Request
			// client text box			
//			client = new Label();
//			client.Size = new System.Drawing.Size(130, 21);
//			client.BackColor = System.Drawing.Color.FromArgb(221,212,206);
//			this.Controls.Add(client);
//			client.Location = new Point(162,2);
//			client.Font = new Font(SubPanel.std_fontname, SubPanel.std_fontsize);
			
			// project combo box
			_project = new ComboBox();
			this.addControl(new Point(32,2), _project,false,false,SubPanel.std_fontsize);
			_project.Size = new System.Drawing.Size(130, 21);
			_project.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			_project.Width = 240;
			

			//worktype combo box
			_worktype = new ComboBox();
			this.addControl(new Point(280,2),_worktype,false,false,SubPanel.std_fontsize);
			_worktype.Size = new System.Drawing.Size(70, 21);
			_worktype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;		
			_worktype.Width = 80;

			// total label
			lbl_total = new Label();
			lbl_total.BackColor = System.Drawing.Color.FromArgb(199,184,173);
			this.addLabel(new Point(680, 7), lbl_total, false, false, SubPanel.std_fontsize);
			
			// days
			sun = new TextBox();
			sun.Tag = 0;
			sun.Size = new Size(40,21);
			sun.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(365, 2), sun, false,false,SubPanel.std_fontsize);
			
			
			mon = new TextBox();
			mon.Tag = 1;
			mon.Size = new Size(40,21);
			mon.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(409, 2), mon, false,false,SubPanel.std_fontsize);
		

			tue = new TextBox();
			tue.Tag = 2;
			tue.Size = new Size(40,21);
			tue.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(453, 2), tue, false,false,SubPanel.std_fontsize);
		
			wed = new TextBox();
			wed.Tag = 3;
			wed.Size = new Size(40,21);
			wed.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(497, 2), wed, false,false,SubPanel.std_fontsize);
		
			thu = new TextBox();
			thu.Tag = 4;
			thu.Size = new Size(40,21);
			thu.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(541, 2), thu, false,false,SubPanel.std_fontsize);	
		
			fri = new TextBox();
			fri.Tag = 5;
			fri.Size = new Size(40,21);
			fri.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(585, 2), fri, false,false,SubPanel.std_fontsize);
			
			sat = new TextBox();
			sat.Tag = 6;
			sat.Size = new Size(40,21);
			sat.Validated += new System.EventHandler(timeEntryChange);
			this.addControl(new Point(629, 2), sat, false,false,SubPanel.std_fontsize);				
	
			// add the listeners for the combo boxes		
			_project.SelectedValueChanged += new System.EventHandler(projectChange);
		}


		#endregion

	}
}

