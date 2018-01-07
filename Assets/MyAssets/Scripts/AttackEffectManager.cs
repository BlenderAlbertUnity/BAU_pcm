using UnityEngine;
using System.Collections;

public class AttackEffectManager : MonoBehaviour {
	//攻撃エフェクトを再生する場所
	public string[] effName;
	//攻撃エフェクトを再生する場所
	public Transform[] effPosition;
	/*
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
	*/
	//攻撃GameObjectの再生
	public void AttackEffect (int index) {
		GameObject.Instantiate (Resources.Load (effName[index]), effPosition[index].position, effPosition[index].rotation);
	}
	//プレイヤーの子 GameObject として攻撃 GameObject の再生
	public void AttackEffectChildren (int index) {
		GameObject.Instantiate (Resources.Load (effName[index]), effPosition[index].position, effPosition[index].rotation, transform);
	}
}