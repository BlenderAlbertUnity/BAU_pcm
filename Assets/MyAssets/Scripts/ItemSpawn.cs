using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawn : MonoBehaviour {
	//出現させるエネミーリスト
	public GameObject[] itemList;
	//最少出現時間と最大出現時間
	public Vector2 spawnRange = new Vector2(10.0f,20.0f);
	//出現時間の計算用
	float cooldown;

	SmartController player;

	void Start(){
		player = FindObjectOfType<SmartController>();
		//待機時間の指定
		cooldown = Random.Range(spawnRange.x, spawnRange.y);
	}

	void Update () {
		if(!player.moveDir.canMove)
			return;
		
		//待機時間の計算
		cooldown -= Time.deltaTime;
		//待機時間が過ぎたなら敵を出現
		if(cooldown < 0)
			SpawnItem();
	}
	//敵を出現させる
	void SpawnItem () {
		//出現時間の計算用
		cooldown = Random.Range(spawnRange.x, spawnRange.y);
		//既に出現数が限界に達しているなら出現させない
		if(transform.childCount > 0)
			return;
		//出現させつつ　EnemyParameter　にアクセス
		GameObject.Instantiate(Resources.Load(itemList[Random.Range(0, itemList.Length)].name), transform.position, transform.rotation, transform);
	}
}
