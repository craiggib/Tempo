using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.BusinessEntity;
using TEMPO.Client.UIElements;

namespace TEMPO.Client
{
	/// <summary>
	/// Line Item Container for Approving Time Sheets
	/// </summary>
	public class AP_LineItemsPanel : SubPanel {
				
		#region Member Declaration

		private static int y_separation = 30;
		private static int x_gutter = 2;
		private int y_cursor = 2;
		private int m_itemcursor = 0;
		private static int LINE_ITEM_MAX = 25;
		private AP_LineItemPanel[] m_lineitems;
		private TimeSheetDS.TimeSheetRow _timesheetrow;
		
		#endregion

		#region Public Initialization

		public AP_LineItemsPanel(TimeSheetDS.TimeSheetRow row): base (new Point(8,36), new Size(758,192), "LineItemsPanel", true ) {
			// store a local reference to the timesheet data
			_timesheetrow = row;
			// setup the UI
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			// set up the memory for the UI
			m_lineitems = new AP_LineItemPanel[LINE_ITEM_MAX];
			// populate the form
			PopulateLineItems();
		}

		#endregion
		
		#region Private UI Helper Methods

		/// <summary>
		/// Add a line item for every time entry row
		/// </summary>
		/// <param name="rownumber"></param>
		private void PopulateLineItems() {
			// check to see if the rownumber we are adding is above the limit or not
			if (m_itemcursor >= LINE_ITEM_MAX) 
				MessageBox.Show(getStringResource("17"));
			else {
				// populate each line item row
				for (int i = 0; i< _timesheetrow.GetTimeEntryRows().Length; i++)				
					AddRowToPanel(_timesheetrow.GetTimeEntryRows()[i]);
			}
		}

		/// <summary>
		/// Add a row to the line items panel based on a timesheet.timeentry row
		/// </summary>
		/// <param name="row"></param>
		private void AddRowToPanel(TimeSheetDS.TimeEntryRow row) {
			// check to see we haven't exceeded the limit for rows
			if ( m_itemcursor == LINE_ITEM_MAX) {
				MessageBox.Show(getStringResource("17"));
			}
			else {
				// build the ui object
				AP_LineItemPanel li = new AP_LineItemPanel();
				// and update hte current user interface
				m_lineitems[m_itemcursor++] = li;
				li.Location = new Point(x_gutter, y_cursor);
				// add it to the panel
				this.Controls.Add(li);
				y_cursor += y_separation;
				// declare the event handler for when input is added in the line item entry
				li.PopulateLineItem(row);
			}
		}

		#endregion

	}
}
