using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum StoryEvents{RibbonCutting, HotDog, StealCandy, LudeActs, movingBetween}
public delegate void OnEventChangeHandler();

public class EventManager : MonoBehaviour{

	private static EventManager _instance = null;    
	public event OnEventChangeHandler OnEventChange;
	public StoryEvents currentEvent { get; private set; }
	public List<EventWayPoint> eventWPs;
	 
	protected EventManager() {}

	public static EventManager Instance { 
		get {
			return _instance ?? GameObject.FindObjectOfType<EventManager>();
		} 
	}

	public void SetEventState(StoryEvents newEvent) {
		this.currentEvent = newEvent;
		if(OnEventChange!=null) {
			OnEventChange();
		}
	}
}
