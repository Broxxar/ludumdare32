using UnityEngine;
using System.Collections;

public class PoliticianWayPoint : WayPoint {



	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<PoliticianController>()){
			other.GetComponent<PoliticianController>().IncrementRouteIndex();
		}
	}
}
