using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour {

	public float lifeTime = 10.0f;	//このオブジェクトの基本的な存在時間（もし何にも触れなければ自動的に消す）

	// Use this for initialization
	void Awake () {
		//自動削除をセット
		GameObject.Destroy(gameObject, lifeTime);
	}
		
}