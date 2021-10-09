using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {

	private Text _timeLeft;

	// Use this for initialization
	void Start () {
		_timeLeft = GameObject.FindGameObjectWithTag("time-left").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		_timeLeft.text = GameTimer.timerInstance.remainingTime();
	}
}
