using UnityEngine;
using System.Collections;

public enum Events{Start, RibbonCutting, HotDog, StealCandy, LudeActs, movingBetween}
public delegate void OnEventChangeHandler();

public class EventManager {

	private static EventManager _instance = null;    
	public event OnEventChangeHandler OnEventChange;
	public Events currentEvent { get; private set; }
	protected EventManager() {}

	public static EventManager Instance { 
		get {
			if (_instance == null) {
				_instance = new EventManager(); 
			}  
			return _instance;
		} 
	}

	public void SetEventState(Events newEvent) {
		this.currentEvent = newEvent;
		if(OnEventChange!=null) {
			OnEventChange();
		}
	}
}
