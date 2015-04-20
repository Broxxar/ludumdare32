using UnityEngine;
using System.Collections;

public class LoadSceneTrigger : MonoBehaviour
{
	public string SceneToLoad;
	public Vector3 NewPlayerPosition;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			SceneController.Instance.FadeToScene(SceneToLoad, other.transform, NewPlayerPosition);
		}
	}
}
