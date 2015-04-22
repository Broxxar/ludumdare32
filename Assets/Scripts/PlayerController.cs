using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	public delegate void TakingPhoto();
	public static event TakingPhoto OnTakePhoto = delegate { };
	public AudioClip CameraSound;
	public float MoveSpeed;
	public float ViewRotationStrength;

	bool _stunned = false;
	public bool Frozen = false;

	Animator _anim;
	Transform _view;
	PlayerVirtualCamera _playerCam;
	BoxCollider2D _photoCollider;

	public Dictionary<StoryEvents, PhotoInfo> SavedPhotos = new Dictionary<StoryEvents, PhotoInfo>();

	int _cameraFlashHash = Animator.StringToHash("CameraFlash");
	int _isWalkingHash = Animator.StringToHash("IsWalking");
	int _isStunnedHash = Animator.StringToHash("IsStunned");

	bool _canTakePicture = true;

	void Awake ()
	{
		_anim = GetComponent<Animator>();
		_view = transform.FindChild("view");
		_playerCam = _view.GetComponentInChildren<PlayerVirtualCamera>();
		_photoCollider = _view.GetComponentInChildren<BoxCollider2D>();

		DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
	{
		_anim.SetBool(_isStunnedHash, _stunned);
	
		if (!_stunned && !Frozen)
		{
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
		if (_canTakePicture)
		{
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
		audio.PlayOneShot(CameraSound);
		GUIController.Instance.CameraFlash();
		// Take Photo
		_playerCam.Capture();
		yield return new WaitForEndOfFrame();
		
		Sprite photoSprite = Sprite.Create(_playerCam.RecentPhoto, new Rect(0,0, 512, 512), Vector2.zero);

		//Evaluate Photo
		bool isPhotoGood = false;
		bool isPhotoValid = false;

		if (EventManager.Instance != null)
		{
			StoryEvents currentEvent = EventManager.Instance.currentEvent;
			
			foreach(EventWayPoint wp in EventManager.Instance.eventWPs)
			{
				if(wp != null && wp.ThisEvent == currentEvent){

					isPhotoGood = PhotoEvalHelper.EvaluatePhoto(_playerCam, wp, GameObject.FindObjectOfType<PoliticianController>().GetState());
					isPhotoValid = PhotoEvalHelper.ValidatePhoto(_photoCollider, wp);

					if (isPhotoValid)
					{
						PhotoInfo newPhotoInfo = new PhotoInfo
						{
							PhotoSprite = photoSprite,
							AssociatedEvent = currentEvent,
							Successful = isPhotoGood
						};

						if (SavedPhotos.ContainsKey(currentEvent))
						{
							if (newPhotoInfo.Successful && !SavedPhotos[currentEvent].Successful)
								SavedPhotos[currentEvent] = newPhotoInfo;
						}
						else
						{
							SavedPhotos.Add(currentEvent, newPhotoInfo);
						}	
					}
				}
			}
		}
		
		GUIController.Instance.SpawnPolaroid(photoSprite, isPhotoValid, isPhotoGood);

		yield return new WaitForSeconds(0.45f);
		
		// Post Photo
		_canTakePicture = true;
	}
	
	public void Stun ()
	{
		StartCoroutine(StunAsync());
	}
	
	IEnumerator StunAsync ()
	{
		_stunned = true;
		yield return new WaitForSeconds(3.0f);
		_stunned = false;
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
