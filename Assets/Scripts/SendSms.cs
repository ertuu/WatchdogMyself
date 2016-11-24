using UnityEngine;
using UnityEngine.UI;


public class SendSms : MonoBehaviour
{

	public InputField PhoneNumber1;// Input fields
	public InputField PhoneNumber2;
	public InputField PhoneNumber3;
	public InputField MessageContext;

	/*public void EngineSend()
	{
		Send (PhoneNumber1.text);
	}*/

	public void Start()
	{
		// Assigning UI stuff
 		PhoneNumber1 = GameObject.Find ("1stPhoneNumber").GetComponent (typeof(InputField)) as InputField; 
		PhoneNumber2 = GameObject.Find ("2ndPhoneNumber").GetComponent (typeof(InputField)) as InputField; 
		PhoneNumber3 = GameObject.Find ("3rdPhoneNumber").GetComponent (typeof(InputField)) as InputField; 
		MessageContext = GameObject.Find ("StatusMessage").GetComponent (typeof(InputField)) as InputField;


	}

	AndroidJavaObject currentActivity;

	public void Send(string message)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			RunAndroidUiThread();
		}
	}

	void RunAndroidUiThread()
	{
		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(SendProcess));
	}

	void SendProcess()
	{
		Debug.Log("Running on UI thread");
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

		// SMS Information
		// developer test purpose
		//string PhoneNumber = "+358401907139"; //my own number =)
		//string text = "Hello world! This is a test SMS by SelfSafety application.";

		string alert;

		try
		{
			// SMS Manager

			AndroidJavaClass SMSManagerClass = new AndroidJavaClass("android.telephony.SmsManager");
			AndroidJavaObject SMSManagerObject = SMSManagerClass.CallStatic<AndroidJavaObject>("getDefault");
			SMSManagerObject.Call("sendTextMessage", PhoneNumber1.text, null, MessageContext.text, null, null);
			//SMSManagerObject.Call("sendTextMessage", PhoneNumber2.text, null, MessageContext.text, null, null);
			//SMSManagerObject.Call("sendTextMessage", PhoneNumber3.text, null, MessageContext.text, null, null);

			alert = "Messages sent successfully.";
		}
		catch (System.Exception e)
		{
			Debug.Log("Error : " + e.StackTrace.ToString());

			alert = "Failed to send messages.";
		}

		// Show Toast

		AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
		AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", alert);
		AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
		toast.Call("show");
	}
}