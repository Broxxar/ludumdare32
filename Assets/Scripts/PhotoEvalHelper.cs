using UnityEngine;
using System.Collections;

public static class PhotoEvalHelper {


	public static bool EvaluatePhoto(Collider2D photoArea, Collider2D[] requiredItems, CircleCollider2D[] restrictedItems){


		int numRequiredObjects = 0;
		foreach (Collider2D coll in requiredItems){
			if(RequiredObjectContained(photoArea, coll)){
				numRequiredObjects ++;
			}
		}
		if(numRequiredObjects != requiredItems.Length){
			return false;
		}

		foreach(CircleCollider2D obj in restrictedItems){
			if(RestrictedObjectContained(photoArea, obj)){
				return false;
			}
		}

		return true;
	}

	static bool RequiredObjectContained(Collider2D photoArea, Collider2D obj){
		if(photoArea.bounds.SqrDistance(obj.bounds.center)>0){
			return false;
		}
		return true;
	}

	static bool RestrictedObjectContained(Collider2D photoArea, CircleCollider2D obj){
		if(photoArea.bounds.SqrDistance(obj.bounds.center)< obj.radius){
			return true;
		}
		return false;
	}


}
