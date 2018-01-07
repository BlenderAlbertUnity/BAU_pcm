using UnityEngine;
using System.Collections;

public class CharacterVoice : MonoBehaviour {

	public AudioClip[] allVoices;

	public AudioSource mouth;

	// Use this for initialization
	void Start () {
		if(!mouth)
			this.enabled = false;
	}
	
	public void SayThis (int thisLine) {
		mouth.Stop();

		mouth.PlayOneShot(allVoices[thisLine]);
	}

	public void SaySameThing (string allLines) {
		
		string[] myList = allLines.Split(',');

		int selected = int.Parse(myList[Random.Range(0, myList.Length)]);

		if(selected <= allVoices.Length - 1){
			mouth.Stop();
			mouth.PlayOneShot(allVoices[selected]);
		}
	}
}
