using UnityEngine;
using System.Collections;

public class SecurityBehaviour : CharacterBehaviour
{
	//the politician
	public GameObject politician;

	//radius where security gaurds 
	public Vector3 guardOffset = new Vector3(1,1,0);

	private bool aggressive = false;
	private bool inFov = false;

	private Vector3 poliPositionLast;
	private Vector3 poliPositionCurrent;

	private Vector3 myPositionLast;
	private Vector3 myPositionCurrent;

	private int roughing = Animator.StringToHash("IsRough");

	Transform _view;
	PlayerController _player;

	// Use this for initialization
	void Start () {

		_view = transform.FindChild ("view");
		_anim = GetComponent<Animator>();
		
		//politician = GameObject.FindObjectOfType<PoliticianController>();
		_player = GameObject.FindObjectOfType<PlayerController>();

		poliPositionLast = politician.transform.position;
		poliPositionCurrent = politician.transform.position;

		_idling = true;
	}
	
	// Update is called once per frame
	void Update () {

		poliPositionLast = poliPositionCurrent;
		poliPositionCurrent = politician.transform.position;

		myPositionLast = myPositionCurrent;
		myPositionCurrent = this.transform.position;

		_idling = false;

		//attack journalist
		if (aggressive && inFov) {

			_anim.SetBool(roughing, true);

			Move (_player.transform.position);
			UpdateView(_player.transform.position - transform.position);

			if (Vector2.Distance(this.transform.position, _player.transform.position) < 0.2)
			{
				_player.Stun();
				aggressive = false;
			}

		} else {

			Move (getGuardPosition ());
			UpdateView (poliPositionCurrent - transform.position);

			if(myPositionLast == myPositionCurrent && poliPositionCurrent == poliPositionLast){
				_idling = true;
				UpdateView ((Vector2)(transform.position - poliPositionCurrent));
			}
		

			_anim.SetBool(_isWalkingHash, !_idling);
		}


	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Player")) {
			inFov = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag("Player")) {
			inFov = false;
			aggressive = false;
			_anim.SetBool(roughing, false);
		}
	}

	void OnEnable(){
		PlayerController.OnTakePhoto += tookPhoto;
	}

	void OnDisable(){
		PlayerController.OnTakePhoto -= tookPhoto;
	}

	void tookPhoto(){
		if (inFov) {
			aggressive = true;
		}
	}
	
	Vector3 getGuardPosition(){
		return politician.transform.position + guardOffset;
		//return politician.transform.position + (Vector3)Random.insideUnitCircle * guardRadius;
	}


	void UpdateView (Vector2 desiredFacing){

		Quaternion desiredRotation = Quaternion.FromToRotation(Vector2.up, desiredFacing);
		
		if (desiredRotation.eulerAngles.y != 0)
		{
			Vector2 fixedEuler = desiredRotation.eulerAngles;
			fixedEuler.y = 0;
			desiredRotation.eulerAngles = fixedEuler;
		}
		
		_view.rotation = Quaternion.Lerp(_view.rotation, desiredRotation, 10 * Time.deltaTime);
	}


}
