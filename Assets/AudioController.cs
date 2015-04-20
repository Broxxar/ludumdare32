using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}
}
