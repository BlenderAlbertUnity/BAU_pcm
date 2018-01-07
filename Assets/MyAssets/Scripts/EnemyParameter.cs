using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyParameter : MonoBehaviour {
	//外部からのメッセージで一時停止させる為
	public bool canMove = true; 
	[HideInInspector]
	//最大ＨＰ
	public int maxHp;
	//現在のＨＰ
	private int hp;
	//このオブジェクトの3ＤCanvas
	private RectTransform myCanvas; 
	//ＨＰの表示のUpDateをしたいのでアクセスします
	public Transform lifesPanel;
	List<Transform> lifesUI;
	//ダメージ時のアニメーションを変えるために使う
	private int damageType;
	//Animatorのパラメーターにアクセスしたい
	public Animator animator;
	[HideInInspector]
	public bool gotDamage;
	[HideInInspector]
	public bool dead;
	public bool noDamage;
	private bool reseted;
	[HideInInspector]
	public EnemySpawnPoint spawnPoint;
	//現在のアニメーション
	public AnimatorStateInfo curAnim;
	//Canvasを常にメインカメラに向けたいのでメインカメラを取得する
	private Transform mainCam;
	// Use this for initialization
	void Start () {
		if(!lifesPanel)
			this.enabled = false;

		lifesUI = new List<Transform>();

		foreach (Transform child in lifesPanel) {
			lifesUI.Add(child);
		}
		maxHp = lifesUI.Count;
		//ＨＰのリセット
		hp = maxHp;
		//MainGameCanvasの指定
		myCanvas = transform.Find("LookAt/Canvas") as RectTransform;
		//見つけられなければエラーで知らせる
		if(!myCanvas){Debug.LogError("Can't find child Canvas !?");}
		//Animatorの指定
		animator = GetComponent<Animator>();
		//メインカメラを取得
		mainCam = Camera.main.transform;
	}
	// Update is called once per frame
	void Update () {
		//Animatorがあれば
		if (animator){
			//現在のアニメーションの取得
			curAnim = animator.GetCurrentAnimatorStateInfo(0);

			if(!canMove && !reseted){

				animator.ResetTrigger("Damage");
				animator.SetInteger("DamageType", 0);

				reseted = true;
			}
		}
	}
	//ここでＨＰパネルをカメラに向けます
	void LateUpdate () {
		myCanvas.rotation = mainCam.rotation;
	}
	
	//ダメージを与える為のファンクション
	public void ApplyDamage (int damage, string type, string effect) {
		//noDamage中なら
		if(noDamage)
			return;

		//ＨＰからダメージ分引く
		hp -= damage;
		//もしＨＰが0以下になったら0に直す
		if(hp <= 0){
			hp = 0;
			//AnimatorにDieアニメーションを再生させる
			damageType = 0;
		//もしＨＰが0以下にならなかったら
		}else{
			//ダメージが大きければ
			if(type == "Finish"){
				//ダメージアニメーションを変える
				damageType = 3;
				//ダメージが大きくないなら
			}else{
				//ダメージアニメーションを変える
				if(damageType == 1){
					damageType = 2;
				}else{
					damageType = 1;
				}
			}
		}
		//AnimatorにDamageアニメーションを再生させる
		animator.SetTrigger("Damage");
		animator.SetInteger("DamageType", damageType);
		gotDamage = true;
		//ＨＰの表示のUpDate
		int count =  lifesUI.Count;
		while (count != hp) {
			Destroy(lifesUI[0].gameObject);
			lifesUI.Remove(lifesUI[0]);
			count =  lifesUI.Count;
		}

		Instantiate(Resources.Load(effect), transform.position, transform.rotation);

		//Debug.Log(gameObject.name + " : " + "Got damage !" + " : " + Time.frameCount + " : " + damageType);
	}
	//出現時のエッフェクトの再生
	public void SpawnEffect(){
		GameObject.Instantiate (Resources.Load ("Enemy_Spawn"), transform.position, transform.rotation);
	}
	//隠れる時のエッフェクトの再生
	public void Hide(){
		GameObject.Instantiate (Resources.Load ("Enemy_Spawn"), transform.position, transform.rotation);
	}
	//死んだときのエッフェクトの再生
	public void Deleted(){
		GameObject.Instantiate (Resources.Load ("Enemy_Dead"), transform.position, transform.rotation);
		spawnPoint.RemoveEnemyFromList(this);
		//このオブジェクトの削除
		GameObject.Destroy(gameObject);
	}
	//動いて欲しくない時に外部からのアクセス
	public void CanNotMove(){
		canMove = false;
		reseted = false;
	}
	//動いてもいい時に外部からのアクセス
	public void CanMove(){
		canMove = true;
	}
}
