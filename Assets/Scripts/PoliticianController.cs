using UnityEngine;
using System.Collections;

public class PoliticianController : MonoBehaviour {

	private float moveSpeed = 3.0f;
	public WayPoint[] Route;//first, 
	int routeIndex=0;
	bool performedEvent = false;

	// Use this for initialization
	void Start () {

		EventManager.Instance.OnEventChange += HandleOnEventChange;
		EventManager.Instance.SetEventState (Events.movingBetween);

	}

	void Update(){
		if(transform.position != Route[routeIndex].transform.position){

			transform.position = Vector3.MoveTowards (transform.position, Route[routeIndex].transform.position, Time.deltaTime * moveSpeed);
		}else{
			Events currentEvent = EventManager.Instance.currentEvent;
			if(currentEvent == Events.RibbonCutting){
				if(!performedEvent){
					performedEvent = true;
					StartCoroutine(RunRibbonEvent());

				}
			}else if(currentEvent == Events.HotDog){
				if(!performedEvent){
					performedEvent = true;
					StartCoroutine(RunHotDogEvent());
				}
			}else if(currentEvent == Events.StealCandy){
				if(!performedEvent){
					performedEvent = true;
					StartCoroutine(RunStealCandyEvent());
				}
			}else if(currentEvent == Events.LudeActs){
				if(!performedEvent){
					performedEvent = true;
					StartCoroutine(RunLudeActsEvent());
				}
			}
		}
	}

	public void IncrementRouteIndex(){
		if(routeIndex < Route.Length)
			routeIndex++;
	}

	void HandleOnEventChange ()
	{
		performedEvent = false;

	}
	
	IEnumerator RunRibbonEvent(){

		//Run animation
		//wait for it to end
		yield return new WaitForSeconds (1.0f);

		//after event ends send him on his way
		IncrementRouteIndex();
		EventManager.Instance.SetEventState (Events.movingBetween);

	}

	IEnumerator RunHotDogEvent(){

		//Run animation
			//wait for it to end
		yield return new WaitForSeconds (1.0f);
		
		//after event ends send him on his way
		IncrementRouteIndex();
		EventManager.Instance.SetEventState (Events.movingBetween);
	}

	IEnumerator RunStealCandyEvent(){
		//Run animation
			//wait for it to end
		yield return new WaitForSeconds (1.0f);
		
		//after event ends send him on his way
		IncrementRouteIndex();
		EventManager.Instance.SetEventState (Events.movingBetween);
	}
	IEnumerator RunLudeActsEvent(){

		//Run animation
		//wait for it to end
		yield return new WaitForSeconds (1.0f);
		
		//after event ends send him on his way
		IncrementRouteIndex();
		EventManager.Instance.SetEventState (Events.movingBetween);


	}
}
