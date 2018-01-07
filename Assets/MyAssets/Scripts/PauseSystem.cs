using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class PauseSystem : MonoBehaviour {

	public bool pause;

	public AudioSource[] notStop;

	public void Pause(bool pause) {
		if(pause){

			Time.timeScale = 0.0f;

			AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();

			foreach (AudioSource sound in sounds) {
				sound.mute = true;
			}

			if(notStop != null){
				foreach (AudioSource sound in notStop) {
					sound.mute = false;
				}
			}

			AudioMixerManager.pause.TransitionTo(0.01f);

		}else{

			Time.timeScale = 1.0f;

			AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();

			foreach (AudioSource sound in sounds) {
				sound.mute = false;
			}

			AudioMixerManager.normal.TransitionTo(0.01f);
		}

	}
}
