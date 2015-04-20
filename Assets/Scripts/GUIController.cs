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

	public void SpawnPolaroid (Sprite photoSprite)
	{
		GameObject newPolaroid = GameObject.Instantiate(PolaroidPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPolaroid.name = PolaroidPrefab.name;
		newPolaroid.transform.SetParent(transform);
		
		newPolaroid.transform.FindChild("image").GetComponent<Image>().sprite = photoSprite;
		
		StartCoroutine(DespawnAsync(newPolaroid));
	}
	
	IEnumerator DespawnAsync (GameObject polaroid)
	{
		yield return new WaitForSeconds(1.5f);
		polaroid.GetComponent<Animator>().SetTrigger("Despawn");
		yield return new WaitForSeconds(0.25f);
		Destroy(polaroid);
	}
}
