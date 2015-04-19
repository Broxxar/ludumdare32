using UnityEngine;
using System.Collections;

public class EventWayPoint : WayPoint{

	public StoryEvents ThisEvent;
	public Collider2D[] EventRequiredItems;
	public CircleCollider2D[] EventRestrictedItems;


	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<PoliticianController>()){
			EventManager.Instance.SetEventState(ThisEvent);
		}
	}






}
