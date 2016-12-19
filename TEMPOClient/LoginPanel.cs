using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.Authorization;
using System.Security.Principal;

namespace TEMPO.Client {
	
	/// <summary>
	/// The Login Panel that Authenticates the User
	/// These methods will assign the current thread to the authenticated user
	/// </summary>
	public class LoginPanel : SubPanel {

		# region Member Declarations

		private TextBox username, password; 
		private Label lblusername, lblpassword, lblwelcome, lblfullname; 
		private GraphicButton login,logout;
		private IPrincipal _bootuser;

		#endregion
		
		# region Public Initalization

		public LoginPanel(): base(new Point(20,10), new Size(311,62), "Login", false) {
		
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.loginpanel.png"));
			
			//input label 1
			lblusername = new Label();			
			lblusername.BackColor = System.Drawing.Color.FromArgb(204,204,204);
			this.addLabel(new Point(10,10), lblusername, "1", true, false,SubPanel.std_fontsize);
			
			//input label 2
			lblpassword = new Label();
			lblpassword.BackColor = System.Drawing.Color.FromArgb(204,204,204);
			this.addLabel(new Point(10,38), lblpassword, "2", true, false,SubPanel.std_fontsize);			

			//input text 1
			username = new TextBox();
			username.Size = new Size(120, 20);
			this.addTextBox(new Point(80,8), username);

			//input text 2
			password = new TextBox();
			password.Size = new Size(120, 20);
			password.PasswordChar='*';
			password.KeyUp += new KeyEventHandler(OnKeyPress);
			this.addTextBox(new Point(80,35), password);

			// login button
			login = new GraphicButton(new Point(225,38), new Size(78,18));
			login.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.loginbutton.png"));
			login.Click += new System.EventHandler(doLogin);
			this.Controls.Add(login);

			// welcome label
			lblwelcome = new Label();			
			lblwelcome.BackColor = System.Drawing.Color.FromArgb(229,229,229);
			lblwelcome.ForeColor = System.Drawing.Color.FromArgb(178,178,178);
			lblwelcome.Visible = false;
			this.addLabel(new Point(10,10), lblwelcome, "3", true, false,SubPanel.std_fontsize);
			
			// full name label
			lblfullname = new Label();			
			lblfullname.BackColor = System.Drawing.Color.FromArgb(204,204,204);
			lblfullname.ForeColor = System.Drawing.Color.FromArgb(51,51,51);
			lblfullname.Visible = false;
			this.addLabel(new Point(10,38), lblfullname, "3",  true, false,SubPanel.std_fontsize);


			// logout button
			logout = new GraphicButton(new Point(225,38), new Size(78,18));
			logout.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.logoutbutton.png"));
			logout.Visible = false;
			logout.Click += new System.EventHandler(doLogout);
			this.Controls.Add(logout);	

		}
		#endregion

		#region UI Event Handlers

		/// <summary>
		/// Perform the Login Operations
		/// </summary>
		private void doLogin (object o, System.EventArgs e) {
			try { 
			
				TEMPO.Authorization.TEMPOPrincipal p = new TEMPOPrincipal(new TEMPOIdentity(username.Text,password.Text));
				if (p.Identity.IsAuthenticated) {
					// store a copy of the old user
					_bootuser = Thread.CurrentPrincipal;
				
					// assign the thread principal so we can use and manage later
					Thread.CurrentPrincipal = p;
				
					//update the background graphic to display the welcome panel (logout)
					this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.logoutpanel.png"));

					// hide the username and password labels
					lblusername.Visible = false;
					lblpassword.Visible = false;
					login.Visible = false;
					username.Visible = false;
					password.Visible = false;

					// show the welcome and name lables
					lblwelcome.Visible = true;
					lblfullname.Visible = true;
					logout.Visible = true;

					// display the users name:
					lblfullname.Text = p.Identity.Name;
					buildNavigation();
				}
				else {
					// failed login
					MessageBox.Show(getStringResource("15"));
				}
			}
			catch {
				MessageBox.Show(getStringResource("15"));
			}
		}

		/// <summary>
		/// Perform the Logout Operations
		/// </summary>
		private void doLogout (object o, System.EventArgs e) {
			
			// revert back to the old user
			Thread.CurrentPrincipal = _bootuser;

			//update the background graphic to display the login panel
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.loginpanel.png"));

			// hide the username and password labels
			lblusername.Visible = true;
			lblpassword.Visible = true;
			login.Visible = true;
			username.Visible = true;
			password.Visible = true;
			password.Text = "";		
			username.Text = "";
			// show the welcome and name lables
			lblwelcome.Visible = false;
			lblfullname.Visible = false;
			logout.Visible = false;

			// display the users name:
			removeNavigation();
		}

		/// <summary>
		/// Handle the return button being pressed
		/// </summary>
		private void OnKeyPress(object sender, KeyEventArgs e) {
			if(e.KeyData == System.Windows.Forms.Keys.Enter)
				doLogin(new object(), new EventArgs());
		}

		#endregion

		#region UI Helper Functions

		private void removeNavigation() {
			MainScreen ms = (MainScreen) this.Parent;
			// remove the current panel
			ms.hideCurrentPanel();
			// update the navigation panel UI
			ms.Navigation.EnableNavigation();
		}

		private void buildNavigation() {
			// get a pointer to the mainscreen
			MainScreen ms = (MainScreen) this.Parent;
			// ask for a refresh of the navigation buttons
			ms.Navigation.EnableNavigation();
		}

		#endregion
	
	}
}
