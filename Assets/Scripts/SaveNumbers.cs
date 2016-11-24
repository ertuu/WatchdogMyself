using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveNumbers : MonoBehaviour {

	public InputField PhoneNumber1;
	public InputField PhoneNumber2;
	public InputField PhoneNumber3;

	string info;

	AndroidJavaObject currentActivity;

	void Start () {

		PhoneNumber1 = GameObject.Find ("1stPhoneNumber").GetComponent (typeof(InputField)) as InputField; 
		PhoneNumber2 = GameObject.Find ("2ndPhoneNumber").GetComponent (typeof(InputField)) as InputField; 
		PhoneNumber3 = GameObject.Find ("3rdPhoneNumber").GetComponent (typeof(InputField)) as InputField; 
			
		//Fetching saved numbers
		PhoneNumber1.text =PlayerPrefs.GetString ("PhoneNumber1");
		PhoneNumber2.text =PlayerPrefs.GetString ("PhoneNumber2");
		PhoneNumber3.text =PlayerPrefs.GetString ("PhoneNumber3");

	}


	public void SavePhoneNumbers()
	{
		PlayerPrefs.SetString ("PhoneNumber1", PhoneNumber1.text);
		PlayerPrefs.SetString ("PhoneNumber2", PhoneNumber2.text);
		PlayerPrefs.SetString ("PhoneNumber3", PhoneNumber3.text);
		PlayerPrefs.Save ();


		info="Numbers saved.";
		if (Application.platform == RuntimePlatform.Android) {
			showToastOnUiThread (info);
		}
		Debug.Log (info);

		//AndroidJavaObject inf= new AndroidJavaObject("java.lang.String", info);
	}

	void showToast(){
		Debug.Log ("Running on UI thread");
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

		AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");

		//We need to use a java String to pass as argument to makeText.
		AndroidJavaObject javaString=new AndroidJavaObject("java.lang.String",info);
		AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));

		//Don't forget to call show :]
		toast.Call ("show");
	}
	void showToastOnUiThread(string info){
		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 

		currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		//this just adds the string passed as parameter to the toastString global variable we declared in the beginning.
		this.info =info;

		//the showToast which we pass as parameter here is a method which we will write next.
		currentActivity.Call ("runOnUiThread", new AndroidJavaRunnable (showToast));
	}
}
