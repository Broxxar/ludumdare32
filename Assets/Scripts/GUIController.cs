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

	Transform _polaroids;
	Animator _textPanelAnim;
	int _isVisibleHash = Animator.StringToHash("IsVisible");
	Text _textPanelText;
	Image _fadePlane;
	
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
		_textPanelAnim = transform.FindChild("text_panel").GetComponent<Animator>();
		_textPanelText = transform.FindChild("text_panel/text").GetComponent<Text>();
		_polaroids = transform.FindChild("polaroids");
		_fadePlane = transform.FindChild("fade_plane").GetComponent<Image>();
	}

	public void SpawnPolaroid (Sprite photoSprite)
	{
		GameObject newPolaroid = GameObject.Instantiate(PolaroidPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPolaroid.name = PolaroidPrefab.name;
		newPolaroid.transform.SetParent(_polaroids);
		
		newPolaroid.transform.FindChild("image").GetComponent<Image>().sprite = photoSprite;
		newPolaroid.GetComponent<Animator>().SetTrigger("Spawn");
		
		StartCoroutine(DespawnAsync(newPolaroid));
	}
	
	public void ShowText(string textToDisplay)
	{
		_textPanelAnim.SetBool(_isVisibleHash, true);
		_textPanelText.text = textToDisplay;
	}
	
	public void HideText()
	{
		_textPanelAnim.SetBool(_isVisibleHash, false);
	}
	
	public void FadeToBlack ()
	{
		StartCoroutine(CrossFadeColor(Color.clear, Color.black));
	}
	
	public void FadeInFromBlack ()
	{
		StartCoroutine(CrossFadeColor(Color.black, Color.clear));
	}
	
	IEnumerator CrossFadeColor (Color start, Color end)
	{
		for (float t = 0; t <= 1.0f; t += Time.deltaTime)
		{
			_fadePlane.color = Color.Lerp(start, end, t);
			yield return null;
		}
	}
	
	IEnumerator DespawnAsync (GameObject polaroid)
	{
		yield return new WaitForSeconds(1.5f);
		polaroid.GetComponent<Animator>().SetTrigger("Despawn");
		yield return new WaitForSeconds(0.25f);
		Destroy(polaroid);
	}
}
