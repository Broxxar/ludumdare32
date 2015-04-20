using UnityEngine;
using System.Collections;
	
	
public enum PoliticianState{laughing, walking, hurtDog};

public class PoliticianController : MonoBehaviour {

	public WayPoint[] Route;//first, 
	public RibbonController ribbonController;
	public Vector3 moveTarget;
	public PoliticianState currentState;


	float moveSpeed = 1.0f;
	int routeIndex=0;
	bool performedEvent = false;

	
	//animationVars
	Animator anim;
	int cameraFlashHash = Animator.StringToHash("CameraFlash");
	int isWalkingHash = Animator.StringToHash("IsWalking");
	int stepRibbonHash = Animator.StringToHash("StepRibbon");
	int roughUpHash = Animator.StringToHash("IsRough");



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool (isWalkingHash, true);
		EventManager.Instance.OnEventChange += HandleOnEventChange;
		EventManager.Instance.SetEventState (StoryEvents.movingBetween);
		currentState = PoliticianState.walking;
		moveTarget = (Vector2)Route[routeIndex].transform.position;
	}

	void Update(){
		StoryEvents currentEvent = EventManager.Instance.currentEvent;
		if((Vector2)transform.position != (Vector2)moveTarget){

			if(currentEvent == StoryEvents.HotDog){
				anim.SetBool(roughUpHash, true);

			}
			UpdateMovement();
		}else{

		
			if(currentEvent == StoryEvents.RibbonCutting){
				if(!performedEvent){
					performedEvent = true;
				//	Debug.Log ("Ribbon Cutting going down");
					StartCoroutine(RunRibbonEvent());
				}
			}else if(currentEvent == StoryEvents.HotDog){

				if(!performedEvent){
					performedEvent = true;
					moveSpeed = 3f;
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
	
	void UpdateMovement () {
		transform.position = Vector2.MoveTowards (transform.position, moveTarget, Time.deltaTime * moveSpeed);
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
		if(routeIndex < Route.Length-1) { 
			routeIndex++;
			moveTarget = (Vector2)Route[routeIndex].transform.position;
		}
	}



	void HandleOnEventChange ()
	{
		performedEvent = false;

	}
	
	IEnumerator RunRibbonEvent(){

		//Run animations

		//withdraw_scissors
		anim.SetTrigger (stepRibbonHash);
		yield return new WaitForSeconds (1.0f);
		//maniac_laughter will start automatically
		currentState = PoliticianState.laughing;
		yield return new WaitForSeconds (2.0f);
		currentState = PoliticianState.walking;

		//triggerwalking towards ribbon
		anim.SetTrigger (stepRibbonHash);
		moveTarget = moveTarget + new Vector3 (0,1.5f,0);
		while((Vector2)transform.position != (Vector2)moveTarget){

			yield return null;
		}
		//trigger cut
		anim.SetTrigger (stepRibbonHash);
		yield return new WaitForSeconds (0.5f);
		//ribbonController;
		ribbonController.CutTheRibbon ();


		//wait for it to end
		yield return new WaitForSeconds (1f);


		//walk away
		anim.SetTrigger (stepRibbonHash);
		EventEnds ();
	}

	IEnumerator RunHotDogEvent(){
		//Debug.Log ("Hotdogscoroutine started");
		//Run animation

		//startDogs 
		//anim.SetBool (roughUpHash, true);
		//currentState = PoliticianState.hurtDog;

		//wait for it to end
		yield return new WaitForSeconds (2.0f);
		currentState = PoliticianState.walking;
		anim.SetBool (roughUpHash, false);
		moveSpeed = 3;

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
