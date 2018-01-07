using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class MissileObject : MonoBehaviour {
	//外部からのアクセスでこのオブジェクトの攻撃力の設定する為
	private int atkPower;
	//このオブジェクトの基本的な存在時間（もし何にも触れなければ自動的に消す）
	public float lifeTime;
	[Tooltip("このオブジェクトの移動速度")]
	public float speed;
	//このオブジェクトを動かしたいのでアクセスします
	private Rigidbody myRigdbody;
	[Tooltip("Collider か　Trigger を使うのか")]
	public bool useTrigger = false;
	[Tooltip("このオブジェクトを少しずつ大きくする為")]
	public float triggerTime;
	[Tooltip("ヒット時に再生するエフェクト")]
	public string hitEffect = "HitEffect";
	[Tooltip("ヒットエフェクトでダメージを与えるかどうか")]
	public bool effectDmg = true;
	[Tooltip("ヒット時のエフェクトの位置調節")]
	public Vector3 offset;
	[Tooltip("与えるダメージの種類")]
	public string dmgType = "Normal";
	[Tooltip("ヒット時にDestroyするかどうか")]
	public bool destroyIt = true;
	[Tooltip("帰還するターゲット")]
	public Transform home;
	[Tooltip("帰還するターゲットのタグ")]
	public string homeTag;
	[Tooltip("攻撃するターゲットの位置")]
	public Vector3 target;
	[Tooltip("攻撃対象のタグ")]
	public string targetTag;
	[Tooltip("方向転換速度")]
	public float turnSpeed = 5.0f;
	//帰還中かどうか
	public bool backHome = false;
	//戻った時にアクセス
	WeaponManager weaponMngr;

	// Use this for initialization
	void Awake () {

		//Rigidbodyにアクセス
		myRigdbody = transform.GetComponent<Rigidbody>();
		//もし何かの中にショットを召還した時の為に一応OnCollision判定を調べさせます
		myRigdbody.WakeUp ();
		//自動削除をセット 
		if(destroyIt)
			StartCoroutine(SetWaitToDestroy());

		if(triggerTime == 0){
			triggerTime = lifeTime;
		}

		if(transform.parent){
			home = transform.parent;

			target = transform.position  + transform.parent.forward * Random.Range(3, 5);
			//target = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(1, 2), transform.position.z + Random.Range(3, 5));

			weaponMngr = transform.GetComponentInParent<WeaponManager>();

			transform.parent = null;
		}
	}

	//自動削除予約ファンクション
	IEnumerator SetWaitToDestroy(){
		//lifeTime分の時間を待たせてから
		yield return new WaitForSeconds(lifeTime);
		//自動削除ファンクションの呼び出し
		DestroyThisGameObject();

		if(hitEffect != ""){
			if(destroyIt)
				//自動削除ファンクションの呼び出し
				DestroyThisGameObject();

			//エフェクトの再生
			GameObject effect = Instantiate(Resources.Load(hitEffect), transform.position + offset, transform.rotation) as GameObject;

			if(effectDmg)
				effect.SendMessage("SetAttackPower", atkPower);
		}

	}

	// Update is called once per frame
	void FixedUpdate () {
		//このオブジェクトの移動の適用
		myRigdbody.MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));

		if(triggerTime != 0){
			triggerTime -= Time.fixedDeltaTime; 

			if(triggerTime < 0){
				triggerTime = 0;
			}
		}

		//ターゲットの方向を計算用
		Quaternion targetRotation;

		if(!backHome){
			if(Vector3.Distance(transform.position, target) < 0.1f){
				backHome = true;
				turnSpeed = turnSpeed + Random.Range(-20, 20);
			}

			//ターゲットの方向を計算
			targetRotation = Quaternion.LookRotation(target - transform.position);
		}else{
			//ターゲットの方向を計算
			targetRotation = Quaternion.LookRotation((home.position +  (home.forward * 0.5f) + (home.up * triggerTime)) - transform.position);
			turnSpeed += Time.fixedDeltaTime * 10; 
		}
		////ターゲットの方向を向かせる
		myRigdbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
	}

	//トリガーに何か触れた時のファンクション
	void OnCollisionEnter(Collision hitTarget){
		if(useTrigger)
			return;

		Debug.Log(hitTarget.transform.tag);

		if(hitTarget.transform.tag == homeTag){
			if(backHome){	
				//自動削除ファンクションの呼び出し
				DestroyThisGameObject();

				weaponMngr.WeaponThrow("Show");
			}
		}else if(hitTarget.transform.tag == targetTag && triggerTime != 0){
			//プレーヤーならパラメータースクリプトにアクセスしたいので
			/*EnemyParameter param = hitTarget.gameObject.GetComponent<EnemyParameter>();
			//もしパラメータースクリプトを見つけたら
			if(param && !effectDmg){
				//ダメージを与える
				param.ApplyDamage(atkPower, dmgType);
			}*/

			if(destroyIt){
				//自動削除ファンクションの呼び出し
				DestroyThisGameObject();
			}else{
				backHome = true;
				turnSpeed = turnSpeed + Random.Range(-20, 20);
			}

			if(hitEffect != ""){
				if(destroyIt)
					//自動削除ファンクションの呼び出し
					DestroyThisGameObject();

				//エフェクトの再生
				GameObject effect = Instantiate(Resources.Load(hitEffect), transform.position + offset, transform.rotation) as GameObject;

				if(effectDmg)
					effect.SendMessage("SetAttackPower", atkPower);
			}
		}else{
			//自動削除ファンクションの呼び出し
			DestroyThisGameObject();

			weaponMngr.WeaponThrow("ShowWithEffect");
		}
	}

	//トリガーに何か触れた時のファンクション
	void OnTriggerEnter(Collider hitTarget){
		if(!useTrigger)
			return;

		Debug.Log(hitTarget.tag);

		if(hitTarget.tag == homeTag){
			if(backHome){	
				//自動削除ファンクションの呼び出し
				DestroyThisGameObject();

				weaponMngr.WeaponThrow("Show");
			}
		}else if(hitTarget.transform.tag == targetTag && triggerTime != 0){
			//プレーヤーならパラメータースクリプトにアクセスしたいので
			/*EnemyParameter param = hitTarget.gameObject.GetComponent<EnemyParameter>();
			//もしパラメータースクリプトを見つけたら
			if(param && !effectDmg){
				//ダメージを与える
				param.ApplyDamage(atkPower, dmgType);
			}*/

			if(destroyIt){
				//自動削除ファンクションの呼び出し
				DestroyThisGameObject();
			}else{
				backHome = true;
				turnSpeed = turnSpeed + Random.Range(-20, 20);
			}

			if(hitEffect != ""){
				if(destroyIt)
					//自動削除ファンクションの呼び出し
					DestroyThisGameObject();

				//エフェクトの再生
				GameObject effect = Instantiate(Resources.Load(hitEffect), transform.position + offset, transform.rotation) as GameObject;

				if(effectDmg)
					effect.SendMessage("SetAttackPower", atkPower);
			}
		}else{
			//自動削除ファンクションの呼び出し
			DestroyThisGameObject();

			weaponMngr.WeaponThrow("ShowWithEffect");
		}
	}

	//自動削除ファンクション
	public void DestroyThisGameObject(){
		//このオブジェクトの削除
		GameObject.Destroy(gameObject);


	}
	//外部からのアクセスで攻撃力の設定
	void SetAttackPower(int power){
		atkPower = power;
	}
	//外部からのアクセスでターゲットの設定
	void SetTargetPosition(Vector3 newTarget){
		target = newTarget;
	}
}
