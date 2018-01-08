using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.Events;

public class Load_Bar : MonoBehaviour {

	Image myImage;

	public float maxTime = 30.0f;
	public float minTime = 20.0f;

	float duration;
	float rest;

	public Text parcent;
	public Text restTime;

	[Header("OnFinished Callback")]
	public UnityEvent OnFinishedLoad;

	public void Start()
	{
		myImage = GetComponent<Image>();

		if (!parcent || !restTime)
			this.enabled = false;

		duration = Random.Range (minTime, maxTime);
	}

	public void Update()
	{
		if(rest < duration){
			
			rest += Time.deltaTime;

			myImage.fillAmount = rest / duration;

			parcent.text = ((int)(myImage.fillAmount * 100)).ToString () + " %";

			restTime.text = ((int)(duration - rest) + 1).ToString() + " s";

			if(rest >= duration)
				OnFinishedLoad.Invoke();
		}


	}
}