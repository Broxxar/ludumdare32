using UnityEngine;
using System.Collections;

public static class PhotoEvalHelper {

	static float RESRICTED_ALLOWANCE = 1.0f;

	public static bool EvaluatePhoto(Collider2D photoArea, EventWayPoint currentEvent, PoliticianState state){


		int numfoundObjects = 0;
		foreach (Collider2D coll in currentEvent.EventRequiredItems){

			if(coll != null && RequiredObjectContained(photoArea, coll)){
				numfoundObjects ++;
			}
		}

		if(numfoundObjects != currentEvent.EventRequiredItems.Length){
		
			return false;
		}

		foreach(CircleCollider2D obj in currentEvent.EventRestrictedItems){
			if(obj != null && RestrictedObjectContained(photoArea, obj)){
				return false;
			}
		}
	//	Debug.Log ("CurrentState: " + state + "RequiredState: " + currentEvent.RequiredState);
		if(state == currentEvent.RequiredState){
			return true;
		}

		return false;
	}

	static bool RequiredObjectContained(Collider2D photoArea, Collider2D obj){
	
		//check without Z
		Vector2 centerBB = photoArea.bounds.center;
		Vector2 topLeft = centerBB+ new Vector2(-photoArea.bounds.extents.x, photoArea.bounds.extents.y); 
		Vector2 bottomRight = centerBB+ new Vector2(photoArea.bounds.extents.x, -photoArea.bounds.extents.y); 
		Vector2 objCenter = obj.bounds.center;
		if(topLeft.x <= objCenter.x && objCenter.x <= bottomRight.x && topLeft.y >= objCenter.y && objCenter.y >= bottomRight.y){
			return true;
		}
		return false;
	}

	static bool RestrictedObjectContained(Collider2D photoArea, CircleCollider2D obj){

		Vector2 centerBB = photoArea.bounds.center;
		Vector2 topLeft = centerBB+ new Vector2(-photoArea.bounds.extents.x, photoArea.bounds.extents.y); 
		Vector2 bottomRight = centerBB+ new Vector2(photoArea.bounds.extents.x, -photoArea.bounds.extents.y); 
		Vector2 circleCenter = obj.bounds.center;
		float circleRadius = obj.radius;

		//Debug.Log ("topLeft: "+topLeft+" bottom right:"+bottomRight+" centerCircle: " + circleCenter + " circleRadius: "+ circleRadius);
		bool inside = (circleCenter.x >= topLeft.x-circleRadius) && (circleCenter.x <= bottomRight.x + circleRadius) && (circleCenter.y <= topLeft.y+circleRadius)&&(circleCenter.y >= bottomRight.y-circleRadius);
		//Debug.Log ("Was restricted in"+inside);
		return inside;

	}


}
