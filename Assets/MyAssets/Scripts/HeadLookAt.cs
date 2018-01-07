using UnityEngine;
using System.Collections;

public class HeadLookAt : MonoBehaviour {

	Animator anim;
	public float weigthValue;
	public float bodyValue;
	public float headValue;

	public float smooth;

	public Transform target;

	public LayerMask handRayLayer;
	public float handOffset = 0.1f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	void LateUpdate () {


		if (target) {

			Vector3 frwd = (target.position - transform.position).normalized;

			bodyValue = Mathf.Lerp (bodyValue, 1.0f, Time.deltaTime * smooth);

			float frontBack;
			frontBack = Vector3.Dot(frwd, transform.forward);

			//Debug.Log (frontBack);

			if(frontBack > 0){

				weigthValue = Mathf.Lerp (weigthValue, 1.0f, Time.deltaTime * smooth);

			}else{

				weigthValue = Mathf.Lerp (weigthValue, 0.0f, Time.deltaTime * smooth);
			}


		} else {

			weigthValue = Mathf.Lerp (weigthValue, 0.0f, Time.deltaTime * smooth);

		}

	}
	void OnAnimatorIK () {

		if (target) {
			anim.SetLookAtPosition (target.position);
		}

		anim.SetLookAtWeight (weigthValue, bodyValue, headValue);
	}
}