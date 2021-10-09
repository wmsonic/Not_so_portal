using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	
	[SerializeField]
	private float _speedX = 5f;

	[SerializeField]
	private GameObject _bluePortalPrefab;

	[SerializeField]
	private GameObject _purplePortalPrefab;

	[SerializeField]
	private LayerMask _wallLayerMask;

	[SerializeField]
	private LayerMask _propsLayerMask;

	[SerializeField]
	private AudioClip _boutonSound;

	[SerializeField]
	private AudioClip _portalSound;

	private Rigidbody2D _rigidBody;

	private Animator _animator;

	private string _avanceInputName="Vertical";

	private Vector2 _curseurPos;

	private string _portalIn = "";

	private string _portalOut = "";

	private BoxCollider2D _collider;

	private float distanceCurseur;

	private Vector2 _direction;

	private string _buttonTag = "button";

	private string _keyTag = "keyCard";

	private bool _playerHasKey = false;

	private string _keyHoleTag = "KeyHole";

	private string _winZoneTag = "win-zone";

	private string _playerMoving = "_playerMoving";

	// Use this for initialization
	void Start () {
		_collider = GetComponent<BoxCollider2D>();
		_rigidBody = GetComponent<Rigidbody2D>();
		GameTimer.timerInstance._timerOn = true;
		_animator= GameObject.FindGameObjectWithTag("playerAntenna").GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		//Détection de la position du cruseur
		_curseurPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Position du Player
		Vector2 playerPos = transform.position;
		//calculez l'angle en radiant entre Player et le curseur
		float distanceY = _curseurPos.y - playerPos.y ;
		float distanceX = _curseurPos.x - playerPos.x ;
		float angleRadiant = Mathf.Atan2(distanceY, distanceX);
		//convertir l'angle en radiant en degrés
		float angleDegres = Mathf.Rad2Deg * angleRadiant;
		//orienter Player avec cette angle
		_rigidBody.rotation = angleDegres;

		Vector2 avancer = new Vector2(distanceX, distanceY).normalized;

		_direction = new Vector2(distanceX, distanceY);

		//se déplacer vers le curseur si on enfonce une touche
		
		
		distanceCurseur = Vector2.Distance(transform.position, _curseurPos);

		if(Input.GetButton(_avanceInputName)){
			if(Input.GetAxis(_avanceInputName) >= 0 && distanceCurseur > _collider.size.x/4){
				_rigidBody.velocity = avancer * _speedX;
				_animator.SetBool(_playerMoving, true);
			}else if(Input.GetAxis(_avanceInputName) <= 0){
				_rigidBody.velocity = -(avancer * _speedX);
			}else{
				_rigidBody.velocity = Vector2.zero;
			}
		}else{
				_rigidBody.velocity = Vector2.zero;
				_animator.SetBool(_playerMoving, false);
		}

	}

	// Update is called once per frame
	void Update () {
		
		//left click
		if(Input.GetButtonDown("Fire1")){
			RaycastHit2D wallTest = Physics2D.Raycast(transform.position,_direction,distanceCurseur,_wallLayerMask);
			RaycastHit2D propsTest = Physics2D.Raycast(transform.position,_direction,distanceCurseur,_propsLayerMask);
			Debug.DrawRay(transform.position,_direction,Color.cyan,3);
			if(wallTest == false && propsTest == false){
				//spawn un portail bleu
				Instantiate(_bluePortalPrefab,_curseurPos,Quaternion.identity);
				AudioSource.PlayClipAtPoint(_portalSound,transform.position);
				// Debug.Log("no walls to report here");
			}
			
			
		}
		//right click
		if(Input.GetButtonDown("Fire2")){
			RaycastHit2D wallTest = Physics2D.Raycast(transform.position,_direction,distanceCurseur,_wallLayerMask);
			RaycastHit2D propsTest = Physics2D.Raycast(transform.position,_direction,distanceCurseur,_propsLayerMask);
			Debug.DrawRay(transform.position,_direction,Color.magenta,3);
			if(wallTest == false && propsTest == false){
				//spawn un portail mauve
				Instantiate(_purplePortalPrefab,_curseurPos,Quaternion.identity);
				AudioSource.PlayClipAtPoint(_portalSound,transform.position);
				// Debug.Log("no walls to report here");
			}
		}
		//left shift
		if(Input.GetButtonUp("Fire3")){
			//se teleporter d'un portail a l'autre
			if(_portalIn!=""){
				GoThroughPortal();
			}
		}

	}

	void GoThroughPortal(){
		//change la position du joueur a la position du portail de sortie
		transform.position = GameObject.FindGameObjectWithTag(_portalOut).transform.position;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag.StartsWith("portal")){
			_portalIn = other.gameObject.tag;
			if(_portalIn.Contains("bleu")){
				_portalOut = "portal_mauve";
			}else if(_portalIn.Contains("mauve")){
				_portalOut = "portal_bleu";
			}
			// Debug.Log(_portalIn + " is IN " + _portalOut + " is OUT");
		}

		if(other.gameObject.tag == _keyTag){
			_playerHasKey = true;
			Destroy(other.gameObject);
		}

		if(other.gameObject.tag == _winZoneTag){
			GameManager.gameManagerInstance._playerWin = true;
		}

	}

	private void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == _buttonTag){
			if(Input.GetButtonUp("Jump")){
				other.gameObject.GetComponent<Button>().OpenWall();
				AudioSource.PlayClipAtPoint(_boutonSound,transform.position);
			}
		}
		if(other.gameObject.tag == _keyHoleTag){
			if(Input.GetButtonUp("Jump")){
				if(_playerHasKey){
					//open wall
					other.gameObject.GetComponent<KeyHole>().OpenDoor();
					_playerHasKey = false;
				}else{
					//play no keycard sound
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag.StartsWith("portal")){
			//remet les tags vide pour eviter que l'utilisateur puisse se teleporter en dehors des portails
			_portalIn = "";
			_portalOut = "";
		}
	}

	public bool PlayerDead(){
		bool _playerDead = true;
		return _playerDead;
	}
	
	
}
