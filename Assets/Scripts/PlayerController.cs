using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	public delegate void TakingPhoto();
	public static event TakingPhoto OnTakePhoto = delegate { };

	public float MoveSpeed;
	public float ViewRotationStrength;

	public bool Stunned = false;
	public bool Frozen = false;

	Animator _anim;
	Transform _view;
	PlayerVirtualCamera _playerCam;
	BoxCollider2D _photoCollider;
	PoliticianController _politician;

	Dictionary<StoryEvents, PhotoInfo> SavedPhotos = new Dictionary<StoryEvents, PhotoInfo>();

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
		if (!Stunned && !Frozen)
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
		
		// Take Photo
		_playerCam.Capture();
		yield return new WaitForEndOfFrame();
		
		Sprite photoSprite = Sprite.Create(_playerCam.RecentPhoto, new Rect(0,0, 512, 512), Vector2.zero);
		GUIController.Instance.SpawnPolaroid(photoSprite);

		//Evaluate Photo
		bool isPhotoGood = false;
		bool isPhotoValid = false;

		if (EventManager.Instance != null)
		{
			StoryEvents currentEvent = EventManager.Instance.currentEvent;
			
			foreach(EventWayPoint wp in EventManager.Instance.eventWPs)
			{
				if(wp != null && wp.ThisEvent == currentEvent){

					isPhotoGood = PhotoEvalHelper.EvaluatePhoto(_photoCollider, wp, _politician.GetState());
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

					Debug.Log ("Good: " + isPhotoGood);
					Debug.Log ("Valid: " + isPhotoValid);

				}
			}
		}

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
