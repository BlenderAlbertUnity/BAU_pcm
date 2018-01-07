using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	Transform mainCam;

	// Use this for initialization
	void Start () {
		mainCam = GameObject.FindWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(mainCam)
			transform.LookAt(mainCam);
	}
}
