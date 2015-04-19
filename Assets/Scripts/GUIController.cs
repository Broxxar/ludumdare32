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

	public void SpawnPolaroid (Texture2D photoTexture)
	{
		GameObject newPolaroid = GameObject.Instantiate(PolaroidPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPolaroid.name = PolaroidPrefab.name;
		newPolaroid.transform.SetParent(transform);
		
		Sprite photoSprite = Sprite.Create(photoTexture, new Rect(0,0, 512, 512), Vector2.zero);
		
		newPolaroid.transform.FindChild("image").GetComponent<Image>().sprite = photoSprite;
	}
}
