using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour {

	private WayPoint previousWP;
	public WayPoint currentWP;
	public WayPoint destinationWP;

	public float moveSpeed;

	public float wpRadius = 1.8f;

	private Vector3 targetPosition;

	//move vector for current move
	private Vector3 currentMoveTarget;

	// Use this for initialization
	void Start () {


		if (currentWP == null) {

			//assign current waypoint by closest one on map
			WayPoint[] allWayPoints = GameObject.FindObjectsOfType<WayPoint> ();

			float minDistance = 10000.0f; 
			float distance;

			for (int i = 0; i < allWayPoints.Length; i ++){

				distance = Vector3.Distance(this.transform.position, allWayPoints[i].transform.position);

				if (distance < minDistance){
					minDistance = distance;
					currentWP = allWayPoints[i];
				}
			}
		}


/*
		if (destinationWP == null) {
			//assign destination waypoint randomly from current neighbours
			if (currentWP != null){
				destinationWP = currentWP.neighbours[Random.Range (0, currentWP.neighbours.Count)];
			}
		}
*/
		previousWP = currentWP;
		//should call getMoveVector first
		targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		//if arrived at target location, find a random neighbour and move there
		if (transform.position.Equals(targetPosition)) {

			WayPoint newDestinationWP;

			do {		
				newDestinationWP = currentWP.neighbours[Random.Range (0, currentWP.neighbours.Count)];
			} while (newDestinationWP == previousWP);

			Debug.Log (newDestinationWP.transform.position);

			previousWP = currentWP;
			
			currentMoveTarget = getMoveTarget(newDestinationWP);
	
			currentWP = newDestinationWP;

			Debug.Log (previousWP.transform.position);
			Debug.Log (newDestinationWP.transform.position);
			Debug.Log ("------------------------");
		}

		//everyframe move towards target position
		Move (currentMoveTarget);
	}

	protected Vector3 getMoveTarget(WayPoint destinationWP){
		//Debug.Log ("In GetMoveTarget");
		

		Vector3 currentPosition = this.transform.position;
		targetPosition = destinationWP.transform.position + new Vector3(Random.Range(-wpRadius, wpRadius), Random.Range(-wpRadius, wpRadius), 0);

		//Debug.Log ("In GetMoveVector end");
		return targetPosition;
	}

	protected void Move(Vector3 moveTarget){

		//Debug.Log ("In move");
		transform.position = Vector3.MoveTowards (transform.position, moveTarget, Time.deltaTime * moveSpeed);
	}


}
