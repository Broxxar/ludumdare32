using UnityEngine;
using System.Collections;

public class SecurityBehaviour : CharacterBehaviour {

	//the politician
	public GameObject politician;
	//the photographer
	public GameObject photographer;

	//area in which taking a photo aggroes security
	private Collider2D threatRange;

	//radius where security gaurds 
	public float guardRadius = 2.0f;
	public float aggroThreshold = 4.0f;

	Transform view;

	// Use this for initialization
	void Start () {

		threatRange = transform.FindChild ("view").GetComponentInChildren<PolygonCollider2D> ();
		//politician = FindObjectOfType ();
		//photographer = FindObjectOfType ();
	}
	
	// Update is called once per frame
	void Update () {


		// photog takes picture close to security
		if (threatRange.bounds.Contains(photographer.transform.position) /*&& takingphoto */) {
			Move(photographer.transform.position);
		}

		// follow politician when moving
		else if (!politician.rigidbody2D.IsSleeping()){
			Move (getRandomNearPolitician());
		}

		// if stationary, do some scanning
//		else{

//		}

	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.name.Equals ("player")) {
			//dosomething to the player
		}
	}

	Vector3 getRandomNearPolitician(){
		return politician.transform.position + new Vector3(Random.Range(-guardRadius, guardRadius), Random.Range(-guardRadius, guardRadius), 0);
	}


}
