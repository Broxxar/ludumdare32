using UnityEngine;
using System.Collections;

public class SecurityBehaviour : CharacterBehaviour {

	//the politician
	public GameObject politician;
	//the photographer
	public GameObject photographer;

	//radius where security gaurds 
	public Vector3 guardOffset = new Vector3(1,1,0);

	public bool aggressive = false;

	private Vector3 poliPositionLast;
	private Vector3 poliPositionCurrent;

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

		if (aggressive) {
			Move (photographer.transform.position);
			UpdateView(photographer.transform.position - transform.position);

			_anim.SetBool(roughing, !_isWalkingHash, !_idling);
		} else {

			Move (getGuardPosition ());
			_anim.SetBool(_isWalkingHash, !_idling);

			// follow politician when moving
			if (poliPositionCurrent != poliPositionLast) {

				// look at direction of movement
				UpdateView (poliPositionCurrent - transform.position);
			}

			// if stationary, do some scanning
			else {
				UpdateView (transform.position - poliPositionCurrent);
				_idling = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Player")) {
			Debug.Log ("On trigger enter journalist");
			aggressive = true;
			//dosomething to the player
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag("Player")) {
			Debug.Log ("On trigger exit journalist");
			aggressive = false;
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
