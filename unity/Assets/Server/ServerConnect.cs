using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServerConnect : MonoBehaviour {
	
    public delegate void Callback(LitJson.JsonData json);

	string railsServer = "http://localhost:3000/";
    //エネミークラス
    public class EnemyTest {
        public int id;
        public int hp;
        public int mp;
        public int skill;
        public int power;
        public int speed;
        public List<EnemyAction> Behaviors = new List<EnemyAction>();
    }
    //エネミーアクション
    public class EnemyAction {
        public int id;
        public int character;
        public int parameter;
        public int value_lower;
        public int value_upper;
        public int action;
        public int enemy_id;
    }
   
    List<EnemyTest> enemies = new List<EnemyTest>();
    void Start(){
        //こいつを呼べばenemiesの中にエネミーデータが全て格納される
        BliudEnemy();
    }

    public void BliudEnemy(){
        GetJsonToDebug("api/enemies/",(data) => {
            foreach( LitJson.JsonData json in data ){
                BliudEnemyStatus(json);
            }
            //デバッグログ
            foreach(EnemyTest enemy in enemies){           
                Debug.Log("id:" + enemy.id);
                Debug.Log("hp:" + enemy.hp);
                Debug.Log("mp:" + enemy.mp);
                Debug.Log("skill:" + enemy.skill);
                Debug.Log("speed:" + enemy.speed);
                Debug.Log("power:" + enemy.power);
            }
        });
    }

    private void GetEnemyAction(int enemy_id ){
        string url = "api/enemies/actions?enemy_id=" + enemy_id;
        GetJsonToDebug(url, (data) => {
            foreach (LitJson.JsonData json in data) {
                BliudEnemyAction(json);
            }
            //デバッグログ
            foreach(EnemyAction action in enemies[enemy_id - 1].Behaviors){           
                Debug.Log("id:" + action.id);
                Debug.Log("enemy_id:" + action.enemy_id);
                Debug.Log("character:" + action.character);
                Debug.Log("parameter:" + action.parameter);
                Debug.Log("value_lower:" + action.value_lower);
                Debug.Log("value_upper:" + action.value_upper);
                Debug.Log("action:" + action.action);
            }
        });
    }

    private void BliudEnemyAction(LitJson.JsonData json) {
        EnemyAction action = new EnemyAction();
        action.id = (int)json["id"];
        action.enemy_id = (int)json["enemy_id"];
        action.character = (int)json["character"];
        action.parameter = (int)json["parameter"];
        action.value_lower = (int)json["value_lower"];
        action.value_upper = (int)json["value_upper"];
        action.action = (int)json["action"];
        enemies[action.enemy_id - 1].Behaviors.Add(action);
    }

    private void BliudEnemyStatus(LitJson.JsonData json){
        EnemyTest enemy = new EnemyTest();
        enemy.hp = (int)json["hp"];
        enemy.id = (int)json["id"];
        enemy.mp = (int)json["mp"];
        enemy.skill = (int)json["skill"];
        enemy.power = (int)json["power"];
        enemy.speed = (int)json["speed"];
        GetEnemyAction(enemy.id);
        enemies.Add(enemy);
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
