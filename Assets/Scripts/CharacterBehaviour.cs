using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterBehaviour : MonoBehaviour
{
	public WayPoint CurrentWP;
	public WayPoint DestinationWP;
	public EventWayPoint EventWP;
	public float MoveSpeed;
	[Range (0, 1)]
	public float ChanceToIdle;
	public float MinIdleTime;
	public float MaxIdleTime;



	Animator _anim;
	int _isWalkingHash = Animator.StringToHash("IsWalking");
	
	WayPoint _previousWP;
	bool _idling;
	Vector3 _currentMoveTarget;

	void Awake ()
	{
		_anim = GetComponent<Animator>();
	}

	void Start () 
	{
		if(EventWP != null){
			_idling = true;
		}
		CurrentWP = CurrentWP ?? GameObject.FindObjectsOfType<WayPoint>()
			.OrderBy(wp => Vector3.Distance((Vector2)wp.transform.position, (Vector2)transform.position))
			.First().GetComponent<WayPoint>();

		GetNewMoveTarget(CurrentWP);
	}

	void Update ()
	{
		if(EventWP != null && EventWP.HasFinished){
			_idling = false;
		}

		// If arrived at target location, find a random neighbour and set it as the new Move Target
		if ((Vector2)transform.position == (Vector2)_currentMoveTarget) {	
			_previousWP = CurrentWP;
			CurrentWP = CurrentWP.neighbours
				.Where(wp => wp != _previousWP || CurrentWP.neighbours.Count == 1)
				.OrderBy(wp => Random.Range(0f, 1f))
				.First();
			GetNewMoveTarget(CurrentWP);
			
			if (Random.Range(0f, 1f) < ChanceToIdle)
				Idle ();
		}
		
		if (!_idling)
			Move (_currentMoveTarget);
			
		_anim.SetBool(_isWalkingHash, !_idling);
	}
	
	void Idle ()
	{
		StartCoroutine(IdleAsync(Random.Range(MinIdleTime, MaxIdleTime)));
	}
	
	IEnumerator IdleAsync (float idleDuration)
	{
		_idling = true;
		yield return new WaitForSeconds(idleDuration);
		_idling = false;
	}

	protected void GetNewMoveTarget(WayPoint destinationWP)
	{
		float radius = destinationWP.GetComponent<CircleCollider2D>().radius;
		_currentMoveTarget = (Vector2)destinationWP.transform.position + (Vector2)(Random.insideUnitCircle * radius);
	}

	protected void Move(Vector3 moveTarget)
	{
		transform.position = Vector2.MoveTowards (transform.position, moveTarget, Time.deltaTime * MoveSpeed);
	}
}
