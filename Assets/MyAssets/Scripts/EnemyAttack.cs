using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour {

	public int atkPower; 			//外部からのアクセスでこのオブジェクトの攻撃力の設定する為
	public string atkType; 			//外部からのアクセスでこのオブジェクトの攻撃タイプを設定する為
	public string hitEffect;

	List<PlayerParameter> attacked;

	//トリガーに何か触れた時のファンクション
	void OnTriggerEnter(Collider hitTarget){

		if(attacked == null)
			attacked = new List<PlayerParameter>();

		if(hitTarget.transform.tag == "Player"){
			//プレーヤーならパラメータースクリプトにアクセスしたいので
			PlayerParameter param = hitTarget.gameObject.GetComponent<PlayerParameter>();
			//もしパラメータースクリプトを見つけたら
			if(param){
				//ダメージを与える
				if(!attacked.Contains(param)){
					param.ApplyDamage(atkPower, atkType, hitEffect);
					attacked.Add(param);
				}
			}
		}
	}

	//トリガーに何か触れた時のファンクション
	void OnTriggerExit(Collider hitTarget){

		if(attacked == null)
			attacked = new List<PlayerParameter>();

		if(hitTarget.transform.tag == "Player"){
			//プレーヤーならパラメータースクリプトにアクセスしたいので
			PlayerParameter param = hitTarget.gameObject.GetComponent<PlayerParameter>();
			//もしパラメータースクリプトを見つけたら
			if(param){
				//ダメージを与える
				if(attacked.Contains(param)){
					attacked.Remove(param);
				}
			}
		}
	}
	//外部からのアクセスで攻撃力の設定
	public void SetAttackPower(int power, string type){
		atkPower = power;
		atkType = type;
	}
}
