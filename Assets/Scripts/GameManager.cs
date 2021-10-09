using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager gameManagerInstance;

	public bool _playerWin = false;
	public bool _playerIsDead = false;
	public bool _outOfTime = false;

	void Awake(){
		// Singleton
		if(gameManagerInstance != null) {
			Destroy(gameObject);
		} else {
			gameManagerInstance = this;

			// Conserve le GameObject sur toutes les scènes
			DontDestroyOnLoad(gameObject);
		}
	}
	
	public void ShowScene(ScenesName nomScene) {
		SceneManager.LoadScene(nomScene.ToString());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_playerIsDead || _outOfTime || _playerWin){
			ShowScene(ScenesName.score);
		}
	}
}

public enum ScenesName {
	menu,
	main,
	score,
}