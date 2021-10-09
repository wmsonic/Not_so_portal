using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

	[SerializeField]
	private GameObject linkedLaser;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			linkedLaser.GetComponent<Animator>().SetBool("laserIsOn", true);
		}
	}
	
	private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			linkedLaser.GetComponent<Animator>().SetBool("laserIsOn", false);
		}
	}
}
