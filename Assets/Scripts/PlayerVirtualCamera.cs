using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVirtualCamera : MonoBehaviour
{
	public Texture2D RecentPhoto
	{
		get { return _lastPhoto; }
	}

	bool _grab;
	RenderTexture _cameraRT;
	Texture2D _lastPhoto;
	List<GameObject> _containedObjects = new List<GameObject>();

	void Awake ()
	{
		_cameraRT = GetComponent<Camera>().targetTexture;
	}

	public void Capture ()
	{
		_grab = true;
	}

	void OnPostRender ()
	{
		if (_grab)
		{
			_lastPhoto = new Texture2D(_cameraRT.width,_cameraRT.height, TextureFormat.RGB24, false);
			_lastPhoto.ReadPixels(new Rect(0,0,_cameraRT.width,_cameraRT.height),0,0);
			_lastPhoto.Apply();			
			_grab = false;
		}
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (!_containedObjects.Contains(other.gameObject))
			_containedObjects.Add(other.gameObject);
	}
	
	void OnTriggerExit2D (Collider2D other)
	{
		if (_containedObjects.Contains(other.gameObject))
			_containedObjects.Remove(other.gameObject);
	}
	
	void OnCollisionEnter2D (Collision2D collision)
	{
		if (!_containedObjects.Contains(collision.collider.gameObject))
			_containedObjects.Add(collision.collider.gameObject);
	}
	
	void OnCollisionExit2D (Collision2D collision)
	{
		if (_containedObjects.Contains(collision.collider.gameObject))
			_containedObjects.Remove(collision.collider.gameObject);
	}
	
	public bool ContainsAll (GameObject[] checkedObjects)
	{
		for (int i = 0; i < checkedObjects.Length; i++)
		{
			if (_containedObjects.Contains(checkedObjects[i]))
				continue;
			return false;
		}
		return true;
	}
	
	public bool ContainsAny (GameObject[] checkedObjects)
	{
		for (int i = 0; i < checkedObjects.Length; i++)
		{
			if (_containedObjects.Contains(checkedObjects[i]))
				return true;
		}
		return false;
	}
}
