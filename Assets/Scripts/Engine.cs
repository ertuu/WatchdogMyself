using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class Engine : MonoBehaviour {
	
	InputField Hours;
	InputField Minutes;
public	AudioSource DogBarking;
    AudioClip DogBarkingAudio;
	public SendSms SendMessage;
	public int WarningTimes=1;

	void Start () {
		Application.runInBackground = true;

		DogBarking = GameObject.Find ("Engine").GetComponent (typeof(AudioSource)) as AudioSource;
		SendMessage = GameObject.Find ("TestButton").GetComponent (typeof(SendSms)) as SendSms;
		//Buttons = GameObject.Find ("SetButton").GetComponent (typeof(Buttons)) as Buttons;
		DogBarkingAudio=DogBarking.clip;

		Hours = GameObject.Find ("Time.Hour").GetComponent (typeof(InputField)) as InputField;
		Minutes = GameObject.Find ("Time.Minutes").GetComponent (typeof(InputField)) as InputField;

	}
	
	void AlarmIsOn ()
	{

		if (Int32.Parse (Hours.text) == System.DateTime.Now.Hour) {
			if (Int32.Parse (Minutes.text) == System.DateTime.Now.Minute) {
				Debug.Log ("alarm alarm alarm");
				WarnUser ();
				DogBarking.PlayOneShot (DogBarkingAudio,1);
				Buttons.IsAlarmActive = false;

			}
		}

	}



	public void WarnUser()
	{
		string message = null;

	 
		if ((Int32.Parse (Minutes.text) + 10) > 60) {
			Hours.text = (Int32.Parse (Hours.text) + 1).ToString ();
		}

		switch (WarningTimes) {

		case 1: 
			Minutes.text = ((Int32.Parse (Minutes.text) + 10) % 60).ToString ();
			Debug.Log ("1/3 of the alarm has been set.");
			WarningTimes++;
			//play alarm	
			break;
		case 2:
			Minutes.text = ((Int32.Parse (Minutes.text) + 10) % 60).ToString ();
			Debug.Log ("2/3 of the alarm has been set.");
			WarningTimes++;
			break;	
		case 3:
			Minutes.text = ((Int32.Parse (Minutes.text) + 10) % 60).ToString ();
			Debug.Log ("3/3 of the alarm has been set.");
			WarningTimes++;
			break;
		case 4:
			
			SendMessage.Send (message);
			break;
		}


	}


	void Update () {
		if (Buttons.IsAlarmActive) {
			AlarmIsOn ();
		}



	}
}
