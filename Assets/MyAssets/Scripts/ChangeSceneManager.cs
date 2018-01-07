using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//画面の表示切り替えに使う
public class ChangeSceneManager : MonoBehaviour {
	//現在のシーンの名前
	public string sceneName;

	public void LoadNewScene(){
		//次のLEVELのロード
		SceneManager.LoadScene(sceneName);
	}

	public void FadeScene(float fadeTime)
	{
		//DelayMethodを3.5秒後に呼び出す
		Invoke("LoadNewScene", fadeTime);
	}
}
