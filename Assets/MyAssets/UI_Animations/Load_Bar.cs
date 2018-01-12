using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.Events;
using System;

public class Load_Bar : MonoBehaviour {

	Image myImage;

	public float maxTime = 30.0f;
	public float minTime = 20.0f;

	float duration;
	float rest;

	public Text parcent;
	public Text restTime;

	DateTime lastPlayed;

	public GameObject skipBttn;

	[Header("OnFinished Callback")]
	public UnityEvent OnFinishedLoad;

	public void Start()
	{
		myImage = GetComponent<Image>();

		if (!parcent || !restTime)
			this.enabled = false;

		duration = UnityEngine.Random.Range (minTime, maxTime);

		if(PrefsManager.HasKey("LastPlayed")){
			lastPlayed = PrefsManager.GetValue<DateTime> ("LastPlayed");

			if (DateTime.Today == lastPlayed) {
				duration = 10.0f;
				skipBttn.SetActive (false);
			}
		}
	}

	public void Update()
	{
		if(rest < duration){
			
			rest += Time.deltaTime;

			myImage.fillAmount = rest / duration;

			parcent.text = ((int)(myImage.fillAmount * 100)).ToString () + " %";

			if (myImage.fillAmount < 0.95f) {
				restTime.text = ((int)(duration - rest) + 1).ToString () + " s";
			} else {
				restTime.text = ((int)(duration - rest)).ToString() + " s";
			}
			if(rest >= duration)
				OnFinishedLoad.Invoke();
		}


	}
}