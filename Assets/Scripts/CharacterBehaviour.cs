using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class CharacterBehaviour : MonoBehaviour {

	private WayPoint previousWP;
	public WayPoint currentWP;
	public WayPoint destinationWP;

	public float moveSpeed;

	public float wpRadius = 1.8f;

	private Vector3 targetPosition;

	//move vector for current move
	private Vector3 currentMoveVector;

	// Use this for initialization
	void Start () {

		if (currentWP == null) {
			//assign current waypoint by closest one on map
		}

		if (destinationWP == null) {
			//assign destination waypoint
		}

		previousWP = currentWP;
		//should call getMoveVector first
		targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		//if arrived at target location, find a random neighbour and move there
		//Debug.Log (transform.position);
		//Debug.Log (targetPosition);
		if (transform.position.Equals(targetPosition)) {




			WayPoint newDestinationWP;

			do {		
			 newDestinationWP = currentWP.neighbours[Random.Range (0, currentWP.neighbours.Count)];
			} while (newDestinationWP == previousWP);

			previousWP = currentWP;
			
			currentMoveVector = getMoveTarget(newDestinationWP);
	
			currentWP = newDestinationWP;

			Debug.Log (previousWP.transform.position);
			Debug.Log (newDestinationWP.transform.position);
			Debug.Log ("------------------------");
		}

		//everyframe move towards target position
		Move (currentMoveVector);
	}

	protected Vector3 getMoveTarget(WayPoint destinationWP){
		//Debug.Log ("In GetMoveVector");
		

		Vector3 currentPosition = this.transform.position;
		targetPosition = destinationWP.transform.position + new Vector3(Random.Range(-wpRadius, wpRadius), Random.Range(-wpRadius, wpRadius), 0);

		//Debug.Log ("In GetMoveVector end");
		return targetPosition;
	}

	protected void Move(Vector3 moveTarget){
		//Debug.Log (moveVector);
		//Debug.Log (transform.position);
		//Debug.Log (moveTarget);

		transform.position = Vector3.MoveTowards (transform.position, moveTarget, Time.deltaTime * moveSpeed);
	}


}
