using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for ClientCRUDEntryPanel.
	/// </summary>
	public class ClientCRUDEntryPanel: SubPanel {
		
		#region Member Declarations

		private Label temp;
		private TextBox clientname;
		private Button action, remove;
		private ClientDS.ClientRow _clientrow;

		# endregion

		public ClientCRUDEntryPanel(EntryType type): base(new Point(378,47), new Size(408,303), "ClientCRUDEntry", false) {
			// UI Init
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 20), temp, "53", false, false, SubPanel.std_fontsize);

			clientname = new TextBox();
			clientname.Size = new Size(200,21);
			this.addControl(new Point(200, 20), clientname, false,false,SubPanel.std_fontsize);

			
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
		
		public enum EntryType {
			Save = 0,
			Create = 1
		}

		public void populate(ClientDS.ClientRow clientrow) {
			_clientrow = clientrow;
			clientname.Text = _clientrow.ClientName;
		}
		

		private void saveRecord(object o, EventArgs e) {
			if (validateRecord()) {
				// perform save action
				_clientrow.ClientName = clientname.Text;				
				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateClients((ClientDS)_clientrow.Table.DataSet);
				}
				catch {
					MessageBox.Show(this.getStringResource("55"));
					return;
				}
				
				// success message
				MessageBox.Show(this.getStringResource("56"));	
				// notify the world
				onRecordMaintained();
			}		
		}


		private void createRecord(object o, EventArgs e) {
			if (validateRecord()) {
				// populate the business entity
				ClientDS ds = new ClientDS();
				ClientDS.ClientRow row = ds.Client.NewClientRow();
				row.ClientName = clientname.Text;
				// add it to the dataset
				ds.Client.AddClientRow(row);
				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateClients(ds);
				}
				catch {
					MessageBox.Show(this.getStringResource("57"));
					return;
				}

				// success message
				MessageBox.Show(this.getStringResource("58"));	
				// notify the world
				onRecordMaintained();
				// then reset the panel
				resetPanel();
			}		
		}

		private void removeRecord(object o, EventArgs e) {
			// verify deletion
			System.Windows.Forms.DialogResult dr = MessageBox.Show(this.getStringResource("59"), this.getStringResource("60"), System.Windows.Forms.MessageBoxButtons.YesNo);
			if (dr == System.Windows.Forms.DialogResult.Yes) {
				// mark the row for deletion
				_clientrow.Delete();
				try {
					// remove the employee
					RequestBroker.TEMPOServerProxy.Instance.UpdateClients((ClientDS)_clientrow.Table.DataSet);
				}
				catch {
					// an error occured
					MessageBox.Show(this.getStringResource("61"));
					return;
				}
				// succesful removal
				MessageBox.Show(this.getStringResource("62"));
				// notify the world
				onRecordMaintained();
				// now hide this panel
				this.Visible = false;
				// employee can't be deleted
				//	MessageBox.Show(this.getStringResource("63"));
			}
		}

		#region Delegate Declarations

		// create the delegates and events to monitor this objects existance
		public delegate void RecordMaintainDelegate();
		public event RecordMaintainDelegate onRecordMaintained;

		# endregion

		# region Helper Functions 

		private void resetPanel() {
			_clientrow = null;
			clientname.Text = "";
		}

		/// <summary>
		/// Validate the record
		/// </summary>
		private bool validateRecord() {
			
			if (clientname.Text == "") {
				MessageBox.Show(this.getStringResource("54"));
				return false;
			}			
			return true;
		}

		# endregion

	}
}
