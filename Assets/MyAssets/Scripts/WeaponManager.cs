using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {
	[Tooltip("武器を見せるかどうかを指定")]
	public bool show;
	//使用して武器（外部呼出し）
	public int weaponSelected;
	//使用中の武器
	int weaponUsing;
	//武器交換の確認
	bool changed = false;
	//各エフェクトの再生判定
	bool showPlayed;
	bool hidePlayed;
	//エフェクトサウンド設定
	[Tooltip("エフェクトサウンドを再生するAudioSource")]
	public AudioSource speaker;
	[Tooltip("武装開始エフェクトサウンド")]
	public AudioClip showEffectSound;
	[Tooltip("武装解除のエフェクトサウンド")]
	public AudioClip hideEffectSound;
	//各武器のリスト
	[System.Serializable]
	public class Weapon{
		[Tooltip("On/Offを切り替えたい武器")]
		public GameObject mesh;
		[Tooltip("武装開始エフェクト")]
		public ParticleSystem showEffect;
		[Tooltip("武装開始時にパーティクルがこれ以下なら武器をON! 武装解除時にパーティクルがこれ以上なら武器をOFF! ")]
		public int toChange = 200;
		[Tooltip("武装解除のエフェクト")]
		public ParticleSystem hideEffect;
		//各武器のリスト
		[System.Serializable]
		public class SubWeapon{
			[Tooltip("On/Offを切り替えたい武器")]
			public GameObject mesh;
			[Tooltip("武装開始エフェクト")]
			public ParticleSystem showEffect;
			[Tooltip("武装解除のエフェクト")]
			public ParticleSystem hideEffect;
		}
		[Tooltip("サブ武器")]
		public SubWeapon subWeapon;
	}
	[Tooltip("各武器のリスト")]
	public Weapon[] weapons;

	// Use this for initialization
	void Start () {
		//各武器をOFFにする
		for (int i = 0; i < weapons.Length; i++) {
			weapons[i].mesh.SetActive(false);

			if(weapons[i].subWeapon.mesh){
				weapons[i].subWeapon.mesh.SetActive(false);
			}
		}
		//weaponUsingを初期化
		weaponUsing = weaponSelected;
	}
	
	// Update is called once per frame
	void Update () {

		if(!changed){
			//Animator　に武器交換アニメーションを指示
		}
		//武器を出すなら...
		if(show){
			//リセット
			hidePlayed = false;
			//選択した武器がまだ出てないなら
			if(!weapons[weaponSelected].mesh.activeSelf){
				//武装開始エフェクトを再生済みなら
				if(showPlayed){
					//武装開始パーティクル数が[toChange]以上なら...
					if(weapons[weaponSelected].showEffect.particleCount > weapons[weaponSelected].toChange){
						//武器の表示
						weapons[weaponSelected].mesh.SetActive(true);
						if(weapons[weaponSelected].subWeapon.mesh){
							weapons[weaponSelected].subWeapon.mesh.SetActive(true);
						}
						//リセット
						weaponUsing = weaponSelected;
						showPlayed = false;
						//交換済みを報告
						changed = true;
					}
				//武装解除エフェクトを未再生なら
				}else{
					//エフェクトの再生
					weapons[weaponSelected].showEffect.Play();
					if(weapons[weaponSelected].subWeapon.mesh){
						weapons[weaponSelected].subWeapon.showEffect.Play();
					}
					//エフェクト音の再生
					speaker.PlayOneShot(showEffectSound);
					//表示済み
					showPlayed = true;
				}
			}
		//武器をしまうなら...
		}else{
			//リセット
			showPlayed = false;
			//選択した武器をまだ隠してないなら
			if(weapons[weaponUsing].mesh.activeSelf){
				//武装開始エフェクトを再生済みなら
				if(hidePlayed){
					//武装解除パーティクル数が[toChange]以下なら...
					if(weapons[weaponUsing].showEffect.particleCount < weapons[weaponSelected].toChange){
						//全武器の非表示
						for (int i = 0; i < weapons.Length; i++) {
							weapons[i].mesh.SetActive(false);

							if(weapons[i].subWeapon.mesh){
								weapons[i].subWeapon.mesh.SetActive(false);
							}
						}
						//リセット
						hidePlayed = false;
					}
				//武装解除エフェクトを未再生なら
				}else{
					//エフェクトの再生
					weapons[weaponSelected].hideEffect.Play();
					if(weapons[weaponSelected].subWeapon.mesh){
						weapons[weaponSelected].subWeapon.hideEffect.Play();
					}
					//エフェクト音の再生
					speaker.PlayOneShot(hideEffectSound);
					//表示済み
					hidePlayed = true;
				}
			}
		}
	}
	//外部からの呼び出しで武器を交換
	public void ChangeWeapon (int newWeapon) {
		//使用したい武器の指定
		weaponSelected = newWeapon;
		//リセット
		changed = false;
		//Animator　に武器交換アニメーションを指示
	}

	//投げた時用の呼び出し
	public void WeaponThrow(string state){
		switch (state) {
		case "Show":
			//武器の表示
			weapons[weaponSelected].mesh.SetActive(true);
			if(weapons[weaponSelected].subWeapon.mesh){
				weapons[weaponSelected].subWeapon.mesh.SetActive(true);
			}
			Debug.Log(weapons[weaponSelected].mesh.activeSelf);
			hidePlayed = true;
			break;

		case "ShowWithEffect":
			//武器の表示のリセット
			showPlayed = false;
			Debug.Log(weapons[weaponSelected].mesh.activeSelf);
			break;

		case "Hide":
			//武器の非表示
			weapons[weaponSelected].mesh.SetActive(false);
			if(weapons[weaponSelected].subWeapon.mesh){
				weapons[weaponSelected].subWeapon.mesh.SetActive(false);
			}
			showPlayed = true;
			break;

		case "HideWithEffect":
			//武器の非表示のリセット
			hidePlayed = false;
			break;
		}
	}
}
