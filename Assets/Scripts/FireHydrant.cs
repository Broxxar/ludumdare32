using UnityEngine;
using System.Collections;

public class FireHydrant : MonoBehaviour {

	public DogController dog;

	void OnTriggerEnter2D(Collider2D other){

		if (other.GetComponent<PoliticianController>() != null) {
			dog.TriggerDogAttack ();
			//EventManager.Instance.SetEventState(StoryEvents.HotDog);
		}
	}
}
