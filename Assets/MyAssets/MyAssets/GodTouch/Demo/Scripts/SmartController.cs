using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SmartController : MonoBehaviour {
	[HideInInspector]
	public Animator animator;

	public int smooth = 5;
	public float deadZone = 0.1f;
	public float zOffcet = 0.2f;

	//傾きセンサーの値を保存する為
	public Vector2 myGyro;

	bool grounded;

	GodTouches.GestureManager gesManager;
	GodTouches.GestureManager.EventType lastType;

	public LayerMask rayLayer;

	[HideInInspector]
	public MoveDirection moveDir;

	public bool faceDir;

	TargetTrigger targetTrigger;
	public bool faceTar;

	public Slider settingSlider;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();

		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
		
		gesManager = GameObject.Find("GestureManager").GetComponent<GodTouches.GestureManager>();

		if(PrefsManager.HasKey(settingSlider.name)){
			zOffcet = PrefsManager.GetValue<float>(settingSlider.name);
		}
		settingSlider.value = zOffcet;

		moveDir = GetComponentInChildren<MoveDirection>();
		targetTrigger = GetComponentInChildren<TargetTrigger>();
	}

	// Update is called once per frame
	void Update () 
	{

		if (animator)
		{
			Vector3 forwardDir = moveDir.MainCam.transform.TransformDirection(Vector3.forward);
			forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;
			Vector3 rightDir = new Vector3(forwardDir.z, 0, -forwardDir.x);
			Vector3 movement = (rightDir * moveDir.myGyro.x) + (forwardDir * moveDir.myGyro.y);

			if(moveDir.canMove){
				if(targetTrigger.targetList.Count > 0 && faceTar){
					//ターゲットの方向を計算
					rightDir = (targetTrigger.targetList[0].transform.position - transform.position).normalized;
					//ターゲットの方向に向く
					transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(rightDir , Vector3.up), Time.deltaTime * smooth);
					transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

				}else if(movement != Vector3.zero && faceDir){
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), Time.deltaTime * smooth);
				}
				animator.SetBool("Move", true);
				animator.SetFloat("MoveSpeed", 1.0f);
			}else{
				animator.SetBool("Move", false);
				animator.SetFloat("MoveSpeed", 0.0f);
			}



			switch (gesManager.type) {
			case GodTouches.GestureManager.EventType.None:
				if(lastType != gesManager.type){
					animator.ResetTrigger("Tap");
					animator.ResetTrigger("W_Tap");
					animator.ResetTrigger("Flick");
					animator.SetBool("Hold", false);
				}
				break;

			case GodTouches.GestureManager.EventType.Tap:
				if(lastType != gesManager.type){
					animator.SetTrigger("Tap");
					animator.ResetTrigger("W_Tap");
					animator.ResetTrigger("Flick");
					animator.SetBool("Hold", false);
				}
				break;

			case GodTouches.GestureManager.EventType.W_Tap:
				if(lastType != gesManager.type){
					animator.ResetTrigger("Tap");
					animator.SetTrigger("W_Tap");
					animator.ResetTrigger("Flick");
					animator.SetBool("Hold", false);
				}
				break;

			case GodTouches.GestureManager.EventType.Slide:
				if(lastType != gesManager.type){
					animator.ResetTrigger("Tap");
					animator.ResetTrigger("W_Tap");
					animator.ResetTrigger("Flick");
					animator.SetBool("Hold", false);
				}
				break;

			case GodTouches.GestureManager.EventType.Flick:
				if(lastType != gesManager.type){
					animator.ResetTrigger("Tap");
					animator.ResetTrigger("W_Tap");
					animator.SetTrigger("Flick");
					animator.SetFloat("Flick_X", gesManager.flickParam.x);
					animator.SetFloat("Flick_Y", gesManager.flickParam.y);
					animator.SetBool("Hold", false);
				}
				break;

			case GodTouches.GestureManager.EventType.Hold:
				if(lastType != gesManager.type){
					animator.ResetTrigger("Tap");
					animator.ResetTrigger("W_Tap");
					animator.ResetTrigger("Flick");
					animator.SetBool("Hold", true);
				}
				break;
			}
			lastType = gesManager.type;
		}
		Debug.DrawLine(transform.position + Vector3.up, (transform.position + Vector3.up) - (transform.up * 1.2f), Color.red);
		if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.2f, rayLayer))
		{
			animator.SetBool("Ground", true);
		}else{
			animator.SetBool("Ground", false);
		}

	}

	public void SaveSettings(){
		zOffcet = settingSlider.value;
		PrefsManager.SetValue<float>(settingSlider.name, zOffcet);
	}
}
