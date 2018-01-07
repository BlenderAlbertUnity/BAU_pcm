using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	public int atkPower; 			//外部からのアクセスでこのオブジェクトの攻撃力の設定する為
	public string atkType; 			//外部からのアクセスでこのオブジェクトの攻撃タイプを設定する為
	public string hitEffect;

	List<EnemyParameter> attacked;

	//トリガーに何か触れた時のファンクション
	void OnTriggerEnter(Collider hitTarget){

		if(attacked == null)
			attacked = new List<EnemyParameter>();

		if(hitTarget.transform.tag == "Enemy"){
			//プレーヤーならパラメータースクリプトにアクセスしたいので
			EnemyParameter param = hitTarget.gameObject.GetComponent<EnemyParameter>();
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
	//外部からのアクセスで攻撃力の設定
	public void SetAttackPower(int power, string type){
		atkPower = power;
		atkType = type;
	}
}
