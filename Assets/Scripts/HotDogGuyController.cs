using UnityEngine;
using System.Collections;

public class HotDogGuyController : MonoBehaviour {

	Animator anim;
	int hotdogHash = Animator.StringToHash("stepHotDog");
	public bool finishedBeingAttacked = false;

	void Start(){
		anim = GetComponent<Animator>();
		GetComponent<CharacterBehaviour> ().BeIdle ();
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.GetComponent<DogController> () != null) {

			StartCoroutine(RunAway());
		}

	}

	IEnumerator RunAway(){
		//panick
		anim.SetTrigger (hotdogHash);
		//be attacked

		while(!finishedBeingAttacked){
			yield return null;
		}
		yield return new WaitForSeconds (1);
		//start running
		//runaway
		GetComponent<CharacterBehaviour> ().StopIdle ();
		yield return null;

	}

}
