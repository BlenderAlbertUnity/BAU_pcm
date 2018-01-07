using UnityEngine;
using System.Collections;

public class BasicEnemyAI : MonoBehaviour {
	public enum AiSituation{
		Patrol, InBattle, BackOff, Paused
	}
	public AiSituation mySituation;
	//プレイヤーを発見するトリガースクリプト
	public EnemyLookAtTrigger targetTrigger;
	//攻撃する相手
	public Transform target;
	//攻撃エフェクトを再生する場所
	public string[] effName;
	//攻撃エフェクトを再生する場所
	public Transform[] effPosition;
	//攻撃する為の距離
	public float[] atkDistance;
	//使用する攻撃
	private int atkType;
	//ターゲットの方向を向いていい時はTrue
	public bool faceTarget;
	//次に攻撃するまでの休憩時間
	private float coolDown;
	//今攻撃中かどうか
	public bool inAttack;
	//今攻撃したいかどうか
	private bool wantAttack;
	//退避したいかどうか
	private bool backOff;
	//退避したいかどうか
	public bool inAction;
	//CanMoveにアクセスしたいので
	private EnemyParameter parameterCs;

	public LayerMask groundLayer;

	UnityEngine.AI.NavMeshAgent agent;
	//今移動したいかどうか
	public bool move;
	public float walkRadius = 8.0f;

	// Use this for initialization
	void Start () {
		
		if(!targetTrigger){
			Debug.Log("You need to determine the [targetTrigger] value !!!");
		}
		//Componentの指定
		parameterCs = GetComponent<EnemyParameter>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		agent.updatePosition = false;
		agent.updateRotation = false;

		mySituation = AiSituation.Patrol;
		agent.enabled = true;

		agent.destination = parameterCs.spawnPoint.transform.position;
		coolDown = Random.Range(2.0f, 5.0f);
		agent.updateRotation = true;
		parameterCs.animator.SetBool("Move",true);

	}

