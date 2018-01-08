using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.Events;

public class UI_Animator : MonoBehaviour {
	[Tooltip("（『見せる』『隠す』の切り替え）")]
	public bool show = false;

	[Tooltip("演出の掛かる時間）")]
	public float duration = 0.0f;


	[Tooltip("演出用のカーブ）")]
	public AnimationCurve testCurve;

	[Tooltip("デバッグ用のスライダー）")]
	[Range(0.0f,1.0f)]
	public float transmition = 0.0f;

	[System.Serializable]
	public class ShowSettings
	{
		[Tooltip("『見せてる』時のRectTransform）")]
		public RectTransform myTransform;

		[Tooltip("ロードするエフェクト名")]
		public string effect;

		[HideInInspector]
		public bool finish;

		[Header("演出完了時に呼び出す")]
		public UnityEvent OnFinishedShow;

	}
	[Tooltip("『見せる』時の設定")]
	public ShowSettings showSettings;

	[System.Serializable]
	public class HideSettings
	{
		[Tooltip("『隠れてる』時のRectTransform）")]
		public RectTransform myTransform;

		[Tooltip("ロードするエフェクト名")]
		public string effect;

		[HideInInspector]
		public bool finish;

		[Header("演出完了時に呼び出す")]
		public UnityEvent OnFinishedHide;

	}
	[Tooltip("『隠す』時の設定")]
	public HideSettings hideSettings;

	//アルファ値などの操作の為に使う
	CanvasGroup cGroup;

	RectTransform myRect;

	// Use this for initialization
	void Start () {
		myRect = GetComponent<RectTransform> ();

		cGroup = GetComponent<CanvasGroup> ();

		UpdateRectTransform (transmition);
	}
	
	// Update is called once per frame
	void Update () {
		if(show){
			if (!showSettings.finish) {
				transmition += Time.deltaTime / duration;

				if (transmition > 1) {
					transmition = 1.0f;
					showSettings.finish = true;
					showSettings.OnFinishedShow.Invoke();
				}

				hideSettings.finish = false;
			}
		}else{
			
			if (!hideSettings.finish) {
				transmition -= Time.deltaTime / duration;

				if (transmition < 0) {
					transmition = 0.0f;
					hideSettings.finish = true;
					hideSettings.OnFinishedHide.Invoke();
				}
				showSettings.finish = false;
			}
		}

		UpdateRectTransform (transmition);
	}


	public void UpdateRectTransform (float curTransmition) {
		float cValue = testCurve.Evaluate (curTransmition);

		cGroup.alpha = cValue;

		myRect.position = Vector3.Lerp (hideSettings.myTransform.position, showSettings.myTransform.position, cValue);
		myRect.rotation = Quaternion.Lerp (hideSettings.myTransform.rotation, showSettings.myTransform.rotation, cValue);
		myRect.anchorMax = Vector2.Lerp (hideSettings.myTransform.anchorMax, showSettings.myTransform.anchorMax, cValue);
		myRect.anchorMin = Vector2.Lerp (hideSettings.myTransform.anchorMin, showSettings.myTransform.anchorMin, cValue);
		myRect.pivot = Vector2.Lerp (hideSettings.myTransform.pivot, showSettings.myTransform.pivot, cValue);
		myRect.localScale = Vector3.Lerp (hideSettings.myTransform.localScale, showSettings.myTransform.localScale, cValue);
	}

}
