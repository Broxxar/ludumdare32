using UnityEngine;
using System.Collections;
	
	
public enum PoliticianState{laughing, walking};

public class PoliticianController : MonoBehaviour {

	private float moveSpeed = 3.0f;
	public WayPoint[] Route;//first, 
	int routeIndex=0;
	bool performedEvent = false;
	PoliticianState currentState;
	

	// Use this for initialization
	void Start () {

		EventManager.Instance.OnEventChange += HandleOnEventChange;
		EventManager.Instance.SetEventState (StoryEvents.movingBetween);
		currentState = PoliticianState.walking;

	}

	void Update(){
		if(transform.position != Route[routeIndex].transform.position){

			transform.position = Vector3.MoveTowards (transform.position, Route[routeIndex].transform.position, Time.deltaTime * moveSpeed);
		}else{
			StoryEvents currentEvent = EventManager.Instance.currentEvent;
			if(currentEvent == StoryEvents.RibbonCutting){
				if(!performedEvent){
					performedEvent = true;
				//	Debug.Log ("Ribbon Cutting going down");
					StartCoroutine(RunRibbonEvent());
				}
			}else if(currentEvent == StoryEvents.HotDog){
				if(!performedEvent){
					performedEvent = true;
				//	Debug.Log ("HOTDOG!");
					StartCoroutine(RunHotDogEvent());
				}
			}else if(currentEvent == StoryEvents.StealCandy){
				if(!performedEvent){
					performedEvent = true;
				//	Debug.Log ("That bitch stole some candy");
					StartCoroutine(RunStealCandyEvent());
				}
			}else if(currentEvent == StoryEvents.LudeActs){
				if(!performedEvent){
					performedEvent = true;
				//	Debug.Log ("OH MY!");
					StartCoroutine(RunLudeActsEvent());
				}
			}


		}
	}

	void EventEnds(){
		//after event ends send him on his way
		EventWayPoint eventWP =Route[routeIndex].GetComponent<EventWayPoint>();
		if(eventWP != null){
			eventWP.HasFinished = true;
		}
		IncrementRouteIndex();
		EventManager.Instance.SetEventState (StoryEvents.movingBetween);
	}
	
	public void IncrementRouteIndex(){
		if(routeIndex < Route.Length-1)
			routeIndex++;
	}



	void HandleOnEventChange ()
	{
		performedEvent = false;

	}
	
	IEnumerator RunRibbonEvent(){

		//Run animation
		currentState = PoliticianState.laughing;
		//wait for it to end
		yield return new WaitForSeconds (5.0f);

		currentState = PoliticianState.walking;
		//Debug.Log ("event Over");
		EventEnds ();
	}

	IEnumerator RunHotDogEvent(){

		//Run animation
			//wait for it to end
		yield return new WaitForSeconds (5.0f);
		

		//Debug.Log ("event Over");
		EventEnds ();
	}

	IEnumerator RunStealCandyEvent(){
		//Run animation
			//wait for it to end
		yield return new WaitForSeconds (5.0f);
		

	//	Debug.Log ("event Over");
		EventEnds ();
	}
	IEnumerator RunLudeActsEvent(){

		//Run animation
		//wait for it to end
		yield return new WaitForSeconds (5.0f);
		
	
	//	Debug.Log ("event Over");
		EventEnds ();


	}

	public PoliticianState GetState(){
		return currentState;
	}
}
