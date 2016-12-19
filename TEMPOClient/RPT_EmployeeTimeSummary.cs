using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client {
	
	/// <summary>
	/// Report Panel for Employee Time Summary
	/// </summary>
	public class RPT_EmployeeTimeSummary: SubPanel {

		#region Member Declaration

		private ComboBox employees;
		private DateTimePicker frompicker, topicker;
		private GraphicButton search;

		#endregion

		#region Public Initialization

		public RPT_EmployeeTimeSummary() : base(new Point(20,140), new Size(820,430), "RPT_EmployeeTimeSummary", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesummarybyemp.png"));

			// From Date
			Label temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(46,132), temp, "111", true, false,SubPanel.std_fontsize);	
			
			// From Value
			frompicker = new DateTimePicker();
			frompicker.Size = new Size(130,20);
			frompicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.addControl(new Point(147,131), frompicker, false,false,SubPanel.std_fontsize);

			// To Date
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(46,172), temp, "112", true, false,SubPanel.std_fontsize);	

			// To Value
			topicker = new DateTimePicker();
			topicker.Size = new Size(130,20);
			topicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.addControl(new Point(147,173), topicker, false,false,SubPanel.std_fontsize);
			
			// employee list
			employees = new ComboBox();
			employees.Size = new Size(130,120);
			employees.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addControl(new Point(147,211), employees,false,false,SubPanel.std_fontsize);
			employees.DataSource = RequestBroker.TEMPOServerProxy.Instance.GetAllEmployees().Employee;
			employees.DisplayMember = "EmployeeName";
			employees.ValueMember = "EmpID";
			
			// Employee Label
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(46,213), temp, "113", true, false,SubPanel.std_fontsize);

			// the search button
			search = new GraphicButton(new Point(716,386), new Size(59,15));
			search.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchbutton.png"));
			search.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchbutton-over.png"));
            this.Controls.Add(search);
			search.Click += new System.EventHandler(GenerateReport);

			// report preview
			PictureBox preview = new PictureBox();
			preview.Image = new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.timesummarypreview.png");
			preview.Location = new Point(371,124);
			preview.Size = new Size(400,162);
			this.Controls.Add(preview);

			
		}

		#endregion

		#region Private UI Event Handlers

		private void GenerateReport(object sender, EventArgs e) {
			string filename = RequestBroker.TEMPOServerProxy.Instance.GetEmployeeTimeSummaryReport((int)employees.SelectedValue,frompicker.Value, topicker.Value);
			System.Diagnostics.Process.Start(filename);
		}

		#endregion

	}
}
