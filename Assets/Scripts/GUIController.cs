using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour
{
	static GUIController _instance;

	public static GUIController Instance
	{
		get { return _instance ?? GameObject.FindObjectOfType<GUIController>(); }
	}

	public GameObject PolaroidPrefab;
	public AudioClip ShowTextSound;
	public AudioClip HideTextSound;
	
	AudioSource _audio;
	Transform _polaroids;
	Animator _textPanelAnim;
	int _isVisibleHash = Animator.StringToHash("IsVisible");
	Text _textPanelText;
	Image _fadePlane;
	Image _flashPlane;
	
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
		_textPanelAnim = transform.FindChild("text_panel").GetComponent<Animator>();
		_textPanelText = transform.FindChild("text_panel/text").GetComponent<Text>();
		_polaroids = transform.FindChild("polaroids");
		_fadePlane = transform.FindChild("fade_plane").GetComponent<Image>();
		_flashPlane = transform.FindChild("flash_plane").GetComponent<Image>();
		_audio = GetComponent<AudioSource>();
	}

	public void SpawnPolaroid (Sprite photoSprite, bool valid, bool isGood)
	{
		GameObject newPolaroid = GameObject.Instantiate(PolaroidPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPolaroid.name = PolaroidPrefab.name;
		newPolaroid.transform.SetParent(_polaroids);
		
		newPolaroid.transform.FindChild("image").GetComponent<Image>().sprite = photoSprite;
		newPolaroid.GetComponent<Animator>().SetTrigger("Spawn");
		
		if (valid)
		{
			if (isGood)
				newPolaroid.GetComponent<Animator>().SetBool("PhotoPassed", true);
			else
				newPolaroid.GetComponent<Animator>().SetBool("PhotoFailed", true);
		}
		
		StartCoroutine(DespawnAsync(newPolaroid));
	}
	
	public void ShowText(string textToDisplay)
	{
		_audio.PlayOneShot(ShowTextSound);
		_textPanelAnim.SetBool(_isVisibleHash, true);
		_textPanelText.text = textToDisplay;
	}
	
	public void HideText()
	{
		_audio.PlayOneShot(HideTextSound);
		_textPanelAnim.SetBool(_isVisibleHash, false);
	}
	
	public void FadeToBlack ()
	{
		StartCoroutine(CrossFadeColor(_fadePlane, Color.clear, Color.black, 1));
	}
	
	public void FadeInFromBlack ()
	{
		StartCoroutine(CrossFadeColor(_fadePlane, Color.black, Color.clear, 1));
	}
	
	IEnumerator CrossFadeColor (Image plane, Color start, Color end, float duration)
	{
		for (float t = 0; t <= duration; t += Time.deltaTime)
		{
			plane.color = Color.Lerp(start, end, t/duration);
			yield return null;
		}
		plane.color = end;
	}
	
	public void CameraFlash ()
	{
		StartCoroutine(CrossFadeColor(_flashPlane, new Color(1,1,1,0.6f), Color.clear, 0.4f));
	}
	
	IEnumerator DespawnAsync (GameObject polaroid)
	{
		yield return new WaitForSeconds(1.5f);
		polaroid.GetComponent<Animator>().SetTrigger("Despawn");
		yield return new WaitForSeconds(0.25f);
		Destroy(polaroid);
	}
}
