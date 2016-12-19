using System;
using System.Drawing;
using System.Windows.Forms;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;

namespace TEMPO.Client {
	/// <summary>
	/// Search Results Container
	/// </summary>
	public class SearchResultsPanel : SubPanel {		
		
		#region Member Declaration

		private int y_gutter = 5;
		private int x_gutter = 2;
		private int item_height = 28;
		private int current_y = 2;
		private SearchResultLineItemPanel lineitem;

		#endregion

		#region Public Initialization

		public SearchResultsPanel(): base(new Point(331,112), new Size(426,233), "SearchResultsPanel", true) {
			// set the background color
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);			
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Populate the panel with search line items given a timesheet data set
		/// </summary>
		/// <param name="timesheets">the result set</param>
		public void PopulateResults(TimeSheetDS timesheets) {
			// remove the existing search results line items
			this.Controls.Clear();
			// initialize the UI pixel counter
			current_y = 2;			
			// build out the results
			for (int i=0; i< timesheets.TimeSheet.Count; i++) {				
				lineitem = new SearchResultLineItemPanel();
				lineitem.Location = new Point(x_gutter,current_y);
				current_y += y_gutter + item_height;
				this.Controls.Add(lineitem);
				// populate the line item
				lineitem.PopulateLineItem(timesheets.TimeSheet[i]);
			}
		}
		#endregion

	}
}
