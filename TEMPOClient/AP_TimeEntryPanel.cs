using System;
using System.Windows.Forms;
using TEMPO.Client.UIElements;
using System.Drawing;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client {
	/// <summary>
	/// Time Entry Panel for Approving Time Sheets
	/// </summary>
	public class AP_TimeEntryPanel : SubPanel {
	
		#region Member Declaration
		
		private Label totaltime,fullname,temp;
		private Label[] days;
		private ComboBox status;
		private AP_LineItemsPanel _lineItemsPanel;
		private TimeSheetDS.TimeSheetRow _timesheetrow;
		
		#endregion

		#region Public Initiatlization

		public AP_TimeEntryPanel(): base(new Point(24,109), new Size(771,268), "AP_TimeEntryPanel", false) {
			
			// UI init
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.ap_timeentrypanel.png"));
			
			// status
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(393,241), temp, "95", true, false,SubPanel.std_fontsize);

			// total
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(625,241), temp, "96", true, false,SubPanel.std_fontsize);
			
			days = new Label[7];
			
			// days of the week - Sunday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(376,11), temp, "97", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(386,11), temp, true, false,SubPanel.std_fontsize);
			days[0] = temp;
			
			// days of the week - Monday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(418,11), temp, "98", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(428,11), temp, true, false,SubPanel.std_fontsize);
			days[1] = temp;

			// days of the week - Tuesday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(466,11), temp, "99", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(476,11), temp, true, false,SubPanel.std_fontsize);
			days[2] = temp;

			// days of the week - Wednesday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(514,11), temp, "100", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(528,11), temp,true, false,SubPanel.std_fontsize);
			days[3] = temp;

			// days of the week - Thursday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(558,11), temp, "101", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(568,11), temp, true, false,SubPanel.std_fontsize);
			days[4] = temp;

			// days of the week - Friday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(602,11), temp, "102", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(612,11), temp, true, false,SubPanel.std_fontsize);
			days[5] = temp;

			// days of the week - Saturday
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(648,11), temp, "103", true, false,SubPanel.std_fontsize);
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.Yellow;
			temp.Size = new Size(20,15);
			this.addControl(new Point(658,11), temp, true, false,SubPanel.std_fontsize);
			days[6] = temp;

			// total time label
			totaltime = new Label();
			totaltime.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			this.addLabel(new Point(685,241), totaltime, false,false,SubPanel.std_fontsize);
	
			// fullname label
			fullname = new Label();			
			fullname.BackColor = System.Drawing.Color.FromArgb(199,184,173);
			fullname.ForeColor = System.Drawing.Color.FromArgb(110,70,58);
			fullname.Size = new Size(300,23);
			this.addControl(new Point(13,9), fullname, true, false,12);

			// add the status combo box
			status = new ComboBox();
			status.Size = new Size(145,20);
			status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(450,238), status, false,false,SubPanel.std_fontsize);

		}

		#endregion

		#region Public UI Methods


		/// <summary>
		/// End the updating for this panel
		/// </summary>
		public void EndUpdating() {
			BindingManagerBase mgr = this.BindingContext[_timesheetrow]; 
			mgr.EndCurrentEdit();	
		}

		/// <summary>
		/// Reset the Panel to it original state
		/// </summary>
		public void ResetPanel() {
			// remove the data bindings
			status.DataBindings.Clear();
			// release the timesheet
			_timesheetrow = null;
			// clear the line items panel
			this.Controls.Remove(_lineItemsPanel);
		}
	

		/// <summary>
		/// Populate a Time Entry Panel with a specific TimeSheet
		/// </summary>
		/// <param name="row"></param>
		public void PopulateTimeEntryForm(TimeSheetDS.TimeSheetRow row) {

			// keep the databinding in a local variable for later
			_timesheetrow = row;

			// build the status list based on a proxy call
			status.DataSource = TEMPOServerProxy.Instance.GetStatusChange(_timesheetrow).Status;
			status.DisplayMember = "StatusName";
			status.ValueMember = "StatusID";
			
			// add the appropriate data binding to the status combo box
			Binding b = new Binding("SelectedValue", _timesheetrow, "StatusID");	
			status.DataBindings.Add(b);

			// populate the fullname
			fullname.Text = _timesheetrow.EmployeeName;

			// update the date/time values for the timesheet
			System.DateTime periodending = _timesheetrow.EndingDate;
			for (int i = (days.Length-1); i >= 0; i--) {
				days[i].Text = " " + periodending.Day.ToString();
				periodending = periodending.AddDays(-1);
			}

			// create the line items panel and pass in an array of time entry rows
			_lineItemsPanel = new AP_LineItemsPanel(_timesheetrow);
			this.Controls.Add(_lineItemsPanel);

			// update the total time
			updateTotal();
		}

		/// <summary>
		/// Event Handler for when a line item gets updated
		/// </summary>
		public void updateTotal () {
			decimal total =0;
			// determine the Total
			TimeSheetDS.TimeEntryRow[] rows =  _timesheetrow.GetTimeEntryRows();
			for (int i=0; i< rows.Length; i++) {
				total += rows[i].Sunday;
				total += rows[i].Monday;
				total += rows[i].Tuesday;
				total += rows[i].Wednesday;
				total += rows[i].Thursday;
				total += rows[i].Friday;
				total += rows[i].Saturday;
			}
			totaltime.Text = String.Format("{0:N}", total);
		}

		#endregion

		
	}
}
