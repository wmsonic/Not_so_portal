using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour {

	[SerializeField]
	private GameObject _linkedDoor;

	[SerializeField]
	private GameObject _winZone;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenDoor(){
		_linkedDoor.GetComponent<Animator>().SetBool("wallOpen",true);
		_winZone.GetComponent<Collider2D>().enabled = true;
	}
}
