using UnityEngine;
using System.Collections;

public class MoveDirection : MonoBehaviour {
	//これを使ってこのスクリプトを一時停止させたりする
	private bool stop = true;
	//傾きセンサーの値を保存する為
	public Vector2 myGyro;
	//Animatorなどの移動関係のParameter用に
	public bool canMove;
	//この値でどのくらい垂直にするとキャラクターが止まるのかを設定する
	public float deadZone = 0.1f;
	//端末が垂直のままではプレー出来ないので　オフセットで奥に傾けるようにする
	public float zOffcet = 0.6f;
	//キャラの足元に表示してる輪
	private SpriteRenderer ringColor;
	//キャラの足元に表示してる矢印
	private SpriteRenderer ballColor;
	//矢印を移動させたりしたいので
	private Transform ball;
	//傾き加減応じて移動可能なら緑色 移動不可なら赤色
	public Gradient myColor;
	//Gradient　の表示色の計算に
	public float lerpedValue;
	//メインカメラから見た傾き方向を計算する為
	public Transform MainCam;

	// Use this for initialization
	void Start () {
		//各Valorの指定
		ringColor =  transform.Find("Ring").GetComponent<SpriteRenderer>();
		ball =  transform.Find("Ball");
		ballColor =  ball.Find("Plane").GetComponent<SpriteRenderer>();
		MainCam = Camera.main.transform;
		//このスクリプトを一時停止
		CanNotMove();
	}
	
	// Update is called once per frame
	void Update () {
		//一時停止中なら何もしない
		if(stop)
			return;
		
		//傾き加減の取得　zOffcetでプレーに最適な角度に原点を調節
		myGyro = new Vector2(Input.acceleration.x, -Input.acceleration.z - zOffcet);
		//myGyro.magnitudeに応じて移動可能なら緑色 移動不可なら赤色
		Vector2 maxGyro = new Vector2((1 - zOffcet), (1 - zOffcet));
		//deadZone　以上ならキャラは動ける
		if(myGyro.magnitude >= deadZone){
			canMove = true;
			//傾き加減をプレイヤーが知る為に色で表現　強いほど緑
			lerpedValue = myGyro.magnitude / maxGyro.magnitude;
			ringColor.color = myColor.Evaluate(Mathf.Clamp(lerpedValue, 0.0f, 1.0f));
			ballColor.color = ringColor.color;

		//以下ならキャラは動けない
		}else{
			canMove = false;
			//傾き加減をプレイヤーが知る為に色で表現　停止は赤
			lerpedValue = Mathf.Lerp(lerpedValue, 0.0f, Time.time);
			lerpedValue = Mathf.Clamp (lerpedValue, 0.0f, 1.0f);
			ringColor.color = myColor.Evaluate(lerpedValue);
			ballColor.color = ringColor.color;
		}
		//メインカメラから見た傾き方向にこのGameObjectを向ける
		Quaternion camRot = MainCam.rotation;
		camRot = Quaternion.Euler(0, camRot.eulerAngles.y, 0);
		transform.rotation = Quaternion.LookRotation(camRot * new Vector3(myGyro.x, 0, myGyro.y), Vector3.up);

		//#if UNITY_ANDROID
		//傾き加減を見やすくする為　Ball GameObjectの位置をmyGyroの値に応じて移動させる　*2なのは調節の結果
		ball.localPosition = new Vector3(0, 0, Mathf.Min(myGyro.magnitude * 2, 0.5f));
		//傾き加減を表現するために大きくしたりする為　*3なのは調節の結果
		ball.localScale = Vector3.one * Mathf.Clamp(myGyro.magnitude * 3, 0.5f, 1);
		//Debug.Log (myGyro.x + " + " + myGyro.y + " + " + myGyro.magnitude);


		//#endif
		//#if UNITY_EDITOR
		//		myGyro.x = Input.GetAxis("Horizontal");
		//		myGyro.y = Input.GetAxis("Vertical");
		//	#endif
	}
	//外部呼出し用で　このスクリプトを一時停止
	public void CanNotMove(){
		stop = true;

		ringColor.color = Color.white;
		ballColor.color = Color.white;

		transform.localRotation = Quaternion.identity;
		ball.transform.localPosition = Vector3.zero;
	}
	//外部呼出し用で　このスクリプトの一時停止を解除
	public void CanMove(){
		stop = false;
	}
}
