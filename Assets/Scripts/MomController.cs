using UnityEngine;
using System.Collections;

public class MomController : MonoBehaviour {

	public EventWayPoint DestinationWP;
	public PoliticianController politician;
	Vector3 targetPos ;
	float moveSpeed = 2;
	bool droppedCandy = false;
	public bool givenNewCandy = false;
	Animator anim;
	Vector3 needPoliticianAt;
	int walkHash = Animator.StringToHash("walk");
	int idleHash = Animator.StringToHash("idle");
	int stepCandyHash = Animator.StringToHash ("stepCandy");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		//anim.SetTrigger (walkHash);
		EventManager.Instance.OnEventChange += HandleOnEventChange;
	}

	void HandleOnEventChange(){
		if(EventManager.Instance.currentEvent == StoryEvents.HotDogOver){
			anim.SetTrigger (walkHash);
			targetPos = new Vector3 (DestinationWP.transform.position.x, DestinationWP.transform.position.y - 2, 0);
		}
	}

	// Update is called once per frame
	void Update () {

		if((targetPos != Vector3.zero)&&(Vector2)transform.position != (Vector2)targetPos){
			Move (targetPos);
		}else if((targetPos != Vector3.zero)&&!droppedCandy && !givenNewCandy){
			//drop Candy
			anim.SetTrigger(idleHash);
			droppedCandy = true;
			StartCoroutine(DropCandy());

		}else if(politician.currentState == PoliticianState.givingCandy&& droppedCandy && !givenNewCandy){
			//getnew candy
			anim.SetTrigger(stepCandyHash);
			givenNewCandy = true;
		}

	}

	IEnumerator DropCandy(){
		EventManager.Instance.SetEventState(StoryEvents.StealCandy);
		yield return new WaitForSeconds (1);
		anim.SetTrigger (stepCandyHash);
		politician.moveTarget = transform.position + new Vector3 (2,0,0);
		yield return new WaitForSeconds (2);

		anim.SetTrigger (stepCandyHash);
	}


	protected void Move(Vector3 moveTarget)
	{
		transform.position = Vector2.MoveTowards (transform.position, moveTarget, Time.deltaTime * moveSpeed);
	}
	

}
