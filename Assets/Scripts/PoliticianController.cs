using UnityEngine;
using System.Collections;

public class PoliticianController : MonoBehaviour {

	private float moveSpeed = 3.0f;
	public WayPoint[] Route; 
	int routeIndex=0;

	// Use this for initialization
	void Start () {
		Debug.Log ("start");
		EventManager.Instance.OnEventChange += HandleOnEventChange;
		EventManager.Instance.SetEventState (Events.movingBetween);
		Debug.Log ("End Start");
	}

	void Update(){
		while(EventManager.Instance.currentEvent == Events.movingBetween && transform.position != Route[routeIndex].transform.position){
			transform.position = Vector3.MoveTowards (transform.position, Route[routeIndex].transform.position, Time.deltaTime * moveSpeed);
		}
	}

	void HandleOnEventChange ()
	{
		if (EventManager.Instance.currentEvent == Events.RibbonCutting) {
			RunRibbonEvent();
		}
		
	}
	
	void RunRibbonEvent(){

	}
}
