# PrefsManager
自由な型で保存できて、さらに保存データが暗号化されるように拡張したPlayerPrefsです。

 
#### 自プロジェクトでの使用
**Assets/PrefsManager.cs**が本体です。
<br>
自プロジェクトで使用する場合は**PrefsManager.cs**のみを取り出してお使いください。
  
<br>
# 使い方
保存されるデータはpersistentDataPathディレクトリ(エディタではStreamingAssets)配下に生成されるuserinfoの中にファイル単位で保存されます。

<br>データはバイナリデータとして保存されるので安易に解読されることはないかと思います。


    using UnityEngine;

    using System.Collections;

    
    public class TestClass : MonoBehaviour {
 
       
        // Use this for initialization

        void Start () {

            // ロード

            bool isTapButton = PrefsManager.GetValue<bool> ("KEY_ON_TAP_BUTTON");

        }

        
        // ボタンが押された

        public void OnTapButton () {

            // セーブ

            PrefsManager.SetValue<bool> ("KEY_ON_TAP_BUTTON", true);

        }

    }


<br>
#### クラスごと保存する
クラスをSerializableでシリアライズ化すればクラスを丸ごと保存できます。


    using UnityEngine;

    using System.Collections;

    
    public class TestClass : MonoBehaviour {

    
        // Use this for initialization

        void Start () {

            // ロード

            PlayerStatus status = PrefsManager.GetValue<PlayerStatus> ("KEY_PLAYER_STATUS");

            Debug.Log("名前 : "+status.name);

            Debug.Log("レベル : "+status.level);

        }


        // ゲームをセーブする

        public void SaveGame (PlayerStatus status) {

            // セーブ

            PrefsManager.SetValue<PlayerStatus> ("KEY_PLAYER_STATUS", status);

        }


        // ステータスクラス

        [System.Serializable]

        public class PlayerStatus {

            public string name = "勇者";

            public int level = 1;

        }

    }



<br>
## リリースノート
####- 2016/8/4
* **メソッド名の変更**<br>
SaveをSetValueにLoadをGetValueに変更しました。


* **namespace削除**<br>
namespace PrefsManageを削除しました。
<br>
クラス名として使用。


* **Keysクラス削除**

<br>
## ビルド環境
Unity 5.4.0f3<br>
MacOSX El Capitan 10.11.5
