using UnityEngine;
using System.Collections;

public class VolumeFade : MonoBehaviour {

	//Start()にFadeしたい時用
	public bool fadeOnStart = false;
	//Startフェードにかかる時間
	public float startFadeTime = 2.0f;
	//通常はFadeInだけどFadeOutにしたい時用
	private bool fadeOut = false;
	//このImageの最大Volume値
	public float maxVolume = 1.0f;
	//経過時間の計算に使う
	private float timer;
	//フェードにかかる時間（秒）
	private float duration;
	//AudioSourceのアクセス
	private AudioSource myAudio;

	// Use this for initialization
	void Awake () {
		//maxVolume値の修正
		maxVolume = Mathf.Clamp (maxVolume, 0.01f, 1.0f);
		//経過時間の計算用に
		float newVolume;
		//FadeInかFadeOutによって計算違う
		if(!fadeOut){
			newVolume = 0.0f;
		}else{
			newVolume = 1 * maxVolume;
		}
		//AudioSourceのアクセス
		myAudio = transform.GetComponent<AudioSource> ();
		//volume値をセット
		myAudio.volume = newVolume;

		//もしStartでFadeするなら
		if(fadeOnStart){
			FadeAction (startFadeTime);
		}
	}

	// Update is called once per frame
	void Update () {
		//まだエフェクト中なら
		if (duration != 0 && duration != 1) {
			//経過時間の計算用に
			float newVolume;
			//FadeInかFadeOutによって計算違う
			if(!fadeOut){
				timer += Time.deltaTime;
			}else{
				timer -= Time.deltaTime;
			}
			//新しいのvolume値の計算
			newVolume = Mathf.Clamp (timer / duration, 0.0f, 1.0f);
			//volume値をセット
			myAudio.volume = newVolume * maxVolume;

			//もしエフェクトが終わりなら各値のリセット
			if (newVolume == 1 || newVolume == 0) {
				timer = 0.0f;
				duration = 0.0f;
			}
		}
	}

	//外部呼び出し用
	public void FadeAction (float fadingTime) {
		//FadeInかFadeOutによって計算違う
		if(fadingTime >= 0){
			fadeOut = false;
			//フェードにかかる時間（秒）をセット
			timer = 0;
			duration = fadingTime;

		}else{
			fadeOut = true;
			//フェードにかかる時間（秒）をセット
			timer = -fadingTime;
			duration = -fadingTime;
		}
	}
}
