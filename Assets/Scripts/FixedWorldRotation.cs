using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FixedWorldRotation : MonoBehaviour
{
	void Update ()
	{
		transform.rotation = Quaternion.identity;
	}
}
