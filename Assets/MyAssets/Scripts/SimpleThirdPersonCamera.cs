using UnityEngine;
using System.Collections;

public class SimpleThirdPersonCamera : MonoBehaviour
{
	public float distanceAway;			// distance from the back of the craft
	public float distanceUp;			// distance above the craft
	public float smooth;				// how smooth the camera movement is
	
	private Vector3 targetPosition;		// the position the camera is trying to be in
	private Quaternion rot;
	public Transform follow;
	
	public LayerMask rayCheckMsk;

	public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis 
	public float xRotLimit = 20.0f;
	private float xRot;

	private bool isRotating;	// Is the camera being rotated? 
	private Touch touchState;


	void Start(){
				
		if(!follow){
			follow = GameObject.FindWithTag ("Player").transform;
		}
	}
	
	void FixedUpdate ()
	{	
				NormalModeCameraMovement();
		
	}
		void NormalModeCameraMovement(){
				
				rot = Quaternion.Euler(xRot, transform.rotation.eulerAngles.y, 0);

				TouchMovement();

				// setting the target position to be the correct offset
				targetPosition = follow.position + (rot * new Vector3(0, distanceUp, -distanceAway));

				// making a smooth transition between it's current position and the position it wants to be in
				//targetPosition = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * smooth * 2f);

				RaycastHit hit;

				if(Physics.Linecast(follow.position, targetPosition, out hit, rayCheckMsk)){

						Vector3 frwd;
						frwd = (follow.position - transform.position).normalized;

						float side;
						side = Vector3.Dot(frwd, follow.right);

						Debug.Log(side);

						if(side > 0.1f){
								targetPosition = Vector3.Slerp(transform.position, hit.point + transform.right, Time.deltaTime * smooth);
						}
						else if(side < -0.1f){
								targetPosition = Vector3.Slerp(transform.position, hit.point - transform.right, Time.deltaTime * smooth);
						}else{
								targetPosition = Vector3.Slerp(transform.position, hit.point, Time.deltaTime * smooth);
						}
				}

				transform.position = targetPosition;
				// make sure the camera is looking the right way!
				//rot = Quaternion.LookRotation(follow.position - transform.position);

				//transform.rotation = Quaternion.Slerp(transform.rotation, rot, smooth);

				transform.LookAt(follow);
		}

		void TouchMovement (){
				if(Input.touchCount > 0){
				touchState = Input.GetTouch(0);

				// Handle finger movements based on touch phase.
				switch (touchState.phase) {
				// Record initial touch position.
				case TouchPhase.Began:

						break;

						// Determine direction by comparing the current touch position with the initial one.
				case TouchPhase.Moved:
								float turnY;
								turnY = Mathf.Lerp(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y + touchState.deltaPosition.x, turnSpeed);

								xRot = Mathf.Lerp(xRot, xRot + touchState.deltaPosition.y, turnSpeed);
								if(xRot > xRotLimit){
										xRot = xRotLimit;
								}else if(xRot < -xRotLimit){
										xRot = -xRotLimit;
								}
								rot = Quaternion.Euler(xRot, turnY, 0);
						break;

						// Report that a direction has been chosen when the finger is lifted.
				case TouchPhase.Ended:

						break;
				}
			}

		}
}
