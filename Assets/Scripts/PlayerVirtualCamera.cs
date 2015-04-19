using UnityEngine;
using System.Collections;

public class PlayerVirtualCamera : MonoBehaviour
{
	public Texture2D RecentPhoto
	{
		get { return _lastPhoto; }
	}

	bool _grab;
	RenderTexture _cameraRT;
	Texture2D _lastPhoto;

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
}
