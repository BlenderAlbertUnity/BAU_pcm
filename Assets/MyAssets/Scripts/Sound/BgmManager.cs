using UnityEngine;
using System.Collections;

public class BgmManager : MonoBehaviour {

	private AudioSource bgmPlayer;
	public AudioClip curBGM;
	public AudioClip nextBGM;
	//Sceneを変更する時にFadeOut用
	private bool fadeOut = false;
	private float bgmFadeTime = 2.0f;
	private AudioClip defaltBGM;

	// Use this for initialization
	void Start() {
		if(!bgmPlayer)
			return;
		bgmPlayer = GetComponent<AudioSource>();

		if(bgmPlayer.clip){
			defaltBGM = bgmPlayer.clip;
			ChangeBGM(defaltBGM, bgmFadeTime);
		}
	}

	// Use this for initialization
	public void StartBgmManager () {

		bgmPlayer = GetComponent<AudioSource>();

		if(bgmPlayer.clip){
			defaltBGM = bgmPlayer.clip;
			ChangeBGM(defaltBGM, bgmFadeTime);
		}
	}

	// Update is called once per frame
	void Update () {
		if (nextBGM) {
			ChangingBGM ();
		}
		if(fadeOut){
			//新しいのvolume値の計算
			bgmPlayer.volume = Mathf.Clamp (bgmPlayer.volume - (Time.deltaTime / bgmFadeTime), 0.0f, 1.0f);
			//もしVolumeが0ならBGMを止める
			if(bgmPlayer.volume == 0){
				//念のため確実に再生を止める
				bgmPlayer.Stop();
				//FadeOutしない
				fadeOut = false;
			}
		}
	}

	//外部アクセスからBGMを変えたい時に呼び出し
	public void ChangeBGM(AudioClip newBGM, float fadeDuration){
		//再生したいBGMを設定
		nextBGM = newBGM;
		//Fadeにかかる秒数を設定
		bgmFadeTime = fadeDuration;
	}
	//外部アクセスからVolumeを0にする時に呼び出し
	public void FadeOut(float fadeDuration){
		//FadeOut開始
		fadeOut = true;
		//Fadeにかかる秒数を設定
		bgmFadeTime = fadeDuration;
	}

	//ゲームのBGMをFadeしながら替える
	void ChangingBGM(){
		//FadeOutしない
		fadeOut = false;
		//もし何も再生してないのなら
		if (!bgmPlayer.isPlaying || !bgmPlayer.clip) {
			//ボリュームをリセット
			bgmPlayer.volume = 0;
			//再生したいBGMをbgmPlayerに伝える
			bgmPlayer.clip = nextBGM;
			//ＢＧＭを再生
			bgmPlayer.Play();
			//再生中のＢＧＭを保存
			curBGM = nextBGM;
			//もし何かを再生中なら
		} else {
			//現在のボリュームの特定
			float vol = bgmPlayer.volume;
			//まだＢＧＭが切り替わってないなら
			if(curBGM != nextBGM){
				//ボリュームを下げる
				vol -= Time.deltaTime / bgmFadeTime;
				//ボリュームが0以下なら
				if(vol < 0){
					//値の修正
					vol = 0;
					//念のため確実に再生を止める
					bgmPlayer.Stop();
					//再生したいBGMをbgmPlayerに伝える
					bgmPlayer.clip = nextBGM;
					//ＢＧＭを再生
					bgmPlayer.Play();
					//再生中のＢＧＭを保存
					curBGM = nextBGM;
				}
				//もしＢＧＭがすでに切り替わってるなら
			}else{
				//ボリュームを上げる
				vol += Time.deltaTime / bgmFadeTime;
				//ボリュームが1以上なら
				if(vol > 1){
					//値を修正
					vol = 1;
					//空にしてChangingBGM()呼ばなくする
					nextBGM = null;
				}
			}
			//ボリュームを設定
			bgmPlayer.volume = vol;
		}
	}
	//外部アクセスからPause関係で呼び出し
	public void ChangeSnapShot(string changeTo){
		if(changeTo == "Normal"){
			AudioMixerManager.normal.TransitionTo(0.01f);
		}else if(changeTo == "Pause"){
			AudioMixerManager.pause.TransitionTo(0.01f);
		}
	}
}

