using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnPoint : MonoBehaviour {
	//出現させるエネミーリスト
	public GameObject[] enemiesList;
	//アクセス用
	List<EnemyParameter> enemies;
	//出現場所のリスト
	public Transform[] spawnPoints;
	//最大出現数
	public float maxCount;
	//最少出現時間と最大出現時間
	public Vector2 spawnRange = new Vector2(1.0f, 5.0f);
	//出現時間の計算用
	float cooldown;

	public SumScoreManager scoreM;

	SmartController player;

	void Start(){
		player = FindObjectOfType<SmartController>();

		//リストを初期化
		enemies = new List<EnemyParameter>();
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
			SpawnEnemy();
	}

	//敵を出現させる
	void SpawnEnemy () {
		//出現時間の計算用
		cooldown = Random.Range(spawnRange.x, spawnRange.y);
		//既に出現数が限界に達しているなら出現させない
		if(enemies.Count >= maxCount)
			return;
		//出現場所の選択用
		int rndm = Random.Range(0, enemiesList.Length - 1);
		//出現させつつ　EnemyParameter　にアクセス
		GameObject newEnemy = GameObject.Instantiate(Resources.Load(enemiesList[Random.Range(0, enemiesList.Length - 1)].name), spawnPoints[rndm].position, spawnPoints[rndm].rotation) as GameObject;
		EnemyParameter param = newEnemy.GetComponent<EnemyParameter>();
		param.spawnPoint = this;
		//リストに追加
		enemies.Add(param);
	}
	//敵が倒された時の外部呼び出し用
	public void RemoveEnemyFromList(EnemyParameter whoIs){
		enemies.Remove(whoIs);

		SumScore.Add(whoIs.maxHp);
	}
}
