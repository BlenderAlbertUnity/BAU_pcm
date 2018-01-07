using UnityEngine;
using System.Collections;

public class CureItem : MonoBehaviour {

	public int amount = 1;

	public string cureEffect = "Cure";

	//トリガーに何か触れた時のファンクション
	void OnTriggerEnter(Collider hitTarget){

		if(hitTarget.transform.tag == "Player"){
			//プレーヤーならパラメータースクリプトにアクセスしたいので
			PlayerParameter param = hitTarget.gameObject.GetComponent<PlayerParameter>();
			//もしパラメータースクリプトを見つけたらダメージを与える
			if(param){
				if(param.hp < param.maxHp && param.hp > 0){
					param.Cure(amount, cureEffect);
					GameObject.Destroy(gameObject);
				}
			}
		}

	}
}
