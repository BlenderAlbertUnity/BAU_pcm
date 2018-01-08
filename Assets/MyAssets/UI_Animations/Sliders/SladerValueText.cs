using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class SladerValueText : MonoBehaviour {

	Image myImage;
	public Text valueText;

	public string valueType = " %";

	public void Start()
	{
		myImage = GetComponent<Image>();

		if (!valueText)
			valueText = GetComponentInChildren<Text> ();
	}

	public void LateUpdate()
	{
		valueText.text = ((int)(myImage.fillAmount * 100f)).ToString () + valueType;
	}
}
