using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour {

	public float radius = 1.0f;
	public List<WayPoint> neighbours = new List<WayPoint>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public LinkedList<WayPoint> findPathAStar (WayPoint target, Stack<WayPoint> pathSoFar, LinkedList<WayPoint> visited){
		return null;
	}


	public LinkedList<WayPoint> findPath (WayPoint target, LinkedList<WayPoint> pathSoFar, Stack<WayPoint> visited){

		//this node has been visited
		if (visited.Contains (this)) {
			return null;
		}

		visited.Push(this);
		pathSoFar.AddLast(this);

		//arrived at target
		if (this.Equals(target)) {
			visited.Pop();
			return pathSoFar;
		}

		LinkedList<WayPoint> candidatePath = new LinkedList<WayPoint>();
	
		//check neighbours
		for (int i = 0; i < neighbours.Count; i ++) {

			//check if there is a path coming from the neighbour
			LinkedList<WayPoint> tempPath = neighbours[i].findPath(target, pathSoFar, visited);


			// have no candidate yet
			if (candidatePath.Count == 0){
				tempPath = candidatePath;
			}

			//if the temp path is valid and shorter than the best so far, replace best so far
			if (tempPath != null && tempPath.Count < candidatePath.Count){
				candidatePath = tempPath;
			} 

		}

		//found no candidates, deadend, return null (no paths)
		if (candidatePath.Count == 0) {
			Debug.Log("Dead End");
			visited.Pop ();
			return null;
			//return the best route
		} else {
			Debug.Log("Testing Path");
			visited.Pop ();
			pathSoFar.AddLast (candidatePath.First);
			return pathSoFar;
		}


	}



}
