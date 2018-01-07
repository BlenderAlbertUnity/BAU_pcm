using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlink : MonoBehaviour {
	[Tooltip("ブレンドシェイプリスト内の『瞬き』の番号")]
	public int index = 0;
	//BlendShapeへアクセスする為
	SkinnedMeshRenderer skinnedMeshRenderer;
	//計算用
	float weight = 0f;
	[Tooltip("『瞬き』のスピード")]
	public float speed = 500f;
	//閉じるか開くかを定める為
	bool finished = false;
	[Tooltip("次の『瞬き』迄の最大の待ち時間")]
	public float maxInterval = 5.0f;
	//次の『瞬き』までの待ち時間
	float coolDown;

	void Start (){
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();

		//indexの値がおかしければスクリプトを停止
		if (index > skinnedMeshRenderer.sharedMesh.blendShapeCount - 1 || index < 0)
			this.enabled = false;

		//次の瞬き迄の休憩時間
		coolDown = Random.Range (1.0f, maxInterval);
	}

	void Update (){
		//次の『瞬き』までの待ち時間の計算
		if(coolDown > 0){
			coolDown -= Time.deltaTime;

		//coolDownが終われば
		}else{
			//まだ瞼を閉じてないなら
			if (!finished) {
				
				//瞼を閉じる計算
				weight = Mathf.Clamp (weight + (Time.deltaTime * speed), 0.0f, 100.0f);

				//瞼を閉じたなら
				if(weight == 100.0f)
					finished = true;
				
			//もう瞼を閉じたなら
			} else {
				
				//瞼を開ける計算
				weight = Mathf.Clamp (weight - (Time.deltaTime * speed), 0.0f, 100.0f);

				//瞼を開けたなら各値をリセット
				if (weight == 0.0f) {
					finished = false;

					//次の瞬き迄の休憩時間
					coolDown = Random.Range (1.0f, maxInterval);
				}
			}
			//値をskinnedMeshRendererに渡す
			skinnedMeshRenderer.SetBlendShapeWeight (index, weight);
		}
	}
}
