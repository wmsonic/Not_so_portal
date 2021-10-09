using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	[SerializeField]
	private GameObject _linkedWall;

	[SerializeField]
	private float _timerLenght = 5f;

	[SerializeField]
	private AudioClip _doorSound;

	private float _timeRemaining;

	private bool wallIsOpened = false;

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_timeRemaining = _timerLenght;
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(wallIsOpened && _timeRemaining > 0){
			_animator.SetBool("button_active",true);
			_timeRemaining -= Time.deltaTime;
			if(_timeRemaining <= 0){
				CloseWall();
			}
		}else{
			_timeRemaining = _timerLenght;
			_animator.SetBool("button_active",false);
		}
		
	}

	public void OpenWall(){
		// Debug.Log("Wall opened");
		_linkedWall.GetComponent<Animator>().SetBool("wallOpen",true);
		AudioSource.PlayClipAtPoint(_doorSound,transform.position);
		wallIsOpened = true;
	}
	
	public void CloseWall(){
		// Debug.Log("Wall closed");
		_linkedWall.GetComponent<Animator>().SetBool("wallOpen",false);
		wallIsOpened = false;
	}
}
