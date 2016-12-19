using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client {
	/// <summary>
	/// Report Search Panel for Time Summary by Project
	/// </summary>
	public class RPT_ProjectTimeSummary:SubPanel {
		
		#region Member Declaration

		private ComboBox projects;
		private DateTimePicker frompicker, topicker;
		private GraphicButton search;

		#endregion

		#region Public Initialization

		public RPT_ProjectTimeSummary()  : base(new Point(20,140), new Size(820,430), "RPT_ProjectTimeSummary", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesummarybyproject.png"));

			// From Date
			Label temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(46,125), temp, "118", true, false,SubPanel.std_fontsize);	
			
			// From Value
			frompicker = new DateTimePicker();
			frompicker.Size = new Size(130,20);
			frompicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.addControl(new Point(147,123), frompicker, false,false,SubPanel.std_fontsize);

			// To Date
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(46,165), temp, "119", true, false,SubPanel.std_fontsize);	

			// To Value
			topicker = new DateTimePicker();
			topicker.Size = new Size(130,20);
			topicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.addControl(new Point(147,163), topicker, false,false,SubPanel.std_fontsize);
			
			// project Label
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(46,206), temp, "120", true, false,SubPanel.std_fontsize);
			
			// project list
			projects = new ComboBox();
			projects.Size = new Size(130,120);
			projects.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(147,203), projects,false,false,SubPanel.std_fontsize);
			projects.DataSource = RequestBroker.TEMPOServerProxy.Instance.GetProjectsList().Project;
			projects.DisplayMember = "ProjectName";
			projects.ValueMember = "ProjectID";	
			projects.Width = 250;


			// the search button
			search = new GraphicButton(new Point(716,386), new Size(59,15));
			search.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchbutton.png"));
			search.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchbutton-over.png"));
			this.Controls.Add(search);
			search.Click += new System.EventHandler(GenerateReport);
			
		}
		#endregion

		#region Private UI Event Handlers

		private void GenerateReport(object sender, EventArgs e) {
			string filename = RequestBroker.TEMPOServerProxy.Instance.GetProjectTimeSummaryReport((int)projects.SelectedValue,frompicker.Value, topicker.Value);
			System.Diagnostics.Process.Start(filename);
		}

		#endregion
	}
}
