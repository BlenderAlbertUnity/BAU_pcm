using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyText : MonoBehaviour {

	public Text baseText;
	Text mytext;

	// Use this for initialization
	void Start () {
		mytext = GetComponent<Text>();

		mytext.text = baseText.text;
	}
}
