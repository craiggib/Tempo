using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for TypeCRUDEntryPanel.
	/// </summary>
	public class TypeCRUDEntryPanel:SubPanel
	{
		#region Member Declarations

		private Label temp;
		private TextBox typename;
		private Button action, remove;
		private WorkTypeDS.WorkTypeRow _worktyperow;

		#endregion

		#region Public Initialization

		public TypeCRUDEntryPanel(EntryType type): base(new Point(378,47), new Size(408,303), "TypeCRUDEntry", false) {
			// UI Init
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 20), temp, "64", false, false, SubPanel.std_fontsize);

			typename = new TextBox();
			typename.Size = new Size(200,21);
			this.addControl(new Point(200, 20), typename, false,false,SubPanel.std_fontsize);
			
			// set up the type of panel
			action = new Button();
			this.addControl(new Point(200, 250), action, false, false, SubPanel.std_fontsize);
			if (type == EntryType.Save) {
				action.Text = "SAVE";
				action.Click += new System.EventHandler(saveRecord);
				// on save, we also need remove
				remove = new Button();
				remove.Text = "DELETE";
				this.addControl(new Point(280, 250), remove, false, false, SubPanel.std_fontsize);
				remove.Click += new System.EventHandler(removeRecord);

			} else {
				action.Text = "CREATE";
				action.Click += new System.EventHandler(createRecord);			
			}
		}

		#endregion

		#region Public Enumerations
		public enum EntryType {
			Save = 0,
			Create = 1
		}
		#endregion

		#region CRUD Operations	

		private void saveRecord(object o, EventArgs e) {
			if (validateRecord()) {
				// perform save action
				_worktyperow.WorkTypeName = typename.Text;
				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateWorkTypes((WorkTypeDS)_worktyperow.Table.DataSet);
				}
				catch {
					MessageBox.Show(this.getStringResource("66"));
					return;
				}				
				// success message
				MessageBox.Show(this.getStringResource("67"));	
				// notify the world
				onRecordMaintained();
			}		
		}


		private void createRecord(object o, EventArgs e) {
			if (validateRecord()) {
				// perform save action
				WorkTypeDS ds = new WorkTypeDS();
				WorkTypeDS.WorkTypeRow row = ds.WorkType.NewWorkTypeRow();
				row.WorkTypeName = typename.Text;
				ds.WorkType.AddWorkTypeRow(row);
				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateWorkTypes(ds);
				}
				catch {
					MessageBox.Show(this.getStringResource("68"));
					return;
				}
				// success message
				MessageBox.Show(this.getStringResource("69"));	
				// notify the world
				onRecordMaintained();
				// then reset the panel
				resetPanel();
			}		
		}

		private void removeRecord(object o, EventArgs e) {
			// verify deletion
			System.Windows.Forms.DialogResult dr = MessageBox.Show(this.getStringResource("70"), this.getStringResource("71"), System.Windows.Forms.MessageBoxButtons.YesNo);
			if (dr == System.Windows.Forms.DialogResult.Yes) {
				// check that we can delete
				_worktyperow.Delete();

				try {
					// remove the employee
					RequestBroker.TEMPOServerProxy.Instance.UpdateWorkTypes((WorkTypeDS)_worktyperow.Table.DataSet);
				}
				catch {
					// an error occured
					MessageBox.Show(this.getStringResource("72"));
					return;
				}
				// succesful removal
				MessageBox.Show(this.getStringResource("73"));
				// notify the world
				onRecordMaintained();
				// now hide this panel
				this.Visible = false;
			}
			else{ 
				// employee can't be deleted
				MessageBox.Show(this.getStringResource("74"));
				return;
			}
		}

		#endregion

		#region Helper Functions

		private void resetPanel() {
			_worktyperow = null;
			typename.Text = "";
		}

		public void Populate(WorkTypeDS.WorkTypeRow worktyperow) {
			_worktyperow = worktyperow;
			typename.Text = _worktyperow.WorkTypeName;
		}
		/// <summary>
		/// Validate the record
		/// </summary>
		private bool validateRecord() {
			
			if (typename.Text == "") {
				MessageBox.Show(this.getStringResource("65"));
				return false;
			}			
			return true;
		}


		#endregion

		#region Delegate Declarations
		// create the delegates and events to monitor this objects existance
		public delegate void RecordMaintainDelegate();
		public event RecordMaintainDelegate onRecordMaintained;
		#endregion


	}
}
