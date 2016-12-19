using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for ApproveTimeSheets.
	/// </summary>
	public class ApproveTimeSheets: SubPanel {

		#region Member Declartion

		private Label temp;
		private ComboBox submitted_timesheets;
		private AP_TimeEntryPanel TimeEntry;
		private GraphicButton notes, save, open;
		private TimeSheetDS _timesheets;
		private TimeSheetDS.TimeSheetRow _opents;

		#endregion

		#region Public Intiatlization

		public ApproveTimeSheets(): base(new Point(20,140), new Size(820,430), "ApproveTimeSheets", false) {

			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.approvetimesheetspanel.png"));

			// subbmited time sheets
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(43,57), temp, "94", true, false,SubPanel.std_fontsize);

			// add the timesheets combo box
			submitted_timesheets = new ComboBox();
			submitted_timesheets.Size = new Size(165,20);
			submitted_timesheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(215,53), submitted_timesheets, false,false,SubPanel.std_fontsize);
			submitted_timesheets.Width = 230;
            
			// open button
			open = new GraphicButton(new Point(460,55), new Size(61,17));
			open.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.tsopen.png"));
			open.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.tsopenover.png"));
			open.Click += new System.EventHandler(OpenTimeSheet);
			this.Controls.Add(open);

			// notes buton
			notes = new GraphicButton(new Point(37,387), new Size(59,15));
			notes.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.notesbutton.png"));
			notes.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.notesbutton-over.png"));			
			notes.Visible = false;
			notes.Click += new System.EventHandler(OpenNotes);
			this.Controls.Add(notes);		

			// save buton
			save = new GraphicButton(new Point(698,387), new Size(59,15));
			save.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.savebutton.png"));
			save.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.savebutton-over.png"));			
			save.Click += new System.EventHandler(saveTimeSheet);
			save.Visible = false;
			this.Controls.Add(save);			

			loadSubmittedTimeSheets();

			// build the time entry panel
			TimeEntry = new AP_TimeEntryPanel();
			TimeEntry.Visible=false;
			this.Controls.Add(TimeEntry);
		}

		#endregion

		#region Private UI Event Handlers
		
		/// <summary>
		/// Send the updated Timesheets to the server
		/// </summary>
		private void saveTimeSheet(object sender, EventArgs e) {
			TimeEntry.EndUpdating();
			
			try {
				TEMPOServerProxy.Instance.ApprovalUpdate(_timesheets);				
			}
			catch (Exception exp) {
				MessageBox.Show(exp.ToString() + this.getStringResource("104"));			
				return;
			}
			//display the success message
			MessageBox.Show(this.getStringResource("105"));
			
			// then reload the timesheet combobox
			loadSubmittedTimeSheets();	

			// hide the time sheet buttons
			hideTimeSheetButtons();
			
			// make the panel go away for now
			TimeEntry.Visible = false;

			// remove the reference to the open timesheet
			_opents = null;
					
		}

		/// <summary>
		/// Open a TimeSheet based on the drop down menu selection
		/// </summary>
		private void OpenTimeSheet (object sender, EventArgs e) {
			if (submitted_timesheets.SelectedIndex != -1) {
				// get the TimeSheet Row
				_opents = _timesheets.TimeSheet.FindByTID((int)(submitted_timesheets.SelectedValue));

				// clear any existing data
				TimeEntry.ResetPanel();
				
				TimeEntry.PopulateTimeEntryForm(_opents);
				TimeEntry.Visible = true;
				// update the total at the bottom of the panel
				TimeEntry.updateTotal();

				// show the buttons			
				showTimeSheetButtons();
			}
		}


		/// <summary>
		/// Open the Notes Form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OpenNotes ( object sender, EventArgs e) {
			// create the notes form then open it in modal mode
			TimeSheetNotesForm tsnf = new TimeSheetNotesForm(_opents);
			tsnf.ShowDialog(this);			
		}
		#endregion


		/// <summary>
		/// Validate entries, then update the timesheet
		/// Checks Line Item validation, and status validation
		/// </summary>
//		private void saveTimeSheet(object sender, EventArgs e) {
//			// validate that a staus has been chosen
//			if (! TimeEntry.validateStatus() ) {
//				MessageBox.Show(this.getStringResource("107"));
//				return;
//			}
//			// validate that all the required data has been entered
//			if ( TimeEntry.validateLineItems() ) {
//				// build the Object for the Business Rules
//				TimeSheets ts = new TimeSheets();
//
//				// check to see if it is simply a status change
//				if (ts.isReject(current_timesheet) ) {
//					// warn of lost changes
//					DialogResult dr = MessageBox.Show(this.getStringResource("108"), this.getStringResource("109"), System.Windows.Forms.MessageBoxButtons.OKCancel);
//					if (dr == DialogResult.OK )	 {
//						// update the timesheet;
//						if ( current_timesheet.Update()) {		
//							// success!
//							MessageBox.Show(this.getStringResource("105"));
//
//							// now, we need to deal with the display list and panel
//							this.Controls.Remove(TimeEntry);
//							loadSubmittedTimeSheets();
//							save.Visible = false;
//							notes.Visible = false;
//						}
//						else {
//							MessageBox.Show(this.getStringResource("104"));
//						}
//					}
//				}
//				else {				
//					// we are approving this timesheet
//					try {
//						ts.AddAdminModifiedTimeSheetToSystem(current_timesheet);
//					}
//					catch (TimeSheets.TimeEntryCreationException) {
//						MessageBox.Show(this.getStringResource("104"));
//					}
//				
//					// success!
//					MessageBox.Show(this.getStringResource("105"));
//
//					// now, we need to deal with the display list and panel
//					this.Controls.Remove(TimeEntry);
//					loadSubmittedTimeSheets();
//					save.Visible = false;
//					notes.Visible = false;
//				}
//			}
//			else 
//				// error in input!
//				MessageBox.Show(this.getStringResource("106"));
//		}
	
		#region Private UI Helper Functions

		/// <summary>
		/// Populate the Save Time Sheets Combo Box
		/// </summary>
		private void loadSubmittedTimeSheets() {
			// get the timesheet data
			TimeSheetDS ds = TEMPOServerProxy.Instance.GetSubmittedTimeSheets();
			_timesheets = ds;
			
			// set up the combo box
			submitted_timesheets.DataSource = _timesheets;
			submitted_timesheets.DisplayMember = "TimeSheet.TimeSheetName";
			submitted_timesheets.ValueMember = "TimeSheet.TID";
		}

		
		private void hideTimeSheetButtons() {
			notes.Visible = false;
			save.Visible = false;
		}

		private void showTimeSheetButtons() {
			notes.Visible = true;
			save.Visible = true;
		}
		#endregion

	

	}
}
