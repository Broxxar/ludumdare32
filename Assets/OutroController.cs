using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OutroController : MonoBehaviour
{
	public Animator RibbonPhotoAnim;
	public Animator HotdogPhotoAnim;
	public Animator BabbyPhotoAnim;
	
	int _successCount = 0;

	PlayerController _player;
	string [] _dialogue = new string[]
	{
		"Let's see how ya did...",
		"Now get da hell outta my office!"
	};
	
	string [] _closingDialogue = new string[]
	{
		"These are terrible! If anything ya made the mayor look good!",
		"Hmm... you only got one usable photo, our client ain't gonna be happy!",
		"Nicely done, the mayor don't look too good in these.",
		"Incredible! You made dis dame look like a real clown! Well done kiddo!"
	};
	
	void Awake ()
	{
		_player = FindObjectOfType<PlayerController>();
		_player.Frozen = true;
	}
	
	void Start ()
	{
		StartCoroutine(RunOutro());
	}
	
	IEnumerator RunOutro()
	{
		GUIController.Instance.FadeInFromBlack();
		
		GUIController.Instance.ShowText(_dialogue[0]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		PhotoInfo ribbonInfo;
		_player.SavedPhotos.TryGetValue(StoryEvents.RibbonCutting, out ribbonInfo);
		
		PhotoInfo hotdogInfo;
		_player.SavedPhotos.TryGetValue(StoryEvents.HotDog, out hotdogInfo);
		
		PhotoInfo babbyInfo;
		_player.SavedPhotos.TryGetValue(StoryEvents.StealCandy, out babbyInfo);
		
		
		if (ribbonInfo != null)
		{
			RibbonPhotoAnim.transform.FindChild("image").GetComponent<Image>().sprite = ribbonInfo.PhotoSprite;
			
			if (ribbonInfo.Successful)
			{
				RibbonPhotoAnim.transform.FindChild("scrap_paper/text").GetComponent<Text>().text = "Mayor goes on jumbo scissor rampage!";
				_successCount ++;
			}
			else
				RibbonPhotoAnim.transform.FindChild("scrap_paper/text").GetComponent<Text>().text = "Mayor brings laughter to hospital opening!";
		}
		
		if (hotdogInfo != null)
		{
			HotdogPhotoAnim.transform.FindChild("image").GetComponent<Image>().sprite = hotdogInfo.PhotoSprite;
			
			if (hotdogInfo.Successful)
			{
				HotdogPhotoAnim.transform.FindChild("scrap_paper/text").GetComponent<Text>().text = "Mayor kicks puppy for no reason!";
				_successCount ++;
			}
			else
				HotdogPhotoAnim.transform.FindChild("scrap_paper/text").GetComponent<Text>().text = "Mayor saves local business owner!";
		}
		
		if (babbyInfo != null)
		{
			BabbyPhotoAnim.transform.FindChild("image").GetComponent<Image>().sprite = babbyInfo.PhotoSprite;
			
			if (babbyInfo.Successful)
			{
				BabbyPhotoAnim.transform.FindChild("scrap_paper/text").GetComponent<Text>().text = "Mayor steals candy from a baby!";
				_successCount ++;
			}
			else
				BabbyPhotoAnim.transform.FindChild("scrap_paper/text").GetComponent<Text>().text = "Mayor brings joy to small child!";
		}
			
		RibbonPhotoAnim.SetTrigger("Spawn");
		yield return new WaitForSeconds(0.3f);
		HotdogPhotoAnim.SetTrigger("Spawn");
		yield return new WaitForSeconds(0.3f);
		BabbyPhotoAnim.SetTrigger("Spawn");
				
		yield return new WaitForSeconds(1.5f);
		yield return StartCoroutine(WaitForClick());
		
		RibbonPhotoAnim.SetTrigger("Despawn");
		HotdogPhotoAnim.SetTrigger("Despawn");
		BabbyPhotoAnim.SetTrigger("Despawn");
		
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_closingDialogue[_successCount]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		GUIController.Instance.ShowText(_dialogue[1]);
		yield return StartCoroutine(WaitForClick());
		GUIController.Instance.HideText();
		yield return new WaitForSeconds(0.5f);
		
		SceneController.Instance.FadeToScene("victory_screen", _player.transform, Vector3.zero);
	}
	
	IEnumerator WaitForClick ()
	{
		while (!Input.GetMouseButtonDown(0))
			yield return null;	
	}
}
