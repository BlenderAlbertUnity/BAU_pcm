using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprite : MonoBehaviour {

	public GameObject[] sprites;
	public float interval = 1.0f;
	float curTime;
	int index = 0;

	// Use this for initialization
	void Start () {
		if (sprites.Length < 1)
			this.enabled = false;

		for (int i = 0; i < sprites.Length; i++) {
			if (i == index) {
				sprites [i].SetActive (true);
			} else {
				sprites [i].SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		curTime += Time.deltaTime;

		if (curTime > interval) {
			curTime -= interval;

			index++;

			if (index > sprites.Length - 1)
				index = 0;

			for (int i = 0; i < sprites.Length; i++) {
				if (i == index) {
					sprites [i].SetActive (true);
				} else {
					sprites [i].SetActive (false);
				}
			}
		}
			
	}
}
