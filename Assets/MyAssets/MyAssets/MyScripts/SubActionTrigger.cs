using UnityEngine;
using System.Collections;

public class SubActionTrigger : MonoBehaviour {
	public SubActionMng.ActionIndex action;		//どのアクションなのかを指定
	public bool telled;				//プレイヤーにアクションコマンドが通達されてるか
	public float rotDifLimt;			//プレイヤーの向きによってボタンが見えたり消えたりする為
	private SubActionMng player;			//プレイヤー
	public Transform[] targetList;			//全マッチターゲットのリスト
	public BasicMatchTarget.BodyParts bodyPart;	//どの部分をマッチさせるのかを指定

	// Use this for initialization
	void Start () {
		if(targetList.Length < 1)
			this.enabled = false;
	}

	// トリガーに何か入ると・・・
	void OnTriggerEnter (Collider col) {
		//プレイヤーの向きがトリガーの向きに近ければボタンを見せる
		if(col.tag == "Player"){
			
			player = col.GetComponent<SubActionMng>();
		}
	}
	// トリガー内に何か居ると・・・
	void OnTriggerStay (Collider col) {
		//プレイヤーじゃなけらば何もしない
		if(col.tag != "Player" || !player)
			return;

		ChecktoSubAction (col);


	}

	// トリガー内から何か出るとボタンを隠す
	void OnTriggerExit (Collider col) {
		//プレイヤーじゃなけらば何もしない
		if(col.tag != "Player")
			return;
		
		player.SetSubActionValues(SubActionMng.ActionIndex.None, new Transform[0], bodyPart);
		telled = false;
		player = null;
	}

	// トリガー内から何か出るとボタンを隠す
	void ChecktoSubAction (Collider col) {
		switch (action) {
		case SubActionMng.ActionIndex.None:
			
			break;

		case SubActionMng.ActionIndex.Climb:
			//プレイヤーの向きがトリガーの向きに近ければボタンを見せる
			if(col.transform.rotation.eulerAngles.y < transform.rotation.eulerAngles.y + rotDifLimt &&
				col.transform.rotation.eulerAngles.y > transform.rotation.eulerAngles.y - rotDifLimt &&
					col.transform.position.y >= transform.position.y){
				if(!telled){
					//まだ通達してないなら
					player.SetSubActionValues(action, targetList, bodyPart);
					telled = true;
				}
				//プレイヤーの向きがトリガーの向きに近くなければボタンを隠す
			}else{
				player.SetSubActionValues(SubActionMng.ActionIndex.None, new Transform[0], bodyPart);
				telled = false;
			}

			break;

		case SubActionMng.ActionIndex.Sit_Down:
			//プレイヤーの向きがトリガーの向きに近ければボタンを見せる
			if(col.transform.rotation.eulerAngles.y < transform.rotation.eulerAngles.y + rotDifLimt && col.transform.rotation.eulerAngles.y > transform.rotation.eulerAngles.y - rotDifLimt){
				if(!telled){
					//まだ通達してないなら
					player.SetSubActionValues(action, targetList, bodyPart);
					telled = true;
				}
				//プレイヤーの向きがトリガーの向きに近くなければボタンを隠す
			}else{
					player.SetSubActionValues(SubActionMng.ActionIndex.None, new Transform[0], bodyPart);
					telled = false;

			}
			break;

		case SubActionMng.ActionIndex.Crouch:
			if(!telled){
				//まだ通達してないなら
				player.SetSubActionValues(action, targetList, bodyPart);
				telled = true;
			}
			break;
		}
	}

}