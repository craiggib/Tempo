using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.BusinessEntity;
using TEMPO.Client.UIElements;

namespace TEMPO.Client {
	/// <summary>
	/// Line item entry user interface for a timesheet
	/// </summary>
	public class LineItemsPanel : SubPanel {
	
		#region Member Declaration

		private static int y_separation = 30;
		private static int x_gutter = 2;
		private int y_cursor = 2;
		private int m_itemcursor = 0;
		private static int LINE_ITEM_MAX = 25;
		private TimeSheetDS.TimeSheetRow _timesheetrow;
		private LineItem[] m_lineitems;
		
		#endregion

		#region Public Initalization
        
		public LineItemsPanel(TimeSheetDS.TimeSheetRow row): base (new Point(8,36), new Size(758,192), "LineItemsPanel", true ) {
			// store a local reference to the timesheet data
			_timesheetrow = row;
			// setup the UI
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			// set up the memory for the UI
			m_lineitems = new LineItem[LINE_ITEM_MAX];
			// populate the form
			PopulateLineItems();
		}

		#endregion

		#region Private UI Helper Functions

		/// <summary>
		/// Updates the Total Time Value based on a line item changing
		/// </summary>
		private void LineItemTotalUpdated() {
			onTimeSheetTotalChange();
		}

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
					addRowToPanel(_timesheetrow.GetTimeEntryRows()[i]);
			}
		}

		#endregion

		#region Private UI Event Handlers

		/// <summary>
		/// Triggered when a line item is removed
		/// </summary>
		/// <param name="li"></param>
		private void LineItemRemoved(LineItem li) {
			// build the new line items array
			LineItem[] temp = new LineItem[m_lineitems.Length-1];
			int cursor =0;
			//remove this control from the list of line items
			for (int i=0;i<m_lineitems.Length;i++) {
				if (m_lineitems[i] == li) {
					// remove the item
					m_lineitems[i].Dispose();
				}
				else {
					temp[cursor++] = m_lineitems[i];
				}
			}
			// reassign the temp value to the current value
			m_lineitems = temp;
			// reset the cursor
			m_itemcursor--;
			// reset the visual params
			y_cursor = 2;
			// and redraw
			for (int i=0; i< m_itemcursor; i++) {
				m_lineitems[i].Location = new Point(x_gutter, y_cursor);
				y_cursor += y_separation;
			}
			// let the time entry form know about the change
			//onLineItemRemove(li);
		}

		/// <summary>
		/// Add a row to the line items panel based on a timesheet.timeentry row
		/// </summary>
		/// <param name="row"></param>
		private void addRowToPanel(TimeSheetDS.TimeEntryRow row) {
			// check to see we haven't exceeded the limit for rows
			if ( m_itemcursor == LINE_ITEM_MAX) {
				MessageBox.Show(getStringResource("17"));
			}
			else {
				// build the ui object
				LineItem li = new LineItem();
				// and update hte current user interface
				m_lineitems[m_itemcursor++] = li;
                li.Location = new Point(x_gutter, this.AutoScrollPosition.Y + y_cursor);
				// add it to the panel
                this.Controls.Add(li);
				y_cursor += y_separation;
				// declare the event handler for when input is added in the line item entry
				li.onLineItemRowTotalChange += new LineItem.LineItemRowTotalChangedDelegate(LineItemTotalUpdated);
				li.onLineItemRemove += new LineItem.LineItemRemoveDelegate(LineItemRemoved);
				li.PopulateLineItem(row);
			}
		}

		#endregion

		#region Delegate Declarations

		// build the subscriber model into this object for total time monitoring
		public delegate void TimeSheetTotalChangedDelegate();
		public event TimeSheetTotalChangedDelegate onTimeSheetTotalChange;


		#endregion

		#region Public UI Methods

		/// <summary>
		/// Add a line item entry to the Time Entry Panel
		/// Note: only 25 rows are possible - a UI error will be thrown if more than 25 are added
		/// </summary>
		public void AddLineItem() {
			if ( m_itemcursor == LINE_ITEM_MAX) {
				MessageBox.Show(getStringResource("17"));
			}
			else{
				// get a reference to the timesheet table that we are working with
				TimeSheetDS.TimeSheetDataTable tstable = (TimeSheetDS.TimeSheetDataTable) _timesheetrow.Table;
				// create a new time entry row
				TimeSheetDS.TimeEntryRow newrow = ((TimeSheetDS.TimeEntryDataTable)tstable.ChildRelations[0].ChildTable).NewTimeEntryRow();
				// associate the timesheet id to the entry row
				newrow.TID = _timesheetrow.TID;
                // then add the row to the table
				TimeSheetDS.TimeEntryDataTable timeentrytb = (TimeSheetDS.TimeEntryDataTable)((TimeSheetDS.TimeSheetDataTable)_timesheetrow.Table).ChildRelations[0].ChildTable;
				timeentrytb.AddTimeEntryRow(newrow);	
				addRowToPanel(newrow);			
			}			
		}

		#endregion


	}
}
