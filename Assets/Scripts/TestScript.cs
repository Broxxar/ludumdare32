using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestScript : MonoBehaviour {

	public WayPoint start;
	public WayPoint end;
	public LinkedList<WayPoint> path = new LinkedList<WayPoint>();
	//public LinkedList<WayPoint> testPath = new LinkedList<WayPoint>();
	public Stack<WayPoint> visited = new Stack<WayPoint>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		visited.Push (start);

		if (Input.GetKeyDown (KeyCode.Space)) {
			path = start.findPath(end, new LinkedList<WayPoint>(), visited);
			Debug.Log(path.Count);
			//Debug.Log(this.transform.position);

		}


	
	}
}
