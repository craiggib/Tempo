using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Data;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;
using TEMPO.Authorization;

namespace TEMPO.Client {
	/// <summary>
	/// Windows Form Interface for displaying and managing TimeSheets
	/// </summary>
	public class TimeSheetsPanel : SubPanel {

		#region Member Declaration

		private ComboBox current_timesheets;
		private GraphicButton notes, remove, save, create,open;
		private Label temp;
		private TimeEntryPanel TimeEntry;
		private TimeSheetDS _timesheets;
		private TimeSheetDS.TimeSheetRow _opents;
		
		#endregion

		#region Public Initailization

		public TimeSheetsPanel() : base(new Point(20,140), new Size(920,430), "TimeSheets", false) {

			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesheetspanel.png"));

			// add the icon to the background panel
			PictureBox icon = new PictureBox();
			icon.Image = new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesheeticon.png");
			icon.Location = new Point(652,11);
			icon.Size = new Size(136,36);
			this.Controls.Add(icon);

			// add the timesheets combo box
			current_timesheets = new ComboBox();
			current_timesheets.Size = new Size(165,20);
			current_timesheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(155,53), current_timesheets, false,false,SubPanel.std_fontsize);				
			
			// notes buton
			notes = new GraphicButton(new Point(37,387), new Size(59,15));
			notes.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.notesbutton.png"));
			notes.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.notesbutton-over.png"));			
			notes.Click += new System.EventHandler(openNotes);
			this.Controls.Add(notes);

			// remove timesheet buton
			remove = new GraphicButton(new Point(106,387), new Size(118,15));
			remove.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.removetimesheetbutton.png"));
			remove.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.removetimesheetbutton-over.png"));
			remove.Click += new System.EventHandler(RemoveTimeSheet);
			this.Controls.Add(remove);

			// save buton
			save = new GraphicButton(new Point(698,387), new Size(59,15));
			save.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.savebutton.png"));
			save.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.savebutton-over.png"));			
			save.Click += new System.EventHandler(saveTimeSheet);
			this.Controls.Add(save);

			// open button
			open = new GraphicButton(new Point(327,55), new Size(61,17));
			open.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.tsopen.png"));
			open.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.tsopenover.png"));
			open.Click += new System.EventHandler(OpenTimeSheet);
			this.Controls.Add(open);
			
			// create button
			create = new GraphicButton(new Point(400,55), new Size(61,17));
			create.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.tsnew.png"));
			create.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.tsnewover.png"));
			create.Click += new System.EventHandler(createTimeSheet);
			this.Controls.Add(create);
			
			// saved time sheets
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(172,149,142);
			temp.ForeColor = System.Drawing.Color.White;
			this.addLabel(new Point(43,57), temp, "4", true, false,SubPanel.std_fontsize);

			// load the saved timesheets
			loadSavedTimeSheets();

			hideTimeSheetButtons();

			// build the time entry panel
			TimeEntry = new TimeEntryPanel();
			TimeEntry.Visible=false;
			this.Controls.Add(TimeEntry);
		}

		#endregion
	
		#region UI Helper Functions

		/// <summary>
		/// Populate the Save Time Sheets Combo Box
		/// </summary>
		private void loadSavedTimeSheets() {
			// get the timesheet data
			TimeSheetDS ds = TEMPOServerProxy.Instance.GetSavedRejectedTimeSheets(((TEMPOIdentity)Thread.CurrentPrincipal.Identity).UserID);
			_timesheets = ds;
			
			// set up the combo box
			current_timesheets.DataSource = _timesheets;
			current_timesheets.DisplayMember = "TimeSheet.TimeSheetName";
			current_timesheets.ValueMember = "TimeSheet.TID";
			
		}

		private void hideTimeSheetButtons() {
			notes.Visible = false;
			remove.Visible = false;
			save.Visible = false;
		}

		private void showTimeSheetButtons() {
			notes.Visible = true;
			remove.Visible = true;
			save.Visible = true;
		}

		#endregion

		#region UI Event Handlers

		/// <summary>
		/// Event Handler for Remove TimeSheet Button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RemoveTimeSheet ( object sender, EventArgs e) {
			// prompt to make sure
			DialogResult dr = MessageBox.Show(this.getStringResource("115"), this.getStringResource("114"),MessageBoxButtons.OKCancel);
			if (dr == DialogResult.OK) {
				// stop updating
				TimeEntry.EndUpdating();

				// mark the timesheet for deletion
				_opents.Delete();
				
				try {
					TEMPOServerProxy.Instance.DeleteTimeSheet(_timesheets);
				}
				catch {
					MessageBox.Show(this.getStringResource("117"));			
					return;
				}
				//display the success message
				MessageBox.Show(this.getStringResource("116"));
			
				// then reload the timesheet combobox
				loadSavedTimeSheets();	

				// hide the time sheet buttons
				hideTimeSheetButtons();
			
				// make the panel go away for now
				TimeEntry.Visible = false;

				// remove the reference to the open timesheet
				_opents = null;
			}
			
		}
		
		/// <summary>
		/// Event Handler for Create New Time Sheets Button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void createTimeSheet ( object sender, EventArgs e) {
			// open the form to create a new timesheet
			CreateNewTimeSheet cnts = new CreateNewTimeSheet();
			cnts.ShowDialog(this);
			// after we have made a call to this form, repopulate the drop down box
			loadSavedTimeSheets();
		}

		/// <summary>
		/// Event Handler for Open Notes button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openNotes ( object sender, EventArgs e) {
			// create the notes form
			TimeSheetNotesForm tsnf = new TimeSheetNotesForm(_opents);
			tsnf.ShowDialog(this);			
		}

		/// <summary>
		/// Send the updated Timesheets to the server
		/// </summary>
		private void saveTimeSheet(object sender, EventArgs e) {
			TimeEntry.EndUpdating();
			
			try {
				TEMPOServerProxy.Instance.UpdateTimeSheet(_timesheets);
				
			}
			catch {
				MessageBox.Show(this.getStringResource("18"));			
				return;
			}
			//display the success message
			MessageBox.Show(this.getStringResource("21"));
			
			// then reload the timesheet combobox
			loadSavedTimeSheets();	

			// hide the time sheet buttons
			hideTimeSheetButtons();
			
			// make the panel go away for now
			TimeEntry.Visible = false;

			// remove the reference to the open timesheet
			_opents = null;
					
		}

		/// <summary>
		/// Open a Timeheet based on the selected index of the combo box
		/// </summary>
		private void OpenTimeSheet (object sender, EventArgs e) {
			if (current_timesheets.SelectedIndex != -1) {
				// get the TimeSheet Row
				_opents = _timesheets.TimeSheet.FindByTID((int)(current_timesheets.SelectedValue));

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

		#endregion

	}
}
