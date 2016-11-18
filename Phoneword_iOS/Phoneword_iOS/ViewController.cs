using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Phoneword_iOS
{
	public partial class ViewController : UIViewController
	{
		public List<String> PhoneNumbers { get; set; }

		protected ViewController(IntPtr handle) : base(handle)
		{
			//initialize list of phone numbers called for Call History screen
			PhoneNumbers = new List<String>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string translatedNumber = "";

			TranslateButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				// Convert the phone number with text to a number
				// using PhoneTranslator.cs
				translatedNumber = PhoneTranslator.ToNumber(PhoneNumberText.Text);

				//Store the phone number that we're dialing in PhoneNumbers
				PhoneNumbers.Add(translatedNumber);

				// Dismiss the keyboard if text field was tapped
				PhoneNumberText.ResignFirstResponder();

				if (translatedNumber == "")
				{
					callButton.SetTitle("Call ", UIControlState.Normal);
					callButton.Enabled = false;
				}
				else {
					callButton.SetTitle("Call " + translatedNumber, UIControlState.Normal);
					callButton.Enabled = true;
				}
			};

			callButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				// Use URL handler with tel: prefix to invoke Apple's Phone app...
				var url = new NSUrl("tel:" + translatedNumber);

				Console.WriteLine(url.ToString());

				// ...otherwise show an alert dialog
				if (!UIApplication.SharedApplication.OpenUrl(url))
				{
					var alert = UIAlertController.Create("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
					PresentViewController(alert, true, null);
				}
			};

			CallHistoryButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				// Launches a new instance of CallHistoryController
				CallHistoryController callHistory = this.Storyboard.InstantiateViewController("CallHistoryController") as CallHistoryController;
				if (callHistory != null)
				{
					callHistory.PhoneNumbers = PhoneNumbers;
					this.NavigationController.PushViewController(callHistory, true);
				}
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
		{
			return false;
		}

		//public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		//{
		//	base.PrepareForSegue(segue, sender);

		//	// set the View Controller that’s powering the screen we’re
		//	// transitioning to

		//	var callHistoryContoller = segue.DestinationViewController as CallHistoryController;

		//	//set the Table View Controller’s list of phone numbers to the
		//	// list of dialed phone numbers

		//	if (callHistoryContoller != null)
		//	{
		//		callHistoryContoller.PhoneNumbers = PhoneNumbers;
		//	}
		//}
	}
}
