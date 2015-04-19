using UnityEngine;
using System.Collections;

public class PoliticianWayPoint : MonoBehaviour {



	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<PoliticianController>()){
			other.GetComponent<PoliticianController>().IncrementRouteIndex();
		}
	}
}
