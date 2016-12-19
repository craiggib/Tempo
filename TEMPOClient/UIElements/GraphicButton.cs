using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// Summary description for GraphicButton.
	/// </summary>
	public class GraphicButton : Button {


		// members
		private Bitmap inactivebg, activebg;

		public GraphicButton(Point location, Size size) {
			this.Location = location;
			this.Size = size;
			
			// other button setup
			this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BackColor = System.Drawing.Color.Transparent;
			this.ForeColor = System.Drawing.Color.Transparent;
			this.MouseEnter += new System.EventHandler(cursorToHand);
			this.MouseLeave += new System.EventHandler(cursorToPointer);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(cursorToWait);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(cursorToPointer2);

		}

		public void setGraphic (Bitmap graphicfile) {
			this.BackgroundImage = graphicfile;
			inactivebg = graphicfile;
		}

		/// <summary>
		/// Update the object so that it listens for mouse events to switch the background
		/// </summary>
		/// <param name="graphicfile">what the active background state will be</param>
		public void setOverGraphicState(Bitmap graphicfile) {
			activebg = graphicfile;
			this.MouseEnter += new System.EventHandler(switchBackground);
			this.MouseLeave += new System.EventHandler(switchBackground);
		}

		/// <summary>
		/// switch the background image to the alternative
		/// </summary>
		private void switchBackground(object sender, System.EventArgs e) {
			if (this.BackgroundImage == inactivebg) 
				this.BackgroundImage = activebg;
			else
				this.BackgroundImage = inactivebg;
		}

		private void cursorToHand(object sender, System.EventArgs e) {			
			this.Cursor = System.Windows.Forms.Cursors.Hand;
		}
		
		private void cursorToPointer(object sender, System.EventArgs e) {			
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}

		private void cursorToPointer2(object sender, System.Windows.Forms.MouseEventArgs e) {			
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}

		private void cursorToWait(object sender, System.Windows.Forms.MouseEventArgs e) {			
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
		}

	}
}
