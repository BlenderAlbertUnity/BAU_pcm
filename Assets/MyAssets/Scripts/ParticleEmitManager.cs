using UnityEngine;
using System.Collections;

public class ParticleEmitManager : MonoBehaviour {

	public ParticleSystem[] effects;

	public bool[] swicth;
	bool[] played;

	void Start () {
		played = new bool[swicth.Length];
	}
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < effects.Length; i++) {
			if(swicth[i]){
				if(!played[i]){
					effects[i].Play();
					ParticleSystem[] childs = effects[i].GetComponentsInChildren<ParticleSystem>();
					foreach (ParticleSystem child in childs) {
						child.Play();
					}
					played[i] = true;
				}	
			}else{
				if(played[i]){
					effects[i].Stop();
					ParticleSystem[] childs = effects[i].GetComponentsInChildren<ParticleSystem>();
					foreach (ParticleSystem child in childs) {
						child.Stop();
					}
				}
				played[i] = false;
			}
		}

	}
}
