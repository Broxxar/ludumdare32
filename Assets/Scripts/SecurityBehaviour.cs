using UnityEngine;
using System.Collections;

public class SecurityBehaviour : CharacterBehaviour {

	//the politician
	public GameObject politician;
	//the photographer
	public GameObject photographer;

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

	// Use this for initialization
	void Start () {

		_view = transform.FindChild ("view");
		_anim = GetComponent<Animator>();

		//politician = GameObject.FindObjectOfType<PoliticianController>();
		//photographer = GameObject.FindObjectOfType<PlayerController> ();

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
			Move (photographer.transform.position);
			UpdateView(photographer.transform.position - transform.position);

		} else {

			Move (getGuardPosition ());


			// follow politician when moving
			if (poliPositionCurrent != poliPositionLast) {

				// look at direction of movement
				UpdateView (poliPositionCurrent - transform.position);
			}

			// if stationary, do some scanning
			else {
				UpdateView (transform.position - poliPositionCurrent);
				if(myPositionLast == myPositionCurrent)
					_idling = true;
			}

			_anim.SetBool(_isWalkingHash, !_idling);
		}

		_anim.SetBool(roughing, aggressive && inFov);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Player")) {
			inFov = true;
			//dosomething to the player
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag("Player")) {
			inFov = false;
			aggressive = false;
		}
	}

	void OnEnable(){
		PlayerController.OnTakePhoto += tookPhoto;
	}

	void OnDisable(){
		PlayerController.OnTakePhoto -= tookPhoto;
	}

	void tookPhoto(){
		aggressive = true;
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
