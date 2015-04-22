using UnityEngine;
using System.Collections;

public class EventWayPoint : WayPoint
{
	public StoryEvents ThisEvent;
	public GameObject[] RequiredItems;
	public GameObject[] RestrictedItems;
	public PoliticianState RequiredState;
	public bool HasFinished = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<PoliticianController>())
			EventManager.Instance.SetEventState(ThisEvent);
	}
}