using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.Authorization;

namespace TEMPO.Client {
	/// <summary>
	/// Windows Form Control for creating new Time Sheets
	/// </summary>
	public class CreateNewTimeSheet : System.Windows.Forms.Form {
	
		# region Member Declaration

		private System.Windows.Forms.Label pelabel;
		private GraphicButton open;
		private System.ComponentModel.Container components = null;
		private ComboBox periodending_list;

		#endregion

		# region Public Initialization

		public CreateNewTimeSheet() {
			InitializeComponent();

			this.BackgroundImage = new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.modalwindow-createts.png");

			pelabel = new Label();
			pelabel.Location = new Point(37,103);
			pelabel.Font = new Font(SubPanel.std_fontname, SubPanel.std_fontsize);
			pelabel.Text = "Period Ending";
			pelabel.Size = new Size(80,16);
			pelabel.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			this.Controls.Add(pelabel);

			// attach the open graphic to the panel
			open = new GraphicButton(new Point(282,208), new Size(59,15));
			open.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.addtolistbutton.png"));
			open.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.addtolistbutton-over.png"));			
			open.Click += new System.EventHandler(open_Click);
			this.Controls.Add(open);
			
			// assign the form icon
			this.Icon = new Icon(typeof(TEMPO.Client.MainScreen), "Resources.trayicon.ico");
			
			// load the data from the proxy client
			periodending_list = new System.Windows.Forms.ComboBox();
			periodending_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			periodending_list.Location = new System.Drawing.Point(141, 102);
			periodending_list.Size = new System.Drawing.Size(176, 21);
            periodending_list.DataSource = TEMPO.RequestBroker.TEMPOServerProxy.Instance.GetMonthPeriodEndingNotCompleted(((TEMPOIdentity)Thread.CurrentPrincipal.Identity).UserID).PeriodEnding;
			periodending_list.DisplayMember = "EndingDate";
			periodending_list.ValueMember = "PEID";
			periodending_list.Font = new Font(SubPanel.std_fontname, SubPanel.std_fontsize);
			this.Controls.Add(periodending_list);
			
			// change the size of this form
			this.ClientSize = new System.Drawing.Size(374, 254);
		}

		#endregion

		#region Windows Form Designer generated code

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
			// 
			// CreateNewTimeSheet
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(398, 236);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateNewTimeSheet";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TEMPO [Create New Time Sheet]";

		}
		#endregion

		#region UI Event Handlers

		/// <summary>
		/// Event Handler for Open Button
		/// Creates a Timesheet messages, sends it to the server and then closes the window
		/// </summary>
		private void open_Click(object sender, System.EventArgs e) {
			// build the new entity
			TimeSheetDS ds = new TimeSheetDS();
			TimeSheetDS.TimeSheetRow row = ds.TimeSheet.NewTimeSheetRow();
			row.PEID = (int) periodending_list.SelectedValue;
			row.EmpID = ((TEMPOIdentity)Thread.CurrentPrincipal.Identity).UserID;

			// add the row to the dataset
			ds.TimeSheet.AddTimeSheetRow(row);
			TEMPO.RequestBroker.TEMPOServerProxy.Instance.CreateTimeSheet(ds);
			
			// close the window
			this.Close();
            
		}
		#endregion

	}
}
