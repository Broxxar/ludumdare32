using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour
{
	public Animator Title;
	public Animator FakePolaroid;
	
	PlayerController _player;
	string [] _dialogue = new string[]
	{
		"Listen up kid! We gotta client payin' us big bucks for some photos...",
		"Our client wants da Mayor's name dragged through da mud, see?",
		"We're gonna use <color=yellow>da greatest weapon</color> at our disposal...",
		"PHOTO JOURNALISM!",
		"Here's a picture of da <color=yellow>Mayor</color>...",
		"Now get out there and take <color=yellow>compromising photos</color> of da Mayor around town!",
		"(Use WASD to move your journalist and left mouse button to take a photo.)",
	};

	void Awake ()
	{
		_player = FindObjectOfType<PlayerController>();
		_player.Frozen = true;
	}

	void Start ()
	{
		StartCoroutine(RunIntro());
	}
	
	IEnumerator RunIntro()
	{
		GUIController.Instance.FadeInFromBlack();
		
		yield return StartCoroutine(WaitForClick());
		Title.SetTrigger("Dismiss");
		yield return new WaitForSeconds(1.0f);
		
		GUIController.Instance.ShowText(_dialogue[0]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[1]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[2]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[3]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[4]);
		FakePolaroid.SetTrigger("Spawn");
		yield return StartCoroutine(WaitForClick());
		FakePolaroid.SetTrigger("Despawn");
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[5]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		
		_player.Frozen = false;
		
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[6]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		
		SceneController.Instance.FadeToScene("city");
	}
	
	IEnumerator WaitForClick ()
	{
		while (!Input.GetMouseButtonDown(0))
			yield return null;	
	}
}
