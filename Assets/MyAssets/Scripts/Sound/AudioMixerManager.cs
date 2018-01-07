using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour {

	public AudioMixer mixer;

	public AudioMixerSnapshot normalShot;
	public static AudioMixerSnapshot normal;

	public AudioMixerSnapshot pauseShot;
	public static AudioMixerSnapshot pause;

	public Slider[] sliders;

	void Awake(){
		normal = normalShot;
		pause = pauseShot;
	}

	void Start(){
		if(sliders.Length > 0){
			foreach (Slider sldr in sliders) {
				string myName = sldr.gameObject.name;
				if(PrefsManager.HasKey(myName)){
					sldr.value = PrefsManager.GetValue<float>(myName);
					mixer.SetFloat(myName, sldr.value);
				}
			}
		}
	}

	public void SetMasterVolume(float newValue){
		mixer.SetFloat("MasterVolume", newValue);
	}

	public void SetSystemVolume(float newValue){
		mixer.SetFloat("SystemVolume", newValue);
	}

	public void SetEffectsVolume(float newValue){
		mixer.SetFloat("EffectsVolume", newValue);
	}

	public void SetBGMVolume(float newValue){
		mixer.SetFloat("BGM_Volume", newValue);
	}

	public void SaveMixerValues(){
		float got;
		bool result = mixer.GetFloat("MasterVolume", out got);
		PrefsManager.SetValue<float>("MasterVolume", got);
		result = mixer.GetFloat("SystemVolume", out got);
		PrefsManager.SetValue<float>("SystemVolume", got);
		result = mixer.GetFloat("EffectsVolume", out got);
		PrefsManager.SetValue<float>("EffectsVolume", got);
		result = mixer.GetFloat("BGM_Volume", out got);
		PrefsManager.SetValue<float>("BGM_Volume", got);


	}
}