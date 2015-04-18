using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float MoveSpeed;
	public float ViewRotationStrength;
	
	Animator _anim;
	Transform _view;

	int _cameraFlashHash = Animator.StringToHash("CameraFlash");
	int _isWalkingHash = Animator.StringToHash("IsWalking");

	void Awake ()
	{
		_anim = GetComponent<Animator>();
		_view = transform.FindChild("view");
	}
	
	void Update ()
	{
		UpdateCameraShoot();
		UpdateMovement();
		UpdateView();
	}
	
	void UpdateCameraShoot ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_anim.SetTrigger(_cameraFlashHash);
		}
	}
	
	void UpdateMovement ()
	{
		Vector3 movement = Vector3.zero;
		
		if (Input.GetKey(KeyCode.W))
			movement.y += 1;
		if (Input.GetKey(KeyCode.A))
			movement.x -= 1;
		if (Input.GetKey(KeyCode.S))
			movement.y -= 1;
		if (Input.GetKey(KeyCode.D))
			movement.x += 1;
		
		_anim.SetBool(_isWalkingHash, movement != Vector3.zero);
		transform.Translate (movement.normalized * MoveSpeed * Time.deltaTime);
	}
	
	void UpdateView ()
	{
		Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseWorld.z = _view.position.z;
		Vector3 desiredFacing = (mouseWorld - _view.position).normalized;
		
		Quaternion desiredRotation = Quaternion.FromToRotation(Vector3.up, desiredFacing);
		
		if (desiredRotation.eulerAngles.y != 0)
		{
			Vector3 fixedEuler = desiredRotation.eulerAngles;
			fixedEuler.y = 0;
			desiredRotation.eulerAngles = fixedEuler;
		}
		
		_view.rotation = Quaternion.Lerp(_view.rotation, desiredRotation, ViewRotationStrength * Time.deltaTime);
	}
}
