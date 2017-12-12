﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailPieceScript : MonoBehaviour {

	// Variable for the tooltip that appears when hovering mouse over the object
	public GameObject trailTooltip;

	// Variables for the rotational and linear speeds of the frisbee (set in TrailHandler)
    public float rotSpeed = 0F;
    public float forwardSpeed = 0F;

	void Start() {

		// Find the tooltip object in the scene
		trailTooltip = GameObject.Find ("Trail Tooltip");
	}

	// Called when the mouse hovers over the object
	void OnMouseOver() {

		// Set the tooltip text to position and speed data
		trailTooltip.GetComponent<Text>().text = ("Position: " + transform.localPosition + "\nRot Speed: " + rotSpeed + "\nForward Speed: " + forwardSpeed);

		// Place the tooltip near the mouse cursor
		trailTooltip.transform.position = new Vector3 (Input.mousePosition.x+80, Input.mousePosition.y+25, Input.mousePosition.z);

		// Change the trail piece's color to green
		GetComponent<Renderer> ().material.color = Color.green;
	}

	// Called when the mouse exits the object
	void OnMouseExit() {

		// Set the tooltip text to nothing
		trailTooltip.GetComponent<Text>().text = ("");

		// Change the trail piece's color back to red
		GetComponent<Renderer> ().material.color = Color.red;
	} 
}