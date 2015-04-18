using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MenuItems
{
	[MenuItem("Tools/Find Neighbours of Way Points")]
	public static void NewMenuOption()
	{
		WayPoint[] allWayPoints = GameObject.FindObjectsOfType<WayPoint> ();

		Debug.Log ("entered function");

		foreach (WayPoint wp in allWayPoints) {

			wp.neighbours.Clear();

			Collider2D[] candidates = Physics2D.OverlapPointAll((Vector2)wp.transform.position + Vector2.up*4);

			foreach (Collider2D candidate in candidates){
				if (candidate.GetComponent<WayPoint>() != null){
					wp.neighbours.Add(candidate.GetComponent<WayPoint>());
				}
			}

			candidates = Physics2D.OverlapPointAll((Vector2)wp.transform.position + Vector2.right*4);
			foreach (Collider2D candidate in candidates){
				if (candidate.GetComponent<WayPoint>() != null){
					wp.neighbours.Add(candidate.GetComponent<WayPoint>());
				}
			}

			candidates = Physics2D.OverlapPointAll((Vector2)wp.transform.position - Vector2.up*4);
			foreach (Collider2D candidate in candidates){
				if (candidate.GetComponent<WayPoint>() != null){
					wp.neighbours.Add(candidate.GetComponent<WayPoint>());
				}
			}

			candidates = Physics2D.OverlapPointAll((Vector2)wp.transform.position - Vector2.right*4);
			foreach (Collider2D candidate in candidates){
				if (candidate.GetComponent<WayPoint>() != null){
					wp.neighbours.Add(candidate.GetComponent<WayPoint>());
				}
			}

			EditorUtility.SetDirty(wp);
		}
	}
}