﻿using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {

	public Transform follow;

	// Use this for initialization
	void Start () {
		if(!follow)
			this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(follow.position.x, transform.position.y, follow.position.z);
	}
}
