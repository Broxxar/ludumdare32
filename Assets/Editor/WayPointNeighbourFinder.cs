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



		foreach (WayPoint wp in allWayPoints) {

			wp.neighbours.Clear();

			Collider2D candidate = Physics2D.OverlapPoint((Vector2)wp.transform.position + Vector2.up*2);
			if (candidate != null){
				wp.neighbours.Add(candidate.GetComponent<WayPoint>());
			}

			candidate = Physics2D.OverlapPoint((Vector2)wp.transform.position + Vector2.right*2);
			if (candidate != null){
				wp.neighbours.Add(candidate.GetComponent<WayPoint>());
			}

			candidate = Physics2D.OverlapPoint((Vector2)wp.transform.position - Vector2.up*2);
			if (candidate != null){
				wp.neighbours.Add(candidate.GetComponent<WayPoint>());
			}
			
			candidate = Physics2D.OverlapPoint((Vector2)wp.transform.position - Vector2.right*2);
			if (candidate != null){
				wp.neighbours.Add(candidate.GetComponent<WayPoint>());
			}

			EditorUtility.SetDirty(wp);
		}
	}
}