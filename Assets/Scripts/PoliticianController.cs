using UnityEngine;
using System.Collections;
	
public enum PoliticianState
{
	laughing,
	walking,
	hurtDog,
	holdingCandy,
	givingCandy,
	done
}

public class PoliticianController : MonoBehaviour {

	public WayPoint[] Route;//first, 
	public RibbonController ribbonController;
	public Vector3 moveTarget;
	public PoliticianState currentState;
	public MomController mom;

	float moveSpeed = 1.0f;
	int routeIndex=0;
	bool performedEvent = false;
	
	//animationVars
	Animator anim;
	
	int isWalkingHash = Animator.StringToHash("IsWalking");
	int roughUpHash = Animator.StringToHash("IsRough");
	int stepRibbonHash = Animator.StringToHash("StepRibbon");
	int stepCandyHash = Animator.StringToHash("StepCandy");

	// Use this for initialization
	void Start () {
		GUIController.Instance.HideText();
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
			if(currentEvent == StoryEvents.StealCandy && currentState != PoliticianState.holdingCandy && !mom.givenNewCandy){
				StartCoroutine(StartRunningWithCandy());
				
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
					StartCoroutine(RunHotDogEvent());
				}
			}else if(currentEvent == StoryEvents.StealCandy){
				if(!performedEvent){
					performedEvent = true;
				//	Debug.Log ("That bitch stole some candy");
					StartCoroutine(StealCandyEvent());
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

	IEnumerator StartRunningWithCandy(){
		currentState = PoliticianState.holdingCandy;
		yield return new WaitForSeconds(1);
		anim.SetTrigger (stepCandyHash);

		yield return new WaitForSeconds(1);
		anim.SetTrigger(stepCandyHash);
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
	
		//wait for it to end
		yield return new WaitForSeconds (2.0f);
		currentState = PoliticianState.walking;
		anim.SetBool (roughUpHash, false);

		//Debug.Log ("event Over");
		EventManager.Instance.SetEventState (StoryEvents.HotDogOver);

		EventEnds ();

	}

	IEnumerator StealCandyEvent()
	{
		yield return new WaitForSeconds (.50f);
		currentState = PoliticianState.givingCandy;
		anim.SetTrigger (stepCandyHash);
		EventManager.Instance.SetEventState (StoryEvents.movingBetween);
		yield return new WaitForSeconds (2.0f);
		anim.SetTrigger (stepCandyHash);
		yield return new WaitForSeconds (2.05f);
		EventEnds ();
		
		StartCoroutine(Magic());
	}
	
	public LoadSceneTrigger NewsTrigger;
	
	IEnumerator Magic ()
	{
		NewsTrigger.collider2D.enabled = true;
		GUIController.Instance.ShowText("Head back to the News Building!");
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(WaitForClick());
	}
	
	IEnumerator WaitForClick ()
	{
		while (!Input.GetMouseButtonDown(0))
			yield return null;	
	}

	public PoliticianState GetState(){
		return currentState;
	}
}
