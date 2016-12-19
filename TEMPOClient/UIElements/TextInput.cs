using System;
using System.Windows.Forms;
using System.Drawing;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// Contains the layout elements and for an input text box
	/// 1. backgroun image with text description
	/// 2. input control
	/// 3. validation routines
	/// </summary>
	public class TextInput: Panel {
		
		private TextBox m_input;
		public TextInput(int pos_x, int pos_y, int width, int height, Point inputloc, Size inputsize) {

			// register the location of the object on the panel
			this.Location = new System.Drawing.Point(pos_x, pos_y);
			
			// register the size of the object
			this.Size = new System.Drawing.Size(width, height);
			
			
			setupInputBox(inputloc, inputsize);
		}

		public void setPasswordMode() {
			m_input.PasswordChar = '*';
		}

		public void setGraphic (Bitmap graphicfile) {
			this.BackgroundImage = graphicfile;
		}

		private void setupInputBox(Point loc, Size size) {
			m_input = new TextBox();
			m_input.Location = loc;
			m_input.Size = size;
			this.Controls.Add(m_input);
		}


	}
}
