using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class YZSorting : MonoBehaviour
{
	public bool IsStatic;

	void Start ()
	{
		enabled = !IsStatic;
	}

	void LateUpdate () 
	{
		Vector3 newPosition = transform.position;
		newPosition.z = newPosition.y;
		transform.position = newPosition;
	}
}
