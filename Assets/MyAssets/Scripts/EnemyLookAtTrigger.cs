using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLookAtTrigger : MonoBehaviour {
	//攻撃対象リスト
	public List<GameObject> targetList;
	//最も近いターゲット
	public GameObject nearest;

	// Use this for initialization
	void Start () {
		//リストの指定
		targetList = new List<GameObject>();

	}
	void Update () {
		//リストに何かあれば
		if (targetList.Count > 0) {
			//もしリストに二体以上居れば
			if (targetList.Count > 1) {
				//リスト内の並べ替え
				targetList.Sort (SortByDistance);
			
			}
			//リストの一番のターゲットが最も近いターゲット
			nearest = targetList [0];
		//リストに何もなければ
		} else {
			//リセット
			nearest = null;
		}
	}
	// Update is called once per frame
	void OnTriggerEnter (Collider lookAtTarget) {
		if(lookAtTarget.transform.tag != "Player")
			return;
		//リストに追加
		targetList.Add(lookAtTarget.gameObject);

	}

	// Update is called once per frame
	void OnTriggerExit (Collider lookAtTarget) {
		if(lookAtTarget.transform.tag != "Player")
			return;
		//リストから削除
		targetList.Remove(lookAtTarget.gameObject);

		nearest = null;

	}
	//リスト内を距離により並べ替え
	private int SortByDistance(GameObject a , GameObject b){	
		return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position));
	}
}