	// Update is called once per frame
	void Update () {
		if(!parameterCs.canMove){

			if(mySituation != AiSituation.Paused){

				parameterCs.animator.SetBool("Move",false);
				parameterCs.animator.ResetTrigger("Attack");
				parameterCs.animator.SetInteger("AttackType", 0);

				target = null;
				coolDown = 0.0f;
				wantAttack = false;
				backOff = false;

				agent.enabled = false;
				mySituation = AiSituation.Paused;

				if(!parameterCs.curAnim.IsName("Hide")){
					if(target){
						parameterCs.animator.Play("Battle_Idle", 0, 0.0f);
					}else{
						parameterCs.animator.Play("Idle", 0, 0.0f);
					}
				}

			}

		}else{
			
			if(mySituation == AiSituation.Paused){
				
				mySituation = AiSituation.Patrol;
				agent.enabled = true;
			
			}else if(inAction){

				agent.updatePosition = false;
				agent.updateRotation = false;

				if(parameterCs.dead){
					this.enabled = false;
				}


			}else{
				if(parameterCs.gotDamage){

					GotDamage();

				}else{

					switch (mySituation) {
					case AiSituation.Patrol:
						Patrol();
						break;

					case AiSituation.InBattle:
						InBattle();
						break;

					case AiSituation.BackOff:
						BackOff();
						break;

					case AiSituation.Paused:
						
						break;
					}


				}


			}

			agent.nextPosition = transform.position;
			if(agent.nextPosition != transform.position){
				transform.position = agent.nextPosition;
			}

			////ターゲットの方向に向いていいなら
			if(target && faceTarget){
				//ターゲットの方向を計算
				Vector3 frwd;
				frwd = (target.position - transform.position).normalized;
				//ターゲットの方向に向く
				transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(frwd , Vector3.up), Time.deltaTime * 5.0f);

			}

			if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, 0.2f, groundLayer)){
				parameterCs.animator.SetBool("Ground", true);
			}else{
				parameterCs.animator.SetBool("Ground", false);
			}
		}
	}

	void Patrol(){
		if(targetTrigger.targetList.Count > 0){
			
			if(targetTrigger.targetList.Count > 1){
				target = targetTrigger.targetList[Random.Range(0, targetTrigger.targetList.Count - 1)].transform;
			}else{
				target = targetTrigger.targetList[0].transform;
			}

			agent.destination = target.position;

			//ターゲットの方向を計算
			Vector3 frwd;
			frwd = (target.position - transform.position).normalized;

			float front;
			front = Vector3.Dot(transform.forward, frwd);
			Debug.Log(front);

			if(front > 0.9f){
				mySituation = AiSituation.InBattle;
				coolDown = Random.Range(1.0f, 3.0f);
				parameterCs.animator.SetBool("InBattle", true);
				agent.updateRotation = false;
				parameterCs.animator.SetBool("Move",false);
			}
		}else{

			if(coolDown == 0){

				RandomDestinationUpdate();

			}else if(agent.remainingDistance <= agent.stoppingDistance){

				agent.updateRotation = false;
				parameterCs.animator.SetBool("Move",false);

				coolDown -= Time.deltaTime;
				coolDown = Mathf.Clamp(coolDown, 0, 10.0f);

			}else{
				agent.updateRotation = true;
				parameterCs.animator.SetBool("Move",true);
			}
		}
	}

	void InBattle(){
		if(inAttack){
			mySituation = AiSituation.BackOff;
			coolDown = 0.0f;
			backOff = true;

			if(faceTarget){
				if(targetTrigger.targetList.Count > 0){
					//ターゲットの方向を計算
					Vector3 dir = (targetTrigger.targetList[0].transform.position - transform.position).normalized;
					//ターゲットの方向に向く
					transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(dir , Vector3.up), Time.deltaTime * 10);
					transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
				}
			}
		}else if(wantAttack){
			
			agent.destination = target.position;

			coolDown -= Time.deltaTime;
			coolDown = Mathf.Clamp(coolDown, 0.0f, 10.0f);

			//ターゲットの方向を計算
			Vector3 frwd;
			frwd = (target.position - transform.position).normalized;

			float front;
			front = Vector3.Dot(transform.forward, frwd);

			if(agent.remainingDistance <= atkDistance[atkType]){
				parameterCs.animator.SetTrigger("Attack");
				atkType = Random.Range(0, atkDistance.Length);
				parameterCs.animator.SetInteger("AttackType", atkType);
				wantAttack = false;
				agent.updateRotation = false;
				parameterCs.animator.SetBool("Move",false);

			}else if(coolDown == 0){
				wantAttack = false;
				parameterCs.animator.SetBool("InBattle", false);
				coolDown = Random.Range(1.0f, 3.0f);
				mySituation = AiSituation.Patrol;
			}

		}else{
			coolDown -= Time.deltaTime;
			coolDown = Mathf.Clamp(coolDown, 0, 10.0f);

			if(coolDown == 0){

				UnityEngine.AI.NavMeshHit hit;
				UnityEngine.AI.NavMesh.SamplePosition(target.position, out hit, walkRadius, 1);
				agent.destination = hit.position;
				wantAttack = true;
				agent.updateRotation = true;
				parameterCs.animator.SetBool("Move",true);
				coolDown = 10.0f;
			}
		}

	}

	void BackOff(){
		if(coolDown == 0){
			if(!inAttack){
				if(backOff){
					BackOffDestinationUpdate();
				}else{
					if(target.gameObject){
						mySituation = AiSituation.InBattle;
						coolDown = 10.0f;
						agent.updateRotation = true;
						parameterCs.animator.SetBool("Move",true);
					}else{
						mySituation = AiSituation.Patrol;
						target = null;
					}
				}
			}


		}else if(agent.remainingDistance <= agent.stoppingDistance){
			if(backOff){
				agent.destination = target.position;
				backOff = false;
				wantAttack = true;
			}else{
				agent.updateRotation = false;
			}
		}else if(wantAttack){
			////ターゲットの方向に向いていいなら
			if(target){
				agent.destination = target.position;
				//ターゲットの方向を計算
				Vector3 frwd;
				frwd = (target.position - transform.position).normalized;

				float front;
				front = Vector3.Dot(transform.forward, frwd);
				//Debug.Log(front);
				if(front > 0.8f){
					parameterCs.animator.SetBool("Move",false);
				}

			}else{
				parameterCs.animator.SetBool("Move",false);
				mySituation = AiSituation.Patrol;
				target = null;
			}
			coolDown -= Time.deltaTime;
			coolDown = Mathf.Clamp(coolDown, 0, 10.0f);
		}
	}
	//攻撃GameObjectの再生
	public void AttackEffect (int index) {
		GameObject.Instantiate (Resources.Load (effName[index]), effPosition[index].position, effPosition[index].rotation);
	}
	public void AttackEffectChild (int index) {
		GameObject.Instantiate (Resources.Load (effName[index]), effPosition[index].position, effPosition[index].rotation);
	}
	void BackOffDestinationUpdate () {
		Vector3 newDirection = transform.position - (transform.forward * Random.Range(4.0f, 6.0f));
		newDirection = Quaternion.Euler(0, Random.Range(-10, 10), 0) * newDirection;
		UnityEngine.AI.NavMeshHit hit;
		UnityEngine.AI.NavMesh.SamplePosition(newDirection, out hit, walkRadius, 1);
		agent.destination = hit.position;
		coolDown = Random.Range(3.0f, 5.0f);
		agent.updateRotation = true;
		parameterCs.animator.SetBool("Move",true);
		backOff = true;
	}

	void RandomDestinationUpdate () {
		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
		randomDirection += transform.position;
		UnityEngine.AI.NavMeshHit hit;
		UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
		agent.destination = hit.position;
		coolDown = Random.Range(3.0f, 5.0f);
		agent.updateRotation = true;
		parameterCs.animator.SetBool("Move",true);
	}
	void GotDamage () {

		mySituation = AiSituation.Patrol;
		parameterCs.animator.SetBool("Move",false);
		target = null;
		coolDown = 0.0f;
		agent.updateRotation = false;
		wantAttack = false;
		backOff = false;

		parameterCs.gotDamage = false;

	}
}
