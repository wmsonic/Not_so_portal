using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour {

	public static GameTimer timerInstance;

	[SerializeField]
	private float _timerInMinutes; 

	private float _timerLenght;

	public bool _timerOn;

	void Awake(){
		if(timerInstance != null) {
			Destroy(gameObject);
		} else {
			timerInstance = this;
			// Conserve le GameObject sur toutes les scènes
			DontDestroyOnLoad(gameObject);
		}
	}


	// Use this for initialization
	void Start () {
		_timerLenght = _timerInMinutes * 60;
	}
	
	// Update is called once per frame
	void Update () {
		if(_timerOn){
			_timerLenght -= Time.deltaTime;
		}else if(!_timerOn){
			_timerLenght = _timerInMinutes * 60;
		}
		if(_timerLenght <=0){
			GameManager.gameManagerInstance._outOfTime = true;
		}
	}

	public string remainingTime(){

		float _timerLeft = _timerLenght;
		
		float _minutes = Mathf.Floor(_timerLeft / 60);
		string _seconds = (_timerLeft % 60).ToString("00.00");


		return _minutes+":"+_seconds;
	}
	
}
