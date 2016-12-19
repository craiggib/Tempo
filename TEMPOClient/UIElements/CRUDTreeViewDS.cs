using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// Summary description for CRUDTreeViewDS.
	/// </summary>
	public class CRUDTreeViewDS: TreeView
	{
		private DataSet _treedata;
		private TreeNode root;
		private TreeNode[] nodes;
		private string _displaymember;

		public CRUDTreeViewDS(string CRUDName, DataSet collection, string DisplayMember) {
			
			_displaymember = DisplayMember;

			// init the scrolling
			this.Scrollable = true;

			// init the images
			this.ImageList = new ImageList();
			ImageList.Images.Add(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.treeview.closed.png"));
			ImageList.Images.Add(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.treeview.open.png"));
			ImageList.Images.Add(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.treeview.file.png"));

			// build the root node
			root = new TreeNode(CRUDName, 0, 1);
			this.SelectedNode = root;
			Nodes.Add(root);
			
			// event monitoring
			AfterSelect += new TreeViewEventHandler(sendDataSetRow);

			// tree data
			_treedata = collection;
			nodes = new TreeNode[_treedata.Tables[0].Rows.Count];
			
			// build the tree
			for (int i=0; i< nodes.Length; i++) {
				// build the node
				nodes[i] = new TreeNode(_treedata.Tables[0].Rows[i][_displaymember].ToString(), 2,2);
				// associate the tree data
				nodes[i].Tag = _treedata.Tables[0].Rows[i];
				// add the node to the root
				root.Nodes.Add(nodes[i]);
			}

			// init the data display
			root.Expand();
		}

		public void addRootNode(string name) {
			// disable updating
			BeginUpdate();
			
			TreeNode tr = new TreeNode(name, 2,2);
			root.Nodes.Add(tr);
			
			// select the root node
			this.SelectedNode = root;

			// enable updating
			EndUpdate();
		}

		/// <summary>
		/// Delegate function for CRUD Tree Item changes
		/// </summary>
		public delegate void CRUDItemClickedDelegate(DataRow dr);
		/// <summary>
		/// Fires when a CRUD Tree Item is clicked
		/// </summary>
		public event CRUDItemClickedDelegate onCRUDItemClicked;

		/// <summary>
		/// monitor changes and build the event wrapper
		/// </summary>
		private void sendDataSetRow(object o, System.Windows.Forms.TreeViewEventArgs tvea) {
			if (tvea.Node != root) onCRUDItemClicked((DataRow)tvea.Node.Tag);
		}

		public void refreshTree(DataSet newdata) {
			// clear what was there already
			root.Nodes.Clear();

			// tree data
			_treedata = newdata;
			nodes = new TreeNode[_treedata.Tables[0].Rows.Count];
			
			// build the tree
			for (int i=0; i< nodes.Length; i++) {
				// build the node
				nodes[i] = new TreeNode(_treedata.Tables[0].Rows[i][_displaymember].ToString(), 2,2);
				// associate the tree data
				nodes[i].Tag = _treedata.Tables[0].Rows[i];
				// add the node to the root
				root.Nodes.Add(nodes[i]);
			}
		}
	}
}
