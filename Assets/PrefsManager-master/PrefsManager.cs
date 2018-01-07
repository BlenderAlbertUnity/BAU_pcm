// ====
// PrefsManager.cs
// v1.1.0
// Created by Kamanii
// ====

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;


/// <summary>
/// 型を自由化した拡張PlayerPrefsです。
/// </summary>
public class PrefsManager
{
	// 保存先のディレクトリ名
	private const string dirName = "userinfo";
	// 初期化が済んでいるかどうか
	private static bool isInit = false;

	// 保存先のディレクトリ
	private static string SaveDir {
		get {
			// 保存先のディレクトリパスを取得
			#if UNITY_EDITOR
			string dir = Application.streamingAssetsPath + "/";
			#else
			string dir = Application.persistentDataPath + "/";
			#endif
			return dir;
		}
	}


	/// <summary>
	/// データを保存します。
	/// </summary>
	/// <param name="data">保存するデータ</param>
	/// <param name="key">データに対応したキー</param>
	public static void SetValue<T> (string key, T data) {
		// 有効なキーかどうかチェック
		if (!KeyCheck(key)) return;

		// パスを取得
		string path = GetSavePath (key);

		// ファイルを開く
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (path, FileMode.Create);
		// シリアライズして保存
		bf.Serialize (file, data);
		// ファイルを閉じる
		file.Close ();
	}


	/// <summary>
	/// データを取得します。
	/// </summary>
	/// <param name="key">取得するデータに対応したキー</param>
	public static T GetValue<T> (string key) {
		// 有効なキーかどうかチェック
		if (!KeyCheck(key)) return default (T);

		// パスを取得
		string path = GetSavePath (key);

		// ファイルが存在するかどうか
		if (File.Exists (path)) {
			// ファイルを開く
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);

			// デシリアライズして取得
			T data = (T)bf.Deserialize (file);
			// ファイルを閉じる
			file.Close ();

			// ロードしたデータを返す
			return data;
		}
		// 規定値を返す
		return default (T);
	}

	/// <summary>
	/// キーに対応したデータを削除します。
	/// </summary>
	/// <param name="key">削除するデータに対応したキー</param>
	/// <returns><c>true</c>, 該当のデータが正しく削除された場合 <c>false</c> 該当するファイルが存在しなかった場合</returns>
	public static bool DeleteKey (string key) {
		// 有効なキーかどうかチェック
		if (!KeyCheck(key)) return false;

		// パスを取得
		string path = GetSavePath (key);

		// ファイルが存在するかどうか
		if (File.Exists (path)) {
			// ファイルを削除
			File.Delete (path);
			// 削除成功
			return true;
		}
		Debug.Log ("キーに対応するファイルが存在しません。");
		// ファイルが存在しない
		return false;
	}


	/// <summary>
	/// すべてのデータを削除します。
	/// </summary>
	/// <returns><c>true</c>,すべてのデータが正しく削除された場合 <c>false</c> 該当するファイルが存在しなかった場合</returns>
	public static bool DeleteAll () {
		// 保存先のファイルを設定
		string path = SaveDir + dirName;

		// ファイルが存在するかどうか
		if (Directory.Exists (path)) {
			// ファイルをすべて取得
			string[] filePaths = Directory.GetFiles (path+"/");

			if (filePaths.Length > 0) {

				// ディレクトリ配下のファイルをすべて削除
				foreach (string filePath in filePaths) {
					File.SetAttributes (filePath, FileAttributes.Normal);
					File.Delete (filePath);
				}

				// 削除成功
				return true;
			}
			// 削除するファイルが存在しない
			return false;
		}
		Debug.Log (dirName + "ディレクトリが存在しません。");
		// ディレクトリが存在しない
		return false;
	}


	/// <summary>
	/// キーが存在するかどうか判定します。
	/// </summary>
	/// <returns><c>true</c>, キーが存在します <c>false</c>キーが存在しません。</returns>
	/// <param name="key">調べるキーを設定します</param>
	public static bool HasKey (string key) {
		// 保存先のファイルを設定
		string dirPath = SaveDir + dirName;
		string savePath = GetSavePath (key);

		// ファイルが存在するかどうか
		if (Directory.Exists (dirPath)) {
			// ファイルをすべて取得
			string[] filePaths = Directory.GetFiles (dirPath+"/");
			if (filePaths.Length > 0) {
				// ディレクトリ配下のファイルをすべて走査
				foreach (string filePath in filePaths) {
					if (savePath == filePath) {
						// キーが存在する
						return true;
					}
				}
			}
			// キーが存在しない
			return false;
		}
		Debug.Log (dirName + "ディレクトリが存在しません。");
		// ディレクトリが存在しない
		return false;
	}


	// 有効なキーかどうかチェック
	private static bool KeyCheck (string key) {
		// キーチェック
		if (key == string.Empty) {
			Debug.LogError ("キーが設定されていません。");
			return false;
		}
		else {
			return true;
		}
	}


	// 保存先パスを返す
	private static string GetSavePath (string key) {

		// iOSのみ MONO_REFLECTION_SERIALIZER を付与
		#if UNITY_IOS && !UNITY_EDITOR
		if (!isInit) {
			Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
			isInit = true;
		}
		#endif

		// ディレクトリが存在するかどうか
		if (!Directory.Exists(SaveDir+dirName)) {
			// 存在しない場合は作成する
			Directory.CreateDirectory(SaveDir+dirName);
		}

		// キーをbyte化して暗号化
		byte[] bytes = Encoding.UTF8.GetBytes(key);
		string encryptionKey = BitConverter.ToString (bytes);

		// 保存先のファイルを設定
		string path = SaveDir + dirName +"/" + encryptionKey + ".dat";

		// パスを返す
		return path;
	}
}
