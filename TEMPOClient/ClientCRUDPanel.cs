using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using TEMPO.Client.UIElements;
using TEMPO.RequestBroker;
using TEMPO.BusinessEntity;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for ClientCRUDPanel.
	/// </summary>
	public class ClientCRUDPanel : SubPanel
	{
		private ClientCRUDEntryPanel entrypanel, createpanel;
		private GraphicButton create;
		private CRUDTreeViewDS ctv;

		public ClientCRUDPanel() : base(new Point(20,140), new Size(820,430), "ClientCRUD", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.CRUDpanel.png"));

			// build the tree view			
			// obtain the data from the server
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			ClientDS ds = tsp.GetAllClients();
			// populate the control
			ctv = new CRUDTreeViewDS("Employees", ds, "ClientName");
			ctv.onCRUDItemClicked += new CRUDTreeViewDS.CRUDItemClickedDelegate(openEntity);

			ctv.Location = new Point(32,71);
			ctv.Size = new Size(306,222);

			this.Controls.Add(ctv);

			entrypanel = new ClientCRUDEntryPanel(ClientCRUDEntryPanel.EntryType.Save);
			entrypanel.Visible = false;
			entrypanel.onRecordMaintained += new ClientCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(entrypanel);

			createpanel = new ClientCRUDEntryPanel(ClientCRUDEntryPanel.EntryType.Create);
			createpanel.Visible = false;
			createpanel.onRecordMaintained += new ClientCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(createpanel);

			create = new GraphicButton(new Point(180,315), new Size(155,28));
			create.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.addnewdocument.png"));
			create.Click += new System.EventHandler(newRecord);
			this.Controls.Add(create);
		}

		private void refreshList() {
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			ClientDS ds = tsp.GetAllClients();
			ctv.refreshTree(ds);
		}


		private void newRecord(object sender,EventArgs e) {
			createpanel.Visible = true;
			entrypanel.Visible = false;
		}

		private void openEntity(DataRow dr) {
			// in this case, the business entity is an employee
			ClientDS.ClientRow clientrow = (ClientDS.ClientRow) dr;
			createpanel.Visible = false;
			entrypanel.Visible = true;
			entrypanel.populate(clientrow);
		}

	}
}
