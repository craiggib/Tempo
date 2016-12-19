using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.BusinessObjects.Entity;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// CRUDTreeView buids a tree view based on a set of business entity data
	/// </summary>
	public class CRUDTreeView : TreeView
	{
		private TreeNode root;
		private TreeNode[] nodes;
		private TEMPO.BusinessObjects.Entity.BusinessEntity[] treedata;

		public CRUDTreeView(string CRUDName, TEMPO.BusinessObjects.Entity.BusinessEntity[] collection, string[] names) {
			
			// name databoth array lenghts must be the same length
			if (names.Length != collection.Length) throw new Exception();

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
			AfterSelect += new TreeViewEventHandler(sendBusinessEntity);

			// tree data
			treedata = collection;
            nodes = new TreeNode[treedata.Length];
			
			// build the tree
			for (int i=0; i< names.Length; i++) {
				// build the node
				nodes[i] = new TreeNode(names[i], 2,2);
				// associate the tree data
				nodes[i].Tag = treedata[i];
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
		public delegate void CRUDItemClickedDelegate(TEMPO.BusinessObjects.Entity.BusinessEntity be);
		/// <summary>
		/// Fires when a CRUD Tree Item is clicked
		/// </summary>
		public event CRUDItemClickedDelegate onCRUDItemClicked;

		/// <summary>
		/// monitor changes and build the event wrapper
		/// </summary>
		private void sendBusinessEntity(object o, System.Windows.Forms.TreeViewEventArgs tvea) {
			if (tvea.Node != root) onCRUDItemClicked((TEMPO.BusinessObjects.Entity.BusinessEntity) tvea.Node.Tag);
		}

		public void refreshTree(TEMPO.BusinessObjects.Entity.BusinessEntity[] collection, string[] names) {
			// clear what was there already
			root.Nodes.Clear();

			// tree data
			treedata = collection;
			nodes = new TreeNode[treedata.Length];
			
			// build the tree
			for (int i=0; i< names.Length; i++) {
				// build the node
				nodes[i] = new TreeNode(names[i], 2,2);
				// associate the tree data
				nodes[i].Tag = treedata[i];
				// add the node to the root
				root.Nodes.Add(nodes[i]);
			}
		}

	}
}
