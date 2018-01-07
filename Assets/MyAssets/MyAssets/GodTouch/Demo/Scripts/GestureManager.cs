using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GodTouches
{
	/// <summary>
	/// 「タップ」「ダブルタップ」「長押し」「スライド」「フリック」　スクリプト
	/// </summary>
	public class GestureManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		/// <summary>
		/// イベントタイプ
		/// </summary>
		public enum EventType { None, Touch, Tap, W_Tap, Hold, Slide, Flick}

		/// <summary>
		/// クリック・ドラッグ判定距離
		/// </summary>
		public float CheckDistance = 0.1f;
		/// <summary>
		/// 長押し判定時間
		/// </summary>
		public float CheckTime = 0.3f;
		/// <summary>
		/// サンプル表示テキスト
		/// </summary>
		public Text typeText;
		/// <summary>
		/// UIの表示に使うカメラ
		/// </summary>
		public Camera renderCam;
		/// <summary>
		/// タッチ時にパーティクルを表示するかどうか
		/// </summary>
		public bool showParticle;
		/// <summary>
		/// 表示パーティクル
		/// </summary>
		GameObject myParticle;
		[HideInInspector]
		/// <summary>
		/// イベントタイプ
		/// </summary>
		public EventType type;
		/// <summary>
		/// イベント実行中かどうか
		/// </summary>
		bool isRunning;
		/// <summary>
		/// 押された時の開始ポジション
		/// </summary>
		Vector3 startPos;
		/// <summary>
		/// 押された時の開始時間
		/// </summary>
		float startTime;
		/// <summary>
		/// 前回のフレームでのポジション
		/// </summary>
		Vector3 lastPos;
		/// <summary>
		/// 離された時のポジション
		/// </summary>
		Vector3 endPos;
		/// <summary>
		/// 離された時の開始時間
		/// </summary>
		float endTime;
		/// <summary>
		/// 押さえた時間
		/// </summary>
		float holdingTime;
		/// <summary>
		/// フレーム間の移動量
		/// </summary>
		Vector3 deltaMoving;
		/// <summary>
		/// 開始位置との距離
		/// </summary>
		Vector3 movedDistance;
		[HideInInspector]
		/// <summary>
		/// フリックパラメーター（方向Ｘ, 方向Ｙ, 移動距離）
		/// </summary>
		public Vector3 flickParam;
		/// <summary>
		/// イベント実行中かどうか
		/// </summary>
		public bool IsRunning { get { return isRunning; } }

		public Slider settingSlider;

		/// <summary>
		/// イベントタイプ設定
		/// </summary>
		/// <param name="type">イベントタイプ</param>
		void SetType(EventType type)
		{
			this.type = type;
			if(typeText)
				typeText.text = type.ToString ();
		}

		/// <summary>
		/// 押された
		/// </summary>
		/// <param name="e">PointerEventData</param>
		public void OnPointerDown (PointerEventData e)
		{
			if(isRunning)
				return;
			
			if (type != EventType.Tap)
				SetType(EventType.Touch);
			isRunning = true;
			startPos = renderCam.ScreenToWorldPoint(GodTouch.GetPosition () + renderCam.transform.forward);
			startTime = Time.time;
		}
		/// <summary>
		/// 更新処理
		/// </summary>
		void Update()
		{
			if (type == EventType.Touch || isRunning)
			{
				// 押されてる
				var pos = renderCam.ScreenToWorldPoint(GodTouch.GetPosition () + renderCam.transform.forward);
				movedDistance.x  = Mathf.Abs(pos.x - startPos.x);
				movedDistance.y  = Mathf.Abs(pos.y - startPos.y);
				deltaMoving.x  = Mathf.Abs(pos.x - lastPos.x);
				deltaMoving.y  = Mathf.Abs(pos.y - lastPos.y);
				holdingTime  = Time.time - startTime;

				if(holdingTime > CheckTime && type != EventType.Slide && type != EventType.Hold)
				{
					if(movedDistance.x > CheckDistance || movedDistance.y > CheckDistance)
					{
						// 一定距離動いていたらドラッグ実行
						SetType(EventType.Slide);

						if(showParticle){
							if(myParticle && myParticle.name != "Slide")
								Destroy(myParticle);

							myParticle = GameObject.Instantiate(Resources.Load("Slide"), pos, Quaternion.identity) as GameObject;
							myParticle.name = "Slide";
								
						}
					}else{
						// 一定時間経過していたら長押し実行
						SetType(EventType.Hold);

						if(showParticle){
							if(myParticle && myParticle.name != "Hold")
								Destroy(myParticle);

							myParticle = GameObject.Instantiate(Resources.Load("Hold"), pos, Quaternion.identity) as GameObject;
							myParticle.name = "Hold";
						}
					}
				}

				if(myParticle){
					if(myParticle.name == "Hold" || myParticle.name == "Slide"){
						myParticle.transform.position = pos;
					}
				}
			}
			else if(!isRunning){
				float ndt  = Time.time - endTime;
				if(ndt > CheckTime)
				{
					// イベント初期化
					SetType (EventType.None);
					isRunning = false;
					endTime = Time.time;

					if(myParticle){
						if(myParticle.name == "Slide" || myParticle.name == "Hold"){
							Destroy(myParticle);
							myParticle = null;
						}
					}
				}
			}
			else if(type != EventType.None){
				// イベント初期化
				SetType (EventType.None);
				isRunning = false;
				endTime = Time.time;
				if(myParticle){
					if(myParticle.name == "Slide" || myParticle.name == "Hold"){
						Destroy(myParticle);
						myParticle = null;
					}
				}
			}
			lastPos = renderCam.ScreenToWorldPoint(GodTouch.GetPosition () + renderCam.transform.forward);
		}
		/// <summary>
		/// 離された
		/// </summary>
		/// <param name="e">PointerEventData</param>
		public void OnPointerUp (PointerEventData e)
		{
			if(!isRunning)
				return;
			
			endPos = lastPos;

			if (type == EventType.Touch)
			{
				// 他のイベントが未入力ならクリック実行
				SetType (EventType.Tap);
				if(showParticle){
					if(myParticle)
						Destroy(myParticle);

					myParticle = GameObject.Instantiate(Resources.Load("Tap"), endPos, Quaternion.identity) as GameObject;
					myParticle.name = "Tap";
				}
			}
			else if (type == EventType.Tap)
			{
				// 他のイベントが未入力ならクリック実行
				SetType (EventType.W_Tap);
				if(showParticle){
					if(myParticle){
						if(myParticle)
							Destroy(myParticle);

						myParticle = GameObject.Instantiate(Resources.Load("W_Tap"), endPos, Quaternion.identity) as GameObject;
						myParticle.name = "W_Tap";
					}
				}
			}

			if(holdingTime <= CheckTime)
			{
				flickParam = (startPos - endPos).normalized;
				flickParam = new Vector3(flickParam.x, flickParam.y, Vector3.Distance(startPos, lastPos));

				Debug.Log("Distnce is :" + flickParam.z);

				if(flickParam.z > CheckDistance)
				{
					// 一定距離動いていたらドラッグ実行
					SetType(EventType.Flick);
					if(showParticle){
						if(myParticle){
							if(myParticle)
								Destroy(myParticle);

							myParticle = GameObject.Instantiate(Resources.Load("Flick"), startPos, Quaternion.LookRotation((endPos - startPos).normalized, transform.forward)) as GameObject;
							myParticle.name = "Flick";

						}
					}
				}
			}
			else
			{
				// イベント初期化
				SetType (EventType.None);

				if(myParticle)
					Destroy(myParticle);
			}
			isRunning = false;
			endTime = Time.time;
		}

		/// <summary>
		/// パネル上から出た
		/// </summary>
		/// <param name="e">PointerEventData</param>
		public void OnPointerExit (PointerEventData e)
		{
			if(!isRunning)
				return;
			
			endPos = lastPos;

			if (type == EventType.Touch)
			{
				// 他のイベントが未入力ならクリック実行
				SetType (EventType.Tap);
				if(showParticle){
					if(myParticle)
						Destroy(myParticle);
					
					myParticle = GameObject.Instantiate(Resources.Load("Tap"), endPos, Quaternion.identity) as GameObject;
					myParticle.name = "Tap";
				}
			}
			else if (type == EventType.Tap)
			{
				// 他のイベントが未入力ならクリック実行
				SetType (EventType.W_Tap);
				if(showParticle){
					if(myParticle){
						if(myParticle)
							Destroy(myParticle);
						
						myParticle = GameObject.Instantiate(Resources.Load("W_Tap"), endPos, Quaternion.identity) as GameObject;
						myParticle.name = "W_Tap";
					}
				}
			}

			if(holdingTime <= CheckTime)
			{
				flickParam = (startPos - endPos).normalized;
				flickParam = new Vector3(flickParam.x, flickParam.y, Vector3.Distance(startPos, lastPos));

				Debug.Log("Distnce is :" + flickParam.z);

				if(flickParam.z > CheckDistance)
				{
					// 一定距離動いていたらドラッグ実行
					SetType(EventType.Flick);
					if(showParticle){
						if(myParticle){
							if(myParticle)
								Destroy(myParticle);

							myParticle = GameObject.Instantiate(Resources.Load("Flick"), endPos, Quaternion.LookRotation((startPos - endPos).normalized, -transform.forward)) as GameObject;
							myParticle.name = "Flick";

						}
					}
				}
			}
			else
			{
				// イベント初期化
				SetType (EventType.None);

				if(myParticle)
					Destroy(myParticle);
			}
			isRunning = false;
			endTime = Time.time;
		}
		//PrefsManager と Slider を使って設定のセーブ (SmartController が呼ぶ)
		public void LoadCheckDistance(){
			if(PrefsManager.HasKey(settingSlider.name)){
				CheckDistance = PrefsManager.GetValue<float>(settingSlider.name);
			}
			settingSlider.value = CheckDistance;
		}
		//PrefsManager と Slider を使って設定のロード (SmartController が呼ぶ)
		public void SaveSettings(){
			CheckDistance = settingSlider.value;
			PrefsManager.SetValue<float>(settingSlider.name, CheckDistance);
		}
	}
}
