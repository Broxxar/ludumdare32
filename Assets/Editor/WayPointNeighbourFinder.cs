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

		Debug.Log (allWayPoints.Length);

		foreach (WayPoint tazdingo in allWayPoints) {
			if(tazdingo.GetType() == typeof(EventWayPoint)){
				Debug.Log ("event");
				continue;
			}
			tazdingo.neighbours.Clear();

			Collider2D[] candidates = Physics2D.OverlapPointAll((Vector2)tazdingo.transform.position + Vector2.up*4);
			CheckAndSetNeighbor(candidates, tazdingo);


			candidates = Physics2D.OverlapPointAll((Vector2)tazdingo.transform.position + Vector2.right*4);
			CheckAndSetNeighbor(candidates, tazdingo);

			candidates = Physics2D.OverlapPointAll((Vector2)tazdingo.transform.position - Vector2.up*4);
			CheckAndSetNeighbor(candidates, tazdingo);

			candidates = Physics2D.OverlapPointAll((Vector2)tazdingo.transform.position - Vector2.right*4);
			CheckAndSetNeighbor(candidates, tazdingo);

			EditorUtility.SetDirty(tazdingo);
		}
	}

	public static void CheckAndSetNeighbor(Collider2D[] candidates, WayPoint wp){
		
		foreach (Collider2D candidate in candidates){
			if (candidate.GetComponent<WayPoint>() != null){
				wp.neighbours.Add(candidate.GetComponent<WayPoint>());

			}
		}
	}
}