using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (SubActionMng))]

public class BasicMatchTarget : MonoBehaviour {
	public enum BodyParts
	{
		Head, Hip, Right_Hand, Left_Hand, Right_Foot, Left_Foot
	}
	[Tooltip("各部位(Bone)のリスト")]
	public Transform[] myBody;
	//[Tooltip("到達したい位置")]
	//public Transform macthPoint;
	public List<Transform> targetList;
	[Tooltip("到達させたい部位")]
	public BodyParts targetPart;
	[Tooltip("自身のtransform.rotationをmacthPointのtransform.rotationと同じにするなら　1.0（アニメーションで設定）")]
	public float rotWeigth;
	//向きの計算用
	Vector3 rotOffSet;
	[Tooltip("myBodyのtransform.positionをmacthPointのtransform.positionと同じにするなら　1.0（アニメーションで設定）")]
	public Vector3 posiWeigth;
	//位置の計算用
	Vector3 posiOffSet;

	Rigidbody myRigidbody;

	Vector3 lastPosi;
	Quaternion LastRot;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody>();
	}

	void Update() {
		if(targetList.Count > 1)
			targetList.Sort(SortByDistance);

		if(targetList.Count > 0 && posiWeigth != Vector3.zero){
			lastPosi = targetList[0].position;
		}

		if(targetList.Count > 0 && rotWeigth > 0){
			LastRot = targetList[0].rotation;
		}
	}

	void FixedUpdate() {

		//もし　各posiWeigth　が　0　じゃないなら
		if(posiWeigth != Vector3.zero && lastPosi != Vector3.zero){
			//部位の位置の取得
			posiOffSet = myBody[(int)targetPart].position;
			//自身と部位との位置の差を,　　macthPointと自身の位置の差を同じに
			posiOffSet = (lastPosi + (transform.position - posiOffSet)) - transform.position;
			//各posiWeigth　で各位置の適用率を掛け合わせる
			posiOffSet = new Vector3 (posiOffSet.x * posiWeigth.x, posiOffSet.y * posiWeigth.y, posiOffSet.z * posiWeigth.z);
			//最終的な位置
			Vector3 goalPoint = transform.position + posiOffSet;
			//自身を移動
			myRigidbody.MovePosition(goalPoint);
		}else{
				lastPosi = Vector3.zero;
		}
		//もし　rotWeigth　が　0　じゃないなら
		if(rotWeigth > 0 && LastRot != Quaternion.identity){
			//自身の向きと　macthPoint　の向きの差
			rotOffSet = LastRot.eulerAngles - transform.rotation.eulerAngles;
			//向きの適用率
			rotOffSet = new Vector3 (rotOffSet.x * rotWeigth, rotOffSet.y * rotWeigth, rotOffSet.z * rotWeigth);
			//最終的な向き
			Vector3 goalRotation = transform.rotation.eulerAngles + rotOffSet;
			//自身を回転
			transform.rotation = Quaternion.Euler(goalRotation);
		}else{
			LastRot = Quaternion.identity;
		}
	}
	private int SortByDistance(Transform a , Transform b){
		return Vector3.Distance(myBody[(int)targetPart].position, a.position).CompareTo(Vector3.Distance(myBody[(int)targetPart].position, b.position));
	}
}
