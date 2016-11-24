using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Buttons: MonoBehaviour {

	static public bool  	IsAlarmActive;
	private InputField 	InputHour;
	private InputField	InputMinutes;
	string alert;
	public  Button ResetButton;
	public Button SetButton;
	public Engine EngineStop;


	AndroidJavaObject currentActivity;

	void Start () {
		
		InputHour = GameObject.Find ("Canvas/Time.Hour").GetComponent (typeof(InputField)) as InputField;
		InputMinutes = GameObject.Find ("Time.Minutes").GetComponent (typeof(InputField)) as InputField;
		ResetButton=GameObject.Find("ResetButton").GetComponent(typeof(Button)) as Button;
		SetButton=GameObject.Find("SetButton").GetComponent(typeof(Button)) as Button;

		EngineStop = GameObject.Find ("Engine").GetComponent (typeof(Engine)) as Engine;

		IsAlarmActive = false;

	}


	public void SetAlarm() {



		if (InputHour.text == "" || InputMinutes.text == "" ) {
			alert = "Enter guessed arrival time.";


		} else if (Int32.Parse (InputHour.text)>24 || Int32.Parse(InputMinutes.text)>60 ) {
			alert = "Hours must be between 0-24 and Minutes must be between 0-60.";
			InputHour.text = "";
			InputMinutes.text = "";
		}

		else
		
		{

			IsAlarmActive = true;
			ResetButton.interactable = true;
			SetButton.interactable = false;
			alert = "Alarm has been set.";
		}

		Debug.Log (alert);

		if (Application.platform == RuntimePlatform.Android) {
			showToastOnUiThread (alert);
		}


	}

	public void ResetAlarm() {

		IsAlarmActive = false;
		ResetButton.interactable = false;
		SetButton.interactable = true;
		InputHour.text = "";
		InputMinutes.text = "";

		alert="Alarm has been reset.";
		if (Application.platform == RuntimePlatform.Android) {
			showToastOnUiThread (alert);
		}

		EngineStop.DogBarking.Stop ();
		EngineStop.WarningTimes = 1;

	}


	void showToastOnUiThread(string alert){
		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 

		currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		//this just adds the string passed as parameter to the toastString global variable we declared in the beginning.
		this.alert =alert;

		//the showToast which we pass as parameter here is a method which we will write next.
		currentActivity.Call ("runOnUiThread", new AndroidJavaRunnable (showToast));
	}

	void showToast(){
		Debug.Log ("Running on UI thread");
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

		AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");

		//We need to use a java String to pass as argument to makeText.
		AndroidJavaObject javaString=new AndroidJavaObject("java.lang.String",alert);
		AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));

		//Don't forget to call show :]
		toast.Call ("show");
	}




}
