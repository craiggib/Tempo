using System;

namespace TEMPO.Client.UIElements {
	/// <summary>
	/// Singleton pattern used to implement static access to common functions
	/// Access this class throught the Instance field
	/// </summary>
	public class InputValidation {

		/// <summary>
		/// Static singleton variable
		/// </summary>
		private static InputValidation _instance;
		
		/// <summary>
		/// Private Creation Method
		/// </summary>
		private InputValidation() { }

		/// <summary>
		/// Get Method for this singleton implementation
		/// </summary>
		public static InputValidation Instance {
			get { 
				if (_instance == null) _instance = new InputValidation();
				return _instance;
			}
		}

		/// <summary>
		/// Tests if the string is empty
		/// </summary>
		/// <param name="test">the string to test</param>
		/// <returns>-true if the string is empty, false otherwise</returns>
		public bool TestStringEmpty(string test) {
			if (test == "") return true;
			else return false;
		}

		/// <summary>
		/// Tests if the string can be converted into a Decimal
		/// </summary>
		/// <param name="test">the string to test</param>
		/// <returns>true if the string can be converted without errors, false otherwise</returns>
		public bool TestConvertToDecimal(string test) {
			try {
				Convert.ToDecimal(test);
			}
			catch {
				return false;
			}
			return true;

		}

	}
}
