using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
	static SceneController _instance;
	
	public static SceneController Instance
	{
		get { return _instance ?? GameObject.FindObjectOfType<SceneController>(); }
	}

	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void FadeToScene (string sceneName)
	{
		StartCoroutine(FadeToSceneAsync(sceneName));
	}
	
	IEnumerator FadeToSceneAsync(string sceneName)
	{
		GUIController.Instance.FadeToBlack();
		yield return new WaitForSeconds (1.1f);
		Application.LoadLevel(sceneName);
		GUIController.Instance.FadeInFromBlack();
	}
}
