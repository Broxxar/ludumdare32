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
	public float MinZoomSize;
	public float MaxZoomSize;
	public AudioClip StunSound;
	
	bool _stunned = false;
	public bool Frozen = false;

	AudioSource _audioSrc;
	Animator _anim;
	Transform _view;
	PlayerVirtualCamera _playerCam;
	Camera _playerCamCamera;
	AudioSource _zoomAudioSrc;
	float _zoomSize;
	BoxCollider2D _photoCollider;
	CameraController _camController;

	public Dictionary<StoryEvents, PhotoInfo> SavedPhotos = new Dictionary<StoryEvents, PhotoInfo>();

	int _cameraFlashHash = Animator.StringToHash("CameraFlash");
	int _isWalkingHash = Animator.StringToHash("IsWalking");
	int _isStunnedHash = Animator.StringToHash("IsStunned");

	bool _canTakePicture = true;

	void Awake ()
	{
		_audioSrc = GetComponent<AudioSource>();
		_anim = GetComponent<Animator>();
		_view = transform.FindChild("view");
		_playerCam = _view.GetComponentInChildren<PlayerVirtualCamera>();
		_playerCamCamera = _playerCam.GetComponent<Camera>();
		_zoomAudioSrc = _playerCam.GetComponent<AudioSource>();
		_zoomSize = _playerCamCamera.orthographicSize;
		_photoCollider = _view.GetComponentInChildren<BoxCollider2D>();
		_camController = FindObjectOfType<CameraController>();

		DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
	{
		_anim.SetBool(_isStunnedHash, _stunned);
	
		if (!_stunned && !Frozen)
		{
			UpdateCameraShoot();
			UpdateMovement();
			UpdateView();
			UpdateZoom();
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
		OnTakePhoto();
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
		if (!_stunned)
			StartCoroutine(StunAsync());
	}
	
	IEnumerator StunAsync ()
	{
		_stunned = true;
		_camController.Shake();
		_audioSrc.PlayOneShot(StunSound);
		yield return new WaitForSeconds(2.5f);
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

	void UpdateZoom ()
	{
		_zoomSize = Mathf.Clamp(_zoomSize - Input.GetAxis("Mouse ScrollWheel"),MinZoomSize, MaxZoomSize);
		_playerCamCamera.orthographicSize = Mathf.Lerp(_playerCamCamera.orthographicSize, _zoomSize, Time.deltaTime * 5);
		_playerCam.transform.localScale = Vector3.one * _playerCamCamera.orthographicSize;
		_zoomAudioSrc.volume = Mathf.Abs(_playerCamCamera.orthographicSize - _zoomSize);
	}
}
