using UnityEngine;
using System.Collections;

public class RibbonController : MonoBehaviour
{
	Animator _anim;
	ParticleSystem _particleSystem;
	int _cutHash; 

	void Start(){
		_particleSystem = transform.GetComponentInChildren<ParticleSystem>();
		_anim = GetComponent<Animator>();
		_cutHash = Animator.StringToHash("Cut");
	}

	public void CutTheRibbon(){
		_anim.SetTrigger (_cutHash);
		_particleSystem.Play ();
	}
}
