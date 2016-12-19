using System;
using System.Drawing;
using System.Windows.Forms;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;

namespace TEMPO.Client
{
	/// <summary>
	/// Line Items UI for Search Results
	/// </summary>
	public class SearchResultLineItemPanel : SubPanel {
		
		#region Member Declaration

		private TimeSheetDS.TimeSheetRow _timesheetrow;
		private Label _employee,_status,_periodending;
		private GraphicButton folder;

		#endregion

		#region Public Declaration

		public SearchResultLineItemPanel() : base(new Point(0,0), new Size(385,28), "SearchResultLineItemPanel", true) {
			
			//background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchresultitempanel.png"));

			// Employee
			_employee = new Label();
			_employee.BackColor = System.Drawing.Color.White;
			_employee.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(40,7), _employee, false, false,SubPanel.std_fontsize);

			// Status
			_status = new Label();
			_status.BackColor = System.Drawing.Color.FromArgb(172,149,142);;
			_status.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(209,7), _status, false, false,SubPanel.std_fontsize);


			// Period Ending
			_periodending = new Label();
			_periodending.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			_periodending.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(302,7), _periodending, false, false,SubPanel.std_fontsize);
			
			// action item
			folder = new GraphicButton(new Point(6,6), new Size(19,16));
			folder.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.folderclosed.png"));
			folder.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.folderopen.png"));			
			folder.Click += new EventHandler(OpenReport);
			this.Controls.Add(folder);
		}

		#endregion

		#region Public UI Methods
		
		/// <summary>
		/// Populate the UI with the timesheet data
		/// </summary>
		/// <param name="row"></param>
		public void PopulateLineItem(TimeSheetDS.TimeSheetRow row) {
			// store a local pointer to the data
			_timesheetrow = row;

			// populate the line item
			_employee.Text = _timesheetrow.EmployeeName;
			_status.Text = _timesheetrow.StatusName;
			_periodending.Text = _timesheetrow.EndingDate.ToShortDateString();

		}

		#endregion

		#region Private Event Handlers

		/// <summary>
		/// Open the TimeSheet Report
		/// </summary>
		private void OpenReport(object sender, System.EventArgs args) {
			// make a call to the request broker for the report
			string filename = RequestBroker.TEMPOServerProxy.Instance.GetTimeSheetReport(_timesheetrow.TID);
			// display the report
			System.Diagnostics.Process.Start(filename);
		}
		
		#endregion

	}
}
