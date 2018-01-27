using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Unit.Enemy;

public class Enemies : MonoBehaviour {

	[SerializeField]List<EnemyController> enemies = new List<EnemyController>();
	[SerializeField] private ServerConnect server;
	void Start () {
		
	}
	
	public void LoadData() {
		server.BuildEnemies((data) => {
			for(int i = 0; i < 5; i++) {
				enemies[i].Model = data[i];
				Debug.Log("ID" + enemies[i].Model.Id);
				Debug.Log("HP" + enemies[i].Model.Hp);
				Debug.Log("Speed" + enemies[i].Model.Speed);
				Debug.Log("Power" + enemies[i].Model.Power);
				Debug.Log("Skill" + enemies[i].Model.Skill);
				foreach(var action in enemies[i].Model.Behaviors){
					Debug.Log("Subject" + action.GetSubject);
					Debug.Log("criterion" + action.GetCriterion);
					Debug.Log("from" + action.GetFrom);
					Debug.Log("to" + action.GetTo);
					Debug.Log("behavior" + action.GetBehavior);
				}
			}
		});		
	}
	
}
