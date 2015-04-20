using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public delegate void TakingPhoto();
	public static event TakingPhoto OnTakePhoto;

	public float MoveSpeed;
	public float ViewRotationStrength;

	public bool Stunned = false;

	Animator _anim;
	Transform _view;
	PlayerVirtualCamera _playerCam;
	BoxCollider2D _photoCollider;
	PoliticianController _politician;

	int _cameraFlashHash = Animator.StringToHash("CameraFlash");
	int _isWalkingHash = Animator.StringToHash("IsWalking");

	bool _canTakePicture = true;

	void Awake ()
	{
		_anim = GetComponent<Animator>();
		_view = transform.FindChild("view");
		_playerCam = _view.GetComponentInChildren<PlayerVirtualCamera>();
		_photoCollider = _view.GetComponentInChildren<BoxCollider2D>();
		_politician = GameObject.FindObjectOfType<PoliticianController>();

	}
	
	void Update ()
	{
		if (!Stunned) {
			UpdateCameraShoot ();
			UpdateMovement ();
			UpdateView();
		}
	}
	
	void UpdateCameraShoot ()
	{
		if (Input.GetMouseButtonDown(0))
			TakePhoto();
	}
	
	void TakePhoto ()
	{
		if (_canTakePicture) {
			StartCoroutine (TakePhotoAsync ());
			OnTakePhoto();
		}
	}
	
	IEnumerator TakePhotoAsync ()
	{
		// Pre Photo
		_canTakePicture = false;
		_anim.SetTrigger(_cameraFlashHash);	
		yield return new WaitForSeconds(0.3f);
		
		// Take Photo
		_playerCam.Capture();
		yield return new WaitForEndOfFrame();
		GUIController.Instance.SpawnPolaroid(_playerCam.RecentPhoto);

		//Evaluate Photo
		bool isPhotoGood = false;
		StoryEvents currentEvent = EventManager.Instance.currentEvent;
		
		foreach(EventWayPoint wp in EventManager.Instance.eventWPs)
		{
			if(wp.ThisEvent == currentEvent){
				isPhotoGood = PhotoEvalHelper.EvaluatePhoto(_photoCollider,wp, _politician.GetState());
			}
		}
		Debug.Log (isPhotoGood);
		yield return new WaitForSeconds(0.45f);
		// Post Photo
		
		_canTakePicture = true;
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
