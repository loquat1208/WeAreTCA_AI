using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AI.Unit.Enemy;
using AI.Behavior;

public class ServerConnect : MonoBehaviour {
	
    public delegate void Callback(LitJson.JsonData json);

	string railsServer = "http://localhost:3000/";
   
    private void GetEnemyAction(int enemy_id, UnityAction<List<AIModel>> callback){
        string url = "api/enemies/actions?enemy_id=" + enemy_id;
        List<AIModel> ai = new List<AIModel>();
        GetJsonToDebug(url, (data) => {
            foreach (LitJson.JsonData json in data) {
                ai.Add(BliudEnemyAction(json));
            }
            callback(ai);
        });
    }

    private AIModel BliudEnemyAction(LitJson.JsonData json) {
        int subject = (int)json["character"];
        int criterion = (int)json["parameter"];
        int from = (int)json["value_lower"];
        int to = (int)json["value_upper"];
        int behavior = (int)json["action"];
        AIModel action = new AIModel((AIModel.Subject) subject, (AIModel.Criterion) criterion, (float)from, (float) to, (AIModel.Behavior) behavior);
        return action;
    }


    public void BuildEnemy(int id, UnityAction<EnemyModel> callback) {
        string str = "api/enemies/enemy?id=" + id;
        GetJsonToDebug(str, (data) => {
            int Hp = (int)data[0]["hp"];
            int Id = (int)data[0]["id"];
            int Mp = (int)data[0]["mp"];
            int Skill = (int)data[0]["skill"];
            int Power = (int)data[0]["power"];
            int Speed = (int)data[0]["speed"];
            GetEnemyAction(Id,(action) => {
                List<AIModel>Behaviors = action;
                EnemyModel enemy = new EnemyModel(Id, (float) Hp, (float)Mp, Power, Speed, (Skill.Type)Skill, Behaviors);
                callback(enemy);
            });
        });

    }
    public void GetJsonToDebug( string url, UnityAction<LitJson.JsonData> callback = null) { 
		var debugPrintStr = "url: " + url + "\n" + "type: GET";
		Debug.unityLogger.Log(debugPrintStr);
        StartCoroutine(GetJSON(url, callback));
	}

    IEnumerator GetJSON (string url, UnityAction<LitJson.JsonData> callback) {
        string urlStr = railsServer + url;
        using(WWW www = new WWW(urlStr)){
            yield return www;
            if(!string.IsNullOrEmpty(www.error)){
                Debug.LogError("www Error:" + www.error);
                yield break;
            }
            Debug.Log(www.text);
            var data = LitJson.JsonMapper.ToObject<LitJson.JsonData>(www.text);
            callback(data);
        }
    }
}
