using UnityEngine;
using System.Collections;

public class EventWayPoint : MonoBehaviour {

	public Events ThisEvent;

	void OnTriggerEvent2D(Collider2D other){
		if(other.GetComponent<PoliticianController>()){
			EventManager.Instance.SetEventState(ThisEvent);
		}
		
	}
}
