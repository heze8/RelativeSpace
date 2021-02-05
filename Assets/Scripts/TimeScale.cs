using System.Collections.Generic;
using UnityEngine;
 

//=========================================================================
// Class: TimeKeeper
// Mirrors Unity's Time.time, allowing the game to be logically paused
// while the animations, UI, and other Time.time dependant tasks keep running at the usual pace.
// TimeKeeper can be paused by the game when timestamp is set to 0,
// or by the user pressing the Pause/Break key.
//=========================================================================
public class TimeScale : Singleton<TimeScale> {
 
	// Variable: time
	// Access TimeKeeper.time to keep track of time with a different timescale then Time.time (read-only)
	public static float time;

	public class TimeScaleTracker<T>: MonoBehaviour where T : MonoBehaviour
	{
		public TimeScaleTracker(T observe)
		{
			
		}
	}
	// Variable: timescale
	// Current timescale of the TimeKeeper.
	public static float timescale {
		get { return Instance.mPaused ? 0 : Instance.mTimescale; }
		set { Instance.mTimescale = value; }
	}
 
	//=========================================================================
	private float mTime = 0.0f;
	private float mTimescale = 1.0f;
	private float mLastTimestamp = 0.0f;
	private bool  mPaused = false;

	public List<TimeScaleTracker<MonoBehaviour>> observing;
	//=========================================================================
	void Start ()
	{
	}
 
	//=========================================================================
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Pause))
			this.mPaused = !this.mPaused;
 
		float realDelta     = Time.realtimeSinceStartup - this.mLastTimestamp;
		this.mLastTimestamp = Time.realtimeSinceStartup;
		this.mTime += realDelta * timescale;
	}
}
