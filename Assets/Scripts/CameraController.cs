using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform FollowTarget;
	public float MouseInfluence;
	public float FixedZ;
	public float FollowStrength;
	
	Camera _camera;
	
	void Awake ()
	{
		_camera = GetComponent<Camera>();	
		Vector3 newPosition = FollowTarget.position;
		newPosition.z = FixedZ;
		transform.position = newPosition;
	}
	
	void LateUpdate ()
	{
		Vector3 mousePos = (_camera.ScreenToViewportPoint(Input.mousePosition) * 2);
		
		mousePos.x = Mathf.Clamp(mousePos.x-1,-1,1);
		mousePos.y = Mathf.Clamp(mousePos.y-1,-1,1);

		Vector3 newPosition = Vector3.Lerp(
			transform.position,
			FollowTarget.position + mousePos * MouseInfluence,
			Time.deltaTime * FollowStrength);
			
		newPosition.z = FixedZ;
		transform.position = newPosition;
	}
}
