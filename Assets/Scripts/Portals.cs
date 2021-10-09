using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour {

	private string _portalType;

	private bool _isNewPortal;

	private string _noPortalTag = "no-portal-zone";

	private bool _noPortalZone = false;


	// Use this for initialization
	void Start () {
		_isNewPortal = true;
		_portalType = gameObject.name;
		SpawnNewPortal(GameObject.Find(_portalType));
		// Debug.Log(_portalType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsNewPortal{
		get{
			return _isNewPortal;
		}
	}

	private void SpawnNewPortal(GameObject _otherPortal){
		// Debug.Log(_otherPortal + " is the other portal at " + _otherPortal.transform.position);

		if(_otherPortal.GetComponent<Portals>().IsNewPortal == false){
			Destroy(_otherPortal);
			_isNewPortal = false;
		}else{
			_isNewPortal = false;
		}
		
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == _noPortalTag){
			Destroy(gameObject);
		}
	}
	
}
