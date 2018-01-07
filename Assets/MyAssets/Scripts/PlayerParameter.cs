using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;


public class PlayerParameter : MonoBehaviour {

	[HideInInspector]
	//最大ＨＰ
	public int maxHp;
	[HideInInspector]
	//現在のＨＰ
	public int hp;
	//ＨＰの表示のUpDateをしたいのでアクセスします
	public Transform lifesPanel;
	Transform[] lifesUI;
	//ダメージ時のアニメーションを変えるために使う
	int damageType;
	public bool noDamage;
	//現在のアニメーション
	public AnimatorStateInfo curAnim;
	SmartController smaCon;

	public Sprite[] faces;
	public Image facePanel;
	float duration;

	[SerializeField]
	public UnityEvent Died;

	// Use this for initialization
	void Start () {
		if(!lifesPanel)
			this.enabled = false;

		smaCon = GetComponent<SmartController>();

		lifesUI = new Transform[lifesPanel.childCount];

		for (int i = 0; i < lifesUI.Length; i++) {
			lifesUI[i] = lifesPanel.GetChild(i);
		}

		maxHp = lifesUI.Length;
		//ＨＰのリセット
		hp = maxHp;
	}

	void Update(){
		if(duration > 0){
			duration -= Time.deltaTime;

			if(duration < 0){
				ChangeFace("Normal");
			}
		}
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
			ChangeFace("Dead");
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
			ChangeFace("Damage");
		}
		//AnimatorにDamageアニメーションを再生させる
		smaCon.animator.SetTrigger("Damage");
		smaCon.animator.SetInteger("DamageType", damageType);
		//ＨＰの表示のUpDate

		for (int i = 0; i < lifesUI.Length; i++) {
			if(i >= hp)
				lifesUI[i].GetChild(0).gameObject.SetActive(false);
		}
		Instantiate(Resources.Load(effect), transform.position, transform.rotation);
	}

	//回復ファンクション
	public void Cure(int amount, string effectName){
		//ＨＰに回復分足す
		hp += amount;
		//もしＨＰが最大ＨＰ以上なら最大ＨＰにする
		if(hp > maxHp){
			hp = maxHp;
		}
		//ＨＰの表示のUpDate
		for (int i = 0; i < hp; i++) {
			lifesUI[i].GetChild(0).gameObject.SetActive(true);
		}

		if(effectName != "")
			Instantiate (Resources.Load (effectName), transform.position, transform.rotation, transform);

		ChangeFace("Happy");
	}
	//死んだときのエッフェクトの再生
	public void Die(){
		//GameObject.Instantiate (Resources.Load ("Deleted"), transform.position, transform.rotation);
		//このオブジェクトの削除
		//GameObject.Destroy(gameObject);
		//ミッション失敗の表示
		//gamePanel.MissionFailedSystem();
		smaCon.moveDir.CanNotMove();
		Died.Invoke();
	}

	public void ChangeFace(string faceMotion){

		switch (faceMotion) {
		case "Normal":
			facePanel.sprite = faces[0];
			break;

		case "Happy":
			facePanel.sprite = faces[1];
			duration = 1.0f;
			break;

		case "Damage":
			facePanel.sprite = faces[2];
			duration = 1.0f;
			break;

		case "Dead":
			facePanel.sprite = faces[2];
			duration = 5.0f;
			break;
		}
	}
}
