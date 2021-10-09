using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {


	private void Start(){
		
	}

	private void Update(){
		
	}

	// Change la scène pour la scène main (le jeu)
	public void Play() {
		GameManager.gameManagerInstance.ShowScene(ScenesName.main);
	}
}
