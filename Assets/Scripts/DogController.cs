using UnityEngine;
using System.Collections;

public class DogController : MonoBehaviour {

	Animator _anim;
	ParticleSystem _particleSystem;
	int _runHash; 
	
	void Start(){
		_particleSystem = transform.GetComponentInChildren<ParticleSystem>();
		_anim = GetComponent<Animator>();
		_runHash = Animator.StringToHash("");
	}

	public void TriggerDog(){
		_anim.SetTrigger(_runHash);
	}
}
