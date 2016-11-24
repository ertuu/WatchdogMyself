using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SystemTime : MonoBehaviour {

	Text timenow;

	void Start () {
		timenow = GameObject.Find ("DeviceTime").GetComponent (typeof(Text)) as Text;


	}


	void Update () {
		timenow.text = System.DateTime.Now.Hour.ToString () + "." + System.DateTime.Now.Minute.ToString ();
	}
}
