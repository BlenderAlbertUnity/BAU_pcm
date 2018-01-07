using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetTrigger : MonoBehaviour {

	public List<GameObject> targetList;

	public string targetTag;

	// Use this for initialization
	void Start () {

		targetList = new List<GameObject>();

	}
	void Update () {

		for (int i = 0; i < targetList.Count; ++i) {
			if(!targetList[i]){
				targetList.Remove(targetList[i]);
			}else if(!targetList[i].activeSelf){
				targetList.Remove(targetList[i]);
			}else{
				targetList[i].transform.GetChild(0).gameObject.SetActive(false);
			}
		}

		if (targetList.Count > 0) {

			if (targetList.Count > 1) {

				targetList.Sort (SortByDistance);

			}
			targetList[0].transform.GetChild(0).gameObject.SetActive(true);
		}
	}
	// Update is called once per frame
	void OnTriggerEnter (Collider lookAtTarget) {
		if(lookAtTarget.tag != targetTag)
			return;
		
		//Debug.Log ("Target in !!");
		if (lookAtTarget.transform.tag == targetTag) {
			targetList.Add (lookAtTarget.gameObject);

			if(targetList.Count > 0){

				List<GameObject> newTargetList = new List<GameObject>();

				foreach(GameObject enemy in targetList){
					if(enemy.tag != targetTag)
						newTargetList.Add(enemy);
				}

				if(newTargetList.Count > 0){
					foreach(GameObject enemy in newTargetList){
						if(enemy.tag != targetTag)
							targetList.Remove(enemy);
					}
				}
			}
		}
	}

	// Update is called once per frame
	void OnTriggerExit (Collider lookAtTarget) {
		if(lookAtTarget.tag != targetTag)
			return;

		//Debug.Log ("Target Out...");
		if(targetList.Contains(lookAtTarget.gameObject))
			targetList.Remove(lookAtTarget.gameObject);

		lookAtTarget.transform.GetChild(0).gameObject.SetActive(false);
	}

	private int SortByDistance(GameObject a , GameObject b){	

		return Vector3.Distance(transform.parent.position, a.transform.position).CompareTo(Vector3.Distance(transform.parent.position, b.transform.position));
	}
}
