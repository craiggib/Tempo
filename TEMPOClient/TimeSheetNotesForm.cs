using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using TEMPO.BusinessEntity;
using TEMPO.Client.UIElements;

namespace TEMPO.Client {
	/// <summary>
	/// User Interface Elements for adding notes to a Timesheet
	/// </summary>
	public class TimeSheetNotesForm : System.Windows.Forms.Form {

		#region Member Declarations

		private System.Windows.Forms.RichTextBox m_notes;
		private GraphicButton m_save;
		private System.ComponentModel.Container components = null;
		private TimeSheetDS.TimeSheetRow _timesheetrow;

		#endregion

		#region Public Initalization

		public TimeSheetNotesForm(TimeSheetDS.TimeSheetRow tsrow) {
			InitializeComponent();

			_timesheetrow = tsrow;
					
			// build up the User Interface
			this.BackgroundImage = new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.modalwindow.png");
			this.ClientSize = new System.Drawing.Size(374, 254);

			m_save = new GraphicButton(new Point(282,208), new Size(59,15));
			m_save.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.savebutton.png"));
			m_save.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.savebutton-over.png"));			
			m_save.Click += new System.EventHandler(m_save_Click);
			this.Controls.Add(m_save);

			// assign the form icon
			this.Icon = new Icon(typeof(TEMPO.Client.MainScreen), "Resources.trayicon.ico");

			// populate the text box with the info
			if (_timesheetrow.IsNotesNull()) m_notes.Text = "";
			else m_notes.Text = _timesheetrow.Notes;
		}

		#endregion

		#region Windows Form Designer generated code
	
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_notes = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// m_notes
			// 
			this.m_notes.Location = new System.Drawing.Point(32, 40);
			this.m_notes.Name = "m_notes";
			this.m_notes.Size = new System.Drawing.Size(296, 152);
			this.m_notes.TabIndex = 0;
			this.m_notes.Text = "";
			// 
			// TimeSheetNotesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(426, 220);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_notes});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TimeSheetNotesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Time Sheet Notes";
			this.ResumeLayout(false);

		}
		#endregion

		#region UI Event Handlers

		/// <summary>
		/// Save the notes to the timesheet row and close the window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void m_save_Click(object sender, System.EventArgs e) {
			_timesheetrow.Notes = m_notes.Text;
			this.Close();
		}

		#endregion

	}
}
