using UnityEngine;
using System.Collections;

public class DogController : MonoBehaviour {

	Animator _anim;
	ParticleSystem _particleSystem;
	int _stepHash; 
	public HotDogGuyController HotDogGuy;
	float _moveSpeed = 3;
	void Start(){
		_particleSystem = transform.GetComponentInChildren<ParticleSystem>();
		_anim = GetComponent<Animator>();
		_stepHash = Animator.StringToHash("stepAttack");
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<PoliticianController>() != null){
			HotDogGuy.finishedBeingAttacked = true;
		}
		Debug.Log (other.name);
	}

	public void TriggerDogAttack(){
		StartCoroutine (DogAttackAnim());

	}

	IEnumerator DogAttackAnim(){
		//run to hot dog guy
		_anim.SetTrigger(_stepHash);
		if(HotDogGuy!= null){
		Vector3 target = HotDogGuy.transform.position + new Vector3(.2f, -.1f, 0);
			while((Vector2)transform.position != (Vector2)target){
				Move((Vector2)target);
				yield return null;
			}
			while(!HotDogGuy.finishedBeingAttacked){
				yield return null;
			}
			yield return new WaitForSeconds (3);
		}
		//die
		_anim.SetTrigger(_stepHash);
		yield return null;


	}

	protected void Move(Vector3 moveTarget)
	{
		transform.position = Vector2.MoveTowards (transform.position, moveTarget, Time.deltaTime * _moveSpeed);
	}
}
