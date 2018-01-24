using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServerConnect : MonoBehaviour {
	
	string railsServer = "http://localhost:3000/";

	public void GetJsonToDebug( string url, string enemyNum = null, UnityAction callback = null ) { 
		var debugPrintStr = "url: " + url + "\n" + "type: GET";
		Debug.unityLogger.Log(debugPrintStr);
		StartCoroutine(GetJSON(url, callback ));
	}

	public IEnumerator GetJSON(string url, UnityAction callback){
		var urlStr = railsServer + url;
		var www = new WWW(urlStr);
		yield return www;
		if(!string.IsNullOrEmpty(www.error)){
			Debug.unityLogger.Log ("エラー発生");
		} else {
			//var date = LitJson.JsonMapper.Toobject(www.text);
			//callback(date);
		}

	}
	
}
