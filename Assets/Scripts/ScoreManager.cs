using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	private GameManager _gameManager;

	private Text _gameRecap;

	private Text _timeLeft;

	private string _remainingTime;

	private string _timeOut = "You ran out of time";

	private string _playerDied = "You died";

	private string _playerWon = "You escaped the room \n You won!";

	// Use this for initialization
	void Start () {
		_gameManager = GameManager.gameManagerInstance;
		_gameRecap = GameObject.FindGameObjectWithTag("game-recap").GetComponent<Text>();
		_timeLeft = GameObject.FindGameObjectWithTag("time-left").GetComponent<Text>();
		_remainingTime = GameTimer.timerInstance.remainingTime();
		if(_gameManager._outOfTime){
			_gameRecap.text = _timeOut;
			_timeLeft.text = "00:00.00";
		}else if(_gameManager._playerIsDead){
			_gameRecap.text = _playerDied;
			_timeLeft.text = _remainingTime;
		}else if(_gameManager._playerWin){
			_gameRecap.text = _playerWon;
			_timeLeft.text = _remainingTime;
		}
		GameTimer.timerInstance._timerOn = false;
		_gameManager._playerIsDead = false;
		_gameManager._outOfTime = false;
		_gameManager._playerWin = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackToMenu() {
		GameManager.gameManagerInstance.ShowScene(ScenesName.menu);
	}


}
