﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour {

	//Update runs 100 times per second?? (really?) so then 1.5sec is obv 150. suggested: long=150, short=50
	public int longBehind;
	public int shortBehind;
	public float throwThreshold;
	public float holdThreshold;

	public Transform trackingTarget;

	//delay between throws ( if the frisbee is laying in the net, don't start a new throw based off that stillness. )
	public float throwRate;
	private float nextThrow;


	private int throwMode; //0 = playback , 1 = waiting for throw , 2 = throw

	//queue 2: a longer queue for detecting the WAIT for THROW
	private Queue<Vector3> oldPosition = new Queue<Vector3>(); 

	//queue 1: a short queue for detecting THROW
	private Queue<Vector3> recentPosition = new Queue<Vector3>(); 

	void Start ()
	{
		//start program off in playback
		throwMode = 0;

		//populate oldPosition with zeroes. this will probably mess up the first few seconds of program start
		//but alternative is having it crash when it Dequeues a null?
		// TODO: set "calibrating" mode.
		for (int i=0; i<longBehind /*elemsBehind*/; i++){
			
			// doesn't matter if it's the same ref for both, no changing happens anyhow - "cannot be declared const" - unity
			Vector3 v3 = new Vector3 (0, 0, 0); 

			oldPosition.Enqueue (v3);
			if (i < shortBehind) {
				recentPosition.Enqueue (v3);
			}
		}
		//oldPosition.TrimToSize (); // not working -> not worth the effort

		//test for lab - could be used to make unity components less cluttered if works.
		//OptitrackRigidBody orb = gameObject.GetComponent<OptitrackRigidBody>();
		//Debug.Log ("orb==trackingtarget: "+(orb.transform == trackingTarget));
	}

	/*
	 * the dream:
	 * check distance between current position and position elemsBehind updates ago to see relative change
	 * and use that to distinguish between modes
	 */
	void Update() { //Update vs LateUpdate?
		Vector3 position = new Vector3 (trackingTarget.position.x, trackingTarget.position.y, trackingTarget.position.z);

		Vector3 positionShort = recentPosition.Dequeue ();
		Vector3 positionLong = oldPosition.Dequeue ();
		//Debug.Log ("short: "+positionShort);
		//Debug.Log ("long: "+positionLong);

		float shortAndLongDif = Vector3.Distance(positionShort, positionLong);
		float nowAndShortDif = Vector3.Distance(position, positionShort);

		float difference = (shortAndLongDif - nowAndShortDif);//2*1000;

		//0 = playback , 1 = waiting for throw , 2 = throw
		if (WaitingForThrow() && nowAndShortDif > throwThreshold) {
			throwMode = 2;
			Debug.Log("now throwing!");
			//throwstart = position OR alternatively  throwstart = positionshort, check which is better
		} 

		else if (InPlayback () && Time.time > nextThrow && difference < holdThreshold) {
			nextThrow = Time.time + throwRate;
			throwMode = 1;
			Debug.Log("now waiting for throw!");
		} 

		else if (Throwing ()) { 
			// TODO: check if "wall" has been breached
			Debug.Log("airborne OR hit wall?!");
			//let's just...
			// if distance ( throwstart, position ) > threshold ...
			throwMode = 0;
		}

		oldPosition.Enqueue (position);
		recentPosition.Enqueue (position);
	}


	/*
	 * returns true if game object is in playback mode, otherwise false. 
	 */
	public bool InPlayback ()
	{
		return throwMode == 0;
	}

	public bool WaitingForThrow()
	{
		return throwMode == 1;
	}

	public bool Throwing ()
	{
		return throwMode == 2;
	}

	//shorthand
	public int GetMode () 
	{
		return throwMode;
	}
}
