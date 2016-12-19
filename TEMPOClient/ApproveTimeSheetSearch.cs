using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.RequestBroker;

namespace TEMPO.Client {
	/// <summary>
	/// UI Panel for Searching and viewing Approved Time Sheets
	/// </summary>
	public class ApproveTimeSheetSearch : SubPanel {

		#region Member Declaration

		private Label temp, employee;
		private GraphicButton search;
		private DateTimePicker frompicker, topicker;
		private SearchResultsPanel _resultpanel;

		#endregion
        
		#region Public Initialization

		public ApproveTimeSheetSearch() : base(new Point(20,140), new Size(820,430), "ApprovedTimeSheetSearch", false){			

			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.approvedsearchpanel.png"));

			// add the icon
			PictureBox icon = new PictureBox();
			icon.Image = new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchpanelicon.png");
			icon.Location = new Point(640,0);
			icon.Size = new Size(162,50);
			this.Controls.Add(icon);

			// Employee
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(40,80), temp, "24", true, false,SubPanel.std_fontsize);			

			// Employee Value
			employee = new Label();
			employee.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			employee.ForeColor = System.Drawing.Color.FromArgb(170,168,166);
			employee.Location = new Point(148,80);
			Font f = new Font(SubPanel.std_fontname,SubPanel.std_fontsize);
			f = new Font(f ,f.Style | FontStyle.Italic);
			f = new Font(f ,f.Style | FontStyle.Bold);
			employee.Font = f;
			employee.Size = new Size(135, 21);
			this.Controls.Add(employee);
			
			
			// Status
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(40,118), temp, "25", true, false,SubPanel.std_fontsize);

			// Status Value
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(221,212,206);
			temp.ForeColor = System.Drawing.Color.FromArgb(170,168,166);
			this.addLabel(new Point(148,118), temp, "28", true, true,SubPanel.std_fontsize);

			// From Date
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(40,158), temp, "26", true, false,SubPanel.std_fontsize);	
			
			// From Value
			frompicker = new DateTimePicker();
			frompicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			frompicker.Size = new Size(130,20);
			this.addControl(new Point(148,156), frompicker, false,false,SubPanel.std_fontsize);

			// To Date
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(40,198), temp, "27", true, false,SubPanel.std_fontsize);	

			// To Value
			topicker = new DateTimePicker();
			topicker .Format = System.Windows.Forms.DateTimePickerFormat.Short;
			topicker .Size = new Size(130,20);
			this.addControl(new Point(148,196), topicker , false,false,SubPanel.std_fontsize);

			// Employee Result
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(373,78), temp, "24", true, false,SubPanel.std_fontsize);		

			// Status Result
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(541,78), temp, "25", true, false,SubPanel.std_fontsize);

			// PE Result
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(205,191,187);
			temp.ForeColor = System.Drawing.Color.FromArgb(88,43,30);
			this.addLabel(new Point(635,78), temp, "29", true, false,SubPanel.std_fontsize);

			search = new GraphicButton(new Point(240,367), new Size(59,15));
			search.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchbutton.png"));
			search.setOverGraphicState(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.searchbutton-over.png"));			
			this.Controls.Add(search);
			search.Click += new System.EventHandler(onSearch);

			// build the result panel
			// don't display it to the user yet
			_resultpanel = new SearchResultsPanel();
			this.Controls.Add(_resultpanel);
			_resultpanel.Visible = false;

			// populate the search parameters
			PopulateSearchParameters();

		}

		#endregion

		#region Private UI Helper Methods

		private void PopulateSearchParameters() {
			// show the employee name
			employee.Text = Thread.CurrentPrincipal.Identity.Name;
		}
		#endregion

		#region Private Event Handlers

		/// <summary>
		/// Event Handler for the Search button
		/// </summary>
		private void onSearch(object sender, System.EventArgs e) {
			
			// verify that the from date is larger than the to date	
			// and that they aren't the same dates
			if ((DateTime.Compare(topicker.Value, frompicker.Value) < 1) ||
				(DateTime.Compare(topicker.Value, frompicker.Value) == 0)){
				MessageBox.Show(this.getStringResource("31"));
				return;
			}

			// populate the result set to the user
			_resultpanel.PopulateResults(TEMPOServerProxy.Instance.GetApprovedTimeSheets(frompicker.Value, topicker.Value));			
			// show the result panel
			_resultpanel.Visible = true;
			
		}
		#endregion
	}

}
