using UnityEngine;
using System.Collections;

public static class PhotoEvalHelper
{
	public static bool EvaluatePhoto(PlayerVirtualCamera virtualCam, EventWayPoint currentEvent, PoliticianState state)
	{
		if (!virtualCam.ContainsAll(currentEvent.RequiredItems))
			return false;

		if (virtualCam.ContainsAny(currentEvent.RestrictedItems))
			return false;

		if(state == currentEvent.RequiredState)
			return true;

		return false;
	}
	
	public static bool ValidatePhoto(Collider2D photoArea, EventWayPoint currentEvent)
	{
		return (Vector2.Distance (photoArea.transform.position, currentEvent.transform.position) < 4);
	}
}
