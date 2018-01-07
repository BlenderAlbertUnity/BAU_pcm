using UnityEngine;
using System.Collections;

public class ParticleEmitSystem : MonoBehaviour {
	
	ParticleSystem myParticle;	//自身のParticleSystem
	ParticleSystem[] childs;	//各子GameObjectのParticleSystem

	public bool swicth = false;	//On Off　の切り替え用
	bool played = false;		//何フレームもswicthを更新させないため

	void Start () {
		//自身のParticleSystemへアクセス
		myParticle = GetComponent<ParticleSystem>();
		//各子GameObjectのParticleSystemへアクセス
		if(transform.childCount > 0)
			childs = myParticle.GetComponentsInChildren<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {
		//Onなら・・・
		if(swicth){
			//まだ再生してないなら
			if(!played){
				//エフェクト再生
				myParticle.Play();
				//必要なら各子のParticleSystemも再生
				if(childs != null){
					foreach (ParticleSystem child in childs) {
						child.Play();
					}
				}
				//再生済みを記憶
				played = true;
			}
		//Off なら
		}else{
			//既に再生済みなら
			if(played){
				//エフェクト停止
				myParticle.Stop();
				//必要なら各子のParticleSystemも停止
				if(childs != null){
					foreach (ParticleSystem child in childs) {
						child.Stop();
					}
				}
			}
			//停止を記憶
			played = false;
		}
	}
}
