using UnityEngine;
using System.Collections;

public class RayToTheGround : MonoBehaviour {

	//[Layer Mask] para definir com qual [layer] o [RayCast] do pé irá colidir
	//[LayerMask] to define with layer my foot [RayCast] will collide
	public LayerMask rayLayer;
	//Para fazer alterar o valor suavemente
	//To make change the value smoothly
	public float smooth = 10;
	
	// Update is called once per frame
	void Update () {
		
		//Criar esse valor para usar o [RaycastHit]
		//Create this value to use the [RaycastHit]
		RaycastHit hit;

		//[RayCast] para o chão, pra saber a distancia
		//[RayCast] to the ground, to know the distance
		if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5.0f, rayLayer))
		{
			//Mostrar o [Ray]
			//Show [Ray]
			Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow);
			if (transform.parent) {
				if (hit.point.y < transform.parent.position.y) {
					//Definir a nova posição do IK
					//Set the new position of IK
					transform.position = Vector3.Lerp (transform.position, hit.point, Time.deltaTime * smooth);
				}
			} else {
				//Definir a nova posição do IK
				//Set the new position of IK
				transform.position = Vector3.Lerp (transform.position, hit.point, Time.deltaTime * smooth);
			}
		}else{
			transform.position = transform.position - (-transform.up * 10);
		}				
	}
}
